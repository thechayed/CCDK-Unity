using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CCDKEngine;
using UnityEngine.AI;
using Unity.Netcode;

namespace CCDKGame
{
    public class Pawn : PossessableObject
    {
        [Header(" - Pawn - ")]
        [ReadOnly] public CCDKObjects.Pawn pawnData;
        public Camera pawnCamera;

        [Header(" - Inventory - ")]
        [Tooltip("A list of all the Items currently in the Pawn's inventory.")]
        public List<CCDKObjects.InventoryItem> inventory;
        [Tooltip("A list of all the Weapons currently in the Pawn's inventory.")]
        public List<CCDKObjects.Weapon> equipedWeapons;
        [Tooltip("Active Weapon Slot is useful when needing an index to replace a weapon, for example, replacing the equiped one with one from the world.")]
        public int activeWeaponSlot = 0;
        [Tooltip("Whether the Pawn should use only one Item slot and automatically pick it.")]
        public bool automateWeaponSlot = true;

        public List<Weapon> activeWeapons;

        [Header(" - Movement - ")]
        public CharacterController characterController;
        public bool moveWithCharacterController = false;
        public Vector3 velocity = new Vector3(0,0,0);
        Vector3 speed;

        [Header(" - Components - ")]
        public Transform baseTransform = null;

        [Header(" - Navmesh Agent - ")]
        public NavMeshAgent agent;
        public Vector3 agentDestination;
        public Transform agentFollowTransform;
        public bool agentFollow = false;

        public override void Start()
        {
            base.Start();

            agentDestination = transform.position;
            pawnData = (CCDKObjects.Pawn)data;

            baseTransform = transform.Find(pawnData.linkedChild);

            if (baseTransform == null)
                baseTransform = transform;

            Engine.AddPawn(gameObject);

            inventory = pawnData.inventoryInfo.inventory;

            if (pawnData.NavMeshAgent)
            {
                SetUpNavAgent();
            }

            characterController = gameObject.GetComponent<CharacterController>();
            if(characterController == null)
                characterController = gameObject.AddComponent<CharacterController>();

            /**If the Pawn is State Enabled, Add it to the Game Type's StateObjectPairing.**/
            if(pawnData.stateEnabled)
            {
                stateEnabled = true;
            }
        }

        public override void Update()
        {
            base.Update();
            //Loop through the inventory to fill any empty weapon slots
            foreach (CCDKObjects.InventoryItem item in inventory)
            {
                if (((CCDKObjects.Weapon)item) != null)
                {
                    if (equipedWeapons.Count < pawnData.inventoryInfo.weaponSlots)
                    {
                        EquipWeapon((CCDKObjects.Weapon)item);

                        if (automateWeaponSlot&&activeWeapons.Count < 1)
                        {
                            ActivateWeapon(0);
                        }
                    }
                }
            }

            //foreach(Weapon weapon in activeWeapons)
            //{
            //    if(weapon.transform.parent != transform)
            //    {
            //        weapon.transform.SetParent(transform);
            //    }
            //}

            if(pawnData == null)
                pawnData = (CCDKObjects.Pawn)data;

            if (agent == null)
                if (pawnData.NavMeshAgent)
                    SetUpNavAgent();

            if (agent != null)
            {
                if (agentFollow)
                {
                    if (agentFollowTransform != null)
                    {
                        agent.destination = agentFollowTransform.position;
                    }
                }
                else if (agentDestination != null)
                {
                    agent.destination = agentDestination;
                }
            }


            /**If no local Players own this pawn, disable it's Audio Listener.**/
            if (pawnCamera != null)
                if (net != null)
                    if (net.IsOwner)
                        pawnCamera.GetComponent<AudioListener>().enabled = true;
                    else
                        pawnCamera.GetComponent<AudioListener>().enabled = false;

            if(agent != null)
            {
                /**Set the Moving speed of the Pawn's NavMesh Agent**/
                agent.speed = pawnData.movement.GroundSpeed;
                agent.angularSpeed = pawnData.movement.rotationSpeed;
            }

            if (moveWithCharacterController)
            {
                Vector3 velocityTo = (characterController.velocity.Lerp(velocity, pawnData.movement.AccelRate)/2).Round()*2;
                velocityTo.y = pawnData.movement.MaxFallSpeed;
                characterController.SimpleMove(velocityTo);
#if USING_NETCODE
                if(NetworkManager.Singleton != null)
                {
                    if (NetworkManager.Singleton.IsHost)
                    {
                        MovePlayerClientRPC(velocityTo);
                    }
                    else
                        if (net.IsOwner)
                            MovePlayerServerRPC(velocityTo);
                }
#endif
            }
        }

        private void OnEnable()
        {
            PawnManager.GetPawn(this);
        }

        /** Override this function for Pawn classes to set their own default classes when spawned into the game **/
        public virtual Dictionary<string> GetDefaultClasses()
        {
            return null;
        }

        //public void Reset()
        //{
        //    componentConstructor.RemovePreviousClasses();
        //}

        //public override bool SetController(Controller controller)
        //{
        //    if (pawnData.possessable)
        //    {
        //        if(this.controller == null)
        //        {
        //            this.controller = controller;

        //            base.SetControllerClientRPC(controller.GetComponent<NetworkObject>().NetworkObjectId);

        //            return true;
        //        }
        //    }
        //    return false;
        //}


        public void DestroySelf()
        {
            DestroyImmediate(this,true);
        }

        private new void OnDestroy()
        {
            Engine.RemovePawn(gameObject);
            PawnManager.RemovePawn(this);
        }

        public void EquipWeapon(CCDKObjects.Weapon weaponData, int slot = -1)
        {
            if(slot==-1)
            {
                equipedWeapons.Add((CCDKObjects.Weapon)weaponData);
            }
            else
            {
                equipedWeapons.Insert(slot, weaponData);
                equipedWeapons.RemoveAt(slot+1);
            }
        }

        /**<summary>Activating a weapon spawns it into the game world for the Player</summary>**/
        public void ActivateWeapon(int slot)
        {
            activeWeapons.Add(Instantiate(equipedWeapons[slot].prefab).GetComponent<Weapon>());
            activeWeapons[activeWeapons.Count - 1].pawn = this;
            activeWeapons[activeWeapons.Count - 1].transform.SetParent(transform);
        }

        public void FireWeapon(object[] fireInfo)
        {
            Debug.Log((Transform)fireInfo[0]);
            Transform direction = (Transform)fireInfo[0];
            int weapon = (int)fireInfo[1];
            int fireType = (int)fireInfo[2];

            if (direction == null)
            {
                direction = transform;
            }
            activeWeapons[weapon].Fire(direction, fireType);
        }

        public void FireWeapon(Transform direction = null, int weapon = 0, int fireType = 0)
        {
            Debug.Log("Firehasbeencalled!");

            if(direction == null)
            {
                direction = transform;
            }
            activeWeapons[weapon].Fire(direction,fireType);
        }


        public void AgentMoveToPosition(Vector3 destination)
        {
            agentDestination = destination;
            agentFollow = false;
        }
        public void AgentFollowTransform(Transform follow)
        {
            agentFollowTransform = follow;
            agentFollow = true;
        }

        public void SetCamera(Camera camera)
        {
            pawnCamera = camera;
        }

        public void SetUpNavAgent()
        {
                agent = baseTransform.gameObject.GetComponent<NavMeshAgent>();
                if (agent == null)
                {
                    agent = baseTransform.gameObject.AddComponent<NavMeshAgent>();
                }
        }

        public void SimpleMoveOnGround(Vector3 direction)
        {
            if(characterController != null)
            {
                velocity = direction*pawnData.movement.GroundSpeed;
            }
            else
            {
                Debug.Log("Tried to move the Pawn with Simple Move, but the Pawn doesn't have a Character Controller applied.");
            }
            moveWithCharacterController = true;
        }


#if USING_NETCODE

        [ClientRpc]
        public void MovePlayerClientRPC(Vector3 velocity)
        {
            if(characterController != null)
                characterController.SimpleMove(velocity);
            else
                characterController = gameObject.GetComponent<CharacterController>();
        }

        [ServerRpc]
        public void MovePlayerServerRPC(Vector3 velocity)
        {
            if (characterController != null)
                characterController.SimpleMove(velocity);
            else
                characterController = gameObject.GetComponent<CharacterController>();
            
            MovePlayerClientRPC(velocity);
        }

#endif
    }
}
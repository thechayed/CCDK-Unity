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
        /**The Pawn's Camera**/
        public Camera pawnCamera;

        [Header(" - Iventory - ")]
        /** A list of all the Items currently in the Pawn's inventory **/
        public List<CCDKObjects.InventoryItem> inventory;

        public List<CCDKObjects.Weapon> equipedWeapons;
        /**Active Weapon Slot is useful when needing an index to replace a weapon, for example, replacing the equiped one with one from the world.**/
        public int activeWeaponSlot = 0;
        /**<summary>Whether the Pawn should use only one Item slot and automatically pick it</summary>**/
        public bool automateWeaponSlot = true;

        public List<Weapon> activeWeapons;

        [Header(" - Movement - ")]
        public CharacterController characterController;
        Vector3 speed;

        [Header(" - Built Components - ")]
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

        public void Fire(Transform direction = null, int weapon = 0, int fireType = 0)
        {
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
    }
}
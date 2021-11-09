/* The object class is a MonoBehavior that should be extended for CC objects that include basic values and functionaliy
 * to simplify development */

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CCDKGame;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

#if USING_NETCODE
using Unity.Netcode;
using Unity.Netcode.Components;
#endif

namespace CCDKEngine
{
    public class Object : FSM.Component
    {
        [Header(" - Object - ")]

        [Header(" - (Object) Basic Info - ")]
        [Tooltip("The Level that this Game Object belongs to.")]
        [HideInInspector] public GameObject levelObj;
        [Tooltip("The Level the object is in.")]
        [ReadOnly] public Level level;
        [Tooltip("Whether the Object is enabled in for use in the CCDK.")]
        public bool ccdkEnabled = true;
        [Tooltip("Should this Object only be enabled while it's Parent Game Type State is active?.")]
        public bool stateEnabled = false;
        private bool addedToStateList = false;
        [Tooltip("The Game Type State that this Object was created during.")]
        public string gameTypeState = "";
        [Tooltip("Whether the Game Type State has been set yet or not.")]
        private bool setGameTypeState = false;
        [Tooltip("The Object does not belong to a Level.")]
        [ReadOnly] public bool Independent = false;
        [Tooltip("Whether this Object should automatically move to the Engine scene if it isn't currently there.")]
        [ReadOnly] public bool engineObject = false;
        [Tooltip("An optional value to pass to an Object to keep track of where it was created from in script.")]
        public string originMethod = "Unknown";
        public int originLine = 0;
        [Tooltip("The amount of time that this Projectile has existed.")]
        public float life = 0.0f;

        [Header(" - (Object) Damage - ")]
        [Tooltip("The amount of health this Object currently has.")]
        public float health = 100f;
        [Tooltip("The Controller that caused Damage to this Object last.")]
        public Controller lastHitBy;
        [Tooltip("The Controller that destroyed this Object.")]
        public Controller destroyedBy;
        [Tooltip("The amount of damage this Object should deal when colliding with another Object.")]
        public float damageOnImpact = 0f;
        [Tooltip("Whether this object should damage other objects, over time, while they are colliding. (IE a pool of Lava.)")]
        public float damageOverTime = 0f;
        [Tooltip("The amount of damage to deal to self when applying Damage to another Object. In the case of a projectile, this will destroy the projectile on impact.")]
        public float damageSelf = 0;
        [Tooltip("If set, this Object will pass status effects to a Damage Reciever when they collide.")]
        public string statusEffect = "";
        [Tooltip("Whether this object should be damaged by Damage Instegators when they collide.")]
        public bool damageReciever = false;
        [Tooltip("A List of status effects that have been inflicted on this Object.")]
        public List<string> inflictedStatusEffects = new List<string>();
        [Tooltip("All the Game Objects that this Object is currently Colliding with.")]
        public List<GameObject> currentCollisions = new List<GameObject>();

        [Header(" - (Object) Multiplayer - ")]
#if USING_NETCODE
        [Tooltip("The ID of the client this Object belongs to. If it wasn't propperly given to it's own yet, it will in the next Update.")]
        public ulong clientID = 0;
        [Tooltip("Whether this object belongs to a client in Multiplayer.")]
        public bool spawnAsClientObject = false;
#endif
        [Tooltip("Whether this object is Reprecated with MLAPI.")]
        public bool replicate;

#if USING_NETCODE
        [Tooltip("The Networked Object Component added to this object to interface with MLAPI.")]
        public NetworkObject net;
        public NetworkTransform netTransform;
        public bool netInitialized = false;
        public bool netEnabled = false;
        [Tooltip("Whether this Object has spawned yet.")]
        public NetworkVariable<bool> spawned = default;
#endif

        //Gameplay Values
        /**The engine has told us play has begun, be ready!**/
        [Tooltip("The engine has told us play has begun, be ready!")]
        [HideInInspector]
        public bool ready;
        /**Whether the object should be ready when the game starts; This value should be set to false on Awake if so desired **/
        [HideInInspector]
        public bool readyOnStart = true;
        /**Whether the object should be visible (visibility functionality should be handled in extending scripts)**/
        [HideInInspector]
        public bool visible = true;



        public virtual void Awake()
        {


            /* If the object should be ready after play has begun, set to true */
            if (readyOnStart)
            {
                if (Engine.running&&LevelManager.GetInLevel())
                {
                    ready = true;
                }
            }

            Engine.NetworkDisconnect += NetworkEnd;
            Engine.NetworkClientJoined += NetworkClientJoined;
            Engine.NetworkClientLeft += NetworkClientLeft;


        }

        /**<summary>Try not to use the built in Update method for most things.</summary>**/
        public virtual void Update()
        {
            if(!setGameTypeState&&Engine.currentGameType!=null)
            {
                gameTypeState = Engine.currentGameType.state;
                setGameTypeState = true;
            }

#if USING_NETCODE
            if(net!=null&& NetworkManager!=null)
                if (net.IsOwner|!NetworkManager.IsClient)
                {
                    NetworkUpdate();
                }
#else
            NetworkUpdate();
#endif

            /**If this is a State Enabled object, add it to the Game Type's State-Object Pairing Dictionary and enable/disable based on the State of the GameType.**/
            if (stateEnabled && Engine.currentGameType.stateObjectPairingEnabled)
            {
                if (!addedToStateList)
                {
                    Engine.currentGameType.AddObjectToState(this, gameTypeState);
                    addedToStateList = true;
                }

                if (Engine.currentGameType.state != gameTypeState)
                    ccdkEnabled = false;
                else
                    ccdkEnabled = true;
            }
        }

        public override void FixedUpdate()
        {
            if (engineObject)
                if (gameObject.scene != LevelManager.engineScene)
                    SceneManager.MoveGameObjectToScene(gameObject,LevelManager.engineScene);

            if (Engine.singleton.initialized&&Engine.singleton.data.startingLevelLoaded)
            {
                /** If an Object has been Created outside of the Engine, references won't be automatically created between it and the Level, fix this now. **/
                if (level == null&&!Independent)
                {
                    LevelManager.AcknowledgeObject(gameObject);
                }
            }
            
            if(data!=null&&((CCDKObjects.PrefabSO)data)!=null)
            {
              replicate=((CCDKObjects.PrefabSO)data).replicate;
            }

            /**Add deltaTime to lifespan to track how long this object has been alive.**/
            life += Time.deltaTime;

#if USING_NETCODE


            if (replicate)
            {

                if (net == null)
                {
                    AddNetworkComponents();
                }
                else
                {
                    net.AutoObjectParentSync = false;

                    if (NetworkManager.Singleton != null)
                        if (NetworkManager.Singleton.IsHost && NetworkManager.GetNetworkPrefabOverride(gameObject) != gameObject)
                            NetworkManager.Singleton.PrefabHandler.RegisterHostGlobalObjectIdHashValues(gameObject, new List<GameObject>() { gameObject });

                }

                if (NetworkManager.Singleton)
                {

                    if (NetworkManager.Singleton.IsClient)
                    {
                        if (!netInitialized)
                            NetworkStart();

                        if(NetworkManager.Singleton.IsHost)
                            if (!net.IsSpawned)
                            {
                                if(!spawnAsClientObject)
                                    net.Spawn();
                                else
                                    GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);
                            }

                        if (clientID != net.OwnerClientId&&spawnAsClientObject)
                            net.ChangeOwnership(clientID);

                    }
                }
            }
#endif
        }

        public void AppendToLevel(GameObject levelObj)
        {
            this.levelObj = levelObj;
            level = levelObj.GetComponent<Level>();
        }

        /**<summary>The Update is only called on "Owned" Objects. This includes all Objects in Singleplayer and Server Set Owned objects in Netcode.</summary>**/
        public virtual void NetworkUpdate()
        {

        }

        public virtual void NetworkStart()
        {
            netInitialized = true;
        }

        public virtual void NetworkEnd()
        {
            if (gameObject == null)
            {
                Engine.NetworkDisconnect -= NetworkEnd;
                return;
            }
            net = gameObject.GetComponent<NetworkObject>();
            netTransform = gameObject.GetComponent<NetworkTransform>();

            if (net == null)
                return;

            net.enabled = false;
            netTransform.enabled = false;
        }

        public virtual void NetworkClientJoined(ulong clientID)
        {

        }

        public virtual void NetworkClientLeft(ulong clientID)
        {
            if (spawnAsClientObject)
                if (this.clientID == clientID)
                    Destroy(gameObject);
        }

        public void AddNetworkComponents()
        {
#if USING_NETCODE
            net = gameObject.GetComponent<NetworkObject>();
            if (net != null)
                return;

            net = gameObject.AddComponent<NetworkObject>();
            net.AutoObjectParentSync = false;
            netTransform = gameObject.AddComponent<NetworkTransform>();
            netEnabled = true;
#endif
        }

        private new void OnDestroy()
        {
            Engine.NetworkConnect -= NetworkStart;
            Engine.NetworkDisconnect -= NetworkEnd;
        }

        public void SetOrigin([CallerLineNumber] int lineNumber = 0, [CallerFilePath] string caller = "Unknown", [CallerMemberName] string method = "Unknown")
        {
            originLine = lineNumber-1;
            string[] nameWords = caller.Split('\\');
            originMethod = nameWords[nameWords.Length-1]+": "+method;
        }


        void OnCollisionEnter(Collision collision)
        {
            /**If the other object in the collision is a Damage Instegator, and this is a Damage Reciever, call the Hit method and pass the Collision for extended functionality.**/
            if (damageReciever)
            {
                Object asObject = collision.gameObject.GetComponent<Object>();
                if (asObject != null)
                    if (asObject.damageOnImpact>0f)
                        Hit(collision);
            }

            /**Add the object to the List of active Collisions.**/
            currentCollisions.Add(collision.gameObject);
        }

        void OnCollisionExit(Collision collision)
        {
            /**Remove the object from the list of active Collisions.**/
            currentCollisions.Remove(collision.gameObject);
        }
        
        /**<summary>A branch of On Collision Enter for Damage Instegator collisions.</summary>**/
        public void Hit(Collision collision)
        {
            Object collisionObject = collision.gameObject.GetComponent<Object>();
            Damage(collisionObject.damageOnImpact);

            if (collisionObject.damageReciever)
                Damage(collisionObject.health -= collisionObject.damageSelf);
        }

        /**<summary>Apply damage to the Object</summary>**/
        public void Damage(float amount, Controller controller = null)
        {
            health -= amount;

            lastHitBy = controller;

            if (health <= 0)
                Destroy(controller);

#if USING_NETCODE
            if(NetworkManager.IsHost)
                SetHealthClientRPC(health);
#endif
        }

        /**<summary>Destroy the Object locally.</summary>**/
        public virtual void Destroy(Controller controller = null)
        {
            destroyedBy = controller;

            if (NetworkManager.IsHost)
                DestroyClientRPC(controller.GetComponent<NetworkObject>().NetworkObjectId);

            GameObject.Destroy(gameObject);
        }

#if USING_NETCODE
        [ClientRpc]
        /**<summary>Set the Health of the Object on the client's side when it is Updated.</summary>**/
        public void SetHealthClientRPC(float health)
        {
            if (!NetworkManager.Singleton.IsHost)
                this.health = health;
        }

        [ClientRpc]
        public void DestroyClientRPC(ulong IDOfDestroyer)
        {
            if (!NetworkManager.Singleton.IsHost)
                Destroy(NetworkManager.SpawnManager.SpawnedObjects[IDOfDestroyer].GetComponent<Controller>());
        }
#endif
    }
}

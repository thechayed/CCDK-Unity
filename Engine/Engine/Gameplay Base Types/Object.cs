/* The object class is a MonoBehavior that should be extended for CC objects that include basic values and functionaliy
 * to simplify development */

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CCDKGame;
using System.Runtime.CompilerServices;

#if USING_NETCODE
using Unity.Netcode;
using Unity.Netcode.Components;
#endif

namespace CCDKEngine
{
    public class Object : FSM.Component
    {
        [Header(" - Object Properties - ")]

        [Tooltip("The Unique ID (in the CCDK) of this Object.")]
        [ReadOnly] public int UID;
        [Tooltip("The Object does not belong to a Level.")]
        [ReadOnly] public bool Independent = false;

        /**<summary>An option value to pass to an Object to keep track of where it was created from in script.</summary>**/
        public string originMethod = "Unknown";
        public int originLine = 0;

        /**<summary>The Level that this Game Object belongs to</summary>**/
        [HideInInspector] public GameObject levelObj;
        [Tooltip("The Level the object is in.")]
        [ReadOnly] public Level level;

        [Header(" - Multiplayer - ")]
        /**<summary>Whether this object is Reprecated with MLAPI.</summary>**/
        public bool replicate;

#if USING_NETCODE
        /**<summary>The Networked Object Behavior added to this object to interface with MLAPI.</summary>**/
        public NetworkObject net;
        public NetworkTransform netTransform;
#endif

        //Gameplay Values
        /**The engine has told us play has begun, be ready!**/
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

            Engine.NetworkConnect += NetworkStart;
            Engine.NetworkDisconnect += NetworkEnd;
        }

        public override void FixedUpdate()
        {
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

#if USING_NETCODE

            if (replicate)
            {

                if(net == null)
                {
                    AddNetworkComponents();
                }
                else
                {
                    net.AutoObjectParentSync = false;
                    if(NetworkManager.Singleton!=null)
                        if(NetworkManager.Singleton.IsHost)
                            NetworkManager.Singleton.PrefabHandler.RegisterHostGlobalObjectIdHashValues(gameObject, new List<GameObject>() { gameObject });
                }

                
                if(net.IsLocalPlayer)
                {
                    NetworkUpdate(); 
                }
            }



#else
            NetworkUpdate();
#endif
        }

        public void AppendToLevel(GameObject levelObj)
        {
            this.levelObj = levelObj;
            level = levelObj.GetComponent<Level>();
        }

        /**The Update is only called on "Local" Objects.**/
        public virtual void NetworkUpdate()
        {

        }

        public virtual void NetworkStart()
        {
            
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

        public void AddNetworkComponents()
        {
#if USING_NETCODE
            net = gameObject.GetComponent<NetworkObject>();
            if (net != null)
                return;

            net = gameObject.AddComponent<NetworkObject>();
            net.AutoObjectParentSync = false;
            netTransform = gameObject.AddComponent<NetworkTransform>();

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
    }
}

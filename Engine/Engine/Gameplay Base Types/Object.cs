/* The object class is a MonoBehavior that should be extended for CC objects that include basic values and functionaliy
 * to simplify development */

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CCDKGame;

namespace CCDKEngine
{
    public class Object : FSM.Component
    {
        [Header(" - Object Properties - ")]

        [Tooltip("The Unique ID (in the CCDK) of this Object.")]
        [ReadOnly] public int UID;
        [Tooltip("The Object does not belong to a Level.")]
        [ReadOnly] public bool Independent = false;

        /**<summary>The Level that this Game Object belongs to</summary>**/
        [HideInInspector] public GameObject levelObj;
        [Tooltip("The Level the object is in.")]
        [ReadOnly] public Level level;

        /**<summary>Whether this object is Reprecated with MLAPI.</summary>**/
        public bool replicate;

#if USING_NETCODE
        /**<summary>The Networked Object Behavior added to this object to interface with MLAPI.</summary>**/
        public NetworkedObject net;
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
            
            if(data!=null)
            {
              replicate=data.replicate;
            }

#if USING_NETCODE
            if (replicate)
            {
                if(net == null)
                {
                    net = gameObject.AddComponent<NetworkedObject>();
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
    }
}

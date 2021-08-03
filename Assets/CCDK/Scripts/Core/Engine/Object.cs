/* The object class is a MonoBehavior that should be extended for CC objects that include basic values and functionaliy
 * to simplify development */

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CCDKEngine
{
    public class Object : FSM.Component
    {
        //Gameplay Values
        /**The engine has told us play has begun, be ready!**/
        public bool ready;
        /**Whether the object should be ready when the game starts; This value should be set to false on Awake if so desired **/
        public bool readyOnStart = true;
        /**Whether the object should be visible (visibility functionality should be handled in extending scripts)**/
        public bool visible = true;

        public virtual void Awake()
        {
            /* If the object should be ready after play has begun, set to true */
            if (readyOnStart)
            {
                if (Engine.runtimeInit&&LevelManager.GetInLevel())
                {
                    ready = true;
                }
            }
        }
    }
}

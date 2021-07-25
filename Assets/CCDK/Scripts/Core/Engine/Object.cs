/* The object class is a MonoBehavior that should be extended for CC objects that include basic values and functionaliy
 * to simplify development */

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CCDKEngine
{
    public class Object : LASEMono
    {
        //Gameplay Values
        /**The engine has told us play has begun, be ready!**/
        public bool ready;
        /**Whether the object should be ready when the game starts; This value should be set to false on Awake if so desired **/
        public bool readyOnStart = true;
        /**Whether the object should be visible (visibility functionality should be handled in extending scripts)**/
        public bool visible = true;

        /** The unique components necesarry for this Object **/
        private List<string> components = new List<string>() {"None"};

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

        public override void Start()
        {
            base.Start();
            /** Create and add all the components that this object was told to add **/
            /** to add Components to the list in the object's class, override Start with base.Start() after adding **/
            if (components != null)
            {
                foreach (string component in components)
                {
                    if (Type.GetType(component) != null)
                    {
                        gameObject.AddComponent(Type.GetType(component));
                    }
                }
            }
        }

        //public void AddComponent(string name)
        //{
        //    components.Add(name);
        //}
    }
}

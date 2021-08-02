using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class Pawn : PossessableObject
    {
        /** Whether the Constructor method has already been called **/
        [HideInInspector]
        public bool constructed;

        public CCDKObjects.Pawn data;

        public ComponentConstructor<Script.PawnClass> componentConstructor;

        public bool possessable = true;

        /** Create children Functionality Classes that enable the Pawn's behavior in the game **/
        public void PawnConstructer(Dictionary<string> newClasses = null)
        {
            /** Initialize the Component Constructor **/ 
            componentConstructor = new ComponentConstructor<Script.PawnClass>(gameObject, data.baseInfo.classes, PawnManager.classDefaults);

            /** Loop through all the Pawn's SubComponents to set the Controller and Pawn values. **/
            foreach (DictionaryItem<Script.PawnClass> item in componentConstructor.classNames.dictionary)
            {
                PawnClass pawnClass = (PawnClass)gameObject.GetComponent(Type.GetType(item.value.ToString()));
                pawnClass.controller = controller;
                pawnClass.pawn = this;
            }
        }

        /** Override this function for Pawn classes to set their own default classes when spawned into the game **/
        public virtual Dictionary<string> GetDefaultClasses()
        {
            return null;
        }

        public void Reset()
        {
            componentConstructor.RemovePreviousClasses();
        }

        public bool SetController(Controller controller)
        {
            if (possessable)
            {
                if(this.controller == null)
                {
                    this.controller = controller;

                    /** Loop through all the Pawn's SubComponents to set the Controller values. **/
                    foreach (DictionaryItem<Script.PawnClass> item in componentConstructor.classNames.dictionary)
                    {
                        PawnClass pawnClass = (PawnClass)gameObject.GetComponent(Type.GetType(item.value.ToString()));
                        pawnClass.controller = controller;
                    }
                }
            }
            return false;
        }
    }
}
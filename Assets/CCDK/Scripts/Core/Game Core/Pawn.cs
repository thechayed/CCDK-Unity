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

        public ComponentConstructor componentConstructor;

        /** Override this function as a replacement for Start **/
        public override void Start()
        {
            base.Start();
            /** If the Pawn hasn't be completely constructed, construct it on StartUp! **/
            //if (gameObject.GetComponent<PawnMovement>() == null || gameObject.GetComponent<PawnLife>() == null || gameObject.GetComponent<PawnInventoryManager>() == null || gameObject.GetComponent<PawnAudio>() == null)
            //{
            //    PawnConstructer();
            //    Debug.LogWarning("Pawn " + data.baseInfo.pawnName + ": has not be Constructed! Constructing now!");
            //}
        }

        ///** The Pawn constructor allows a Pawn of any type to be created anywhere **/
        //public Pawn(bool construct = false, StringsDictionary newClasses = null)
        //{
        //    if (construct)
        //    {
        //        data.baseInfo.classes = newClasses;
        //        PawnConstructer();
        //    }
        //}

        /** Create children Functionality Classes that enable the Pawn's behavior in the game **/
        public void PawnConstructer(Dictionary<string> newClasses = null)
        {
            /** Initialize the Component Constructor **/ 
            componentConstructor = new ComponentConstructor(gameObject, data.baseInfo.classes, PawnManager.classDefaults);

            /** Loop through all the Pawn's SubComponents to set the Controller and Pawn values. **/
            foreach (DictionaryItem<string> item in componentConstructor.classNames.dictionary)
            {
                PawnClass pawnClass = (PawnClass)gameObject.GetComponent(Type.GetType(item.value));
                pawnClass.controller = controller;
                pawnClass.pawn = this;
            }

            /** Set State info **/
            state = data.baseInfo.state;
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
    }
}
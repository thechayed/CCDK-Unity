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
        public void PawnConstructer(StringsDictionary newClasses = null)
        {
            /** Make sure that the Classes exist **/
            ValidateClasses();
            /** Remove components that have been previously added **/
            RemovePreviousClasses();
            /** Add the Component Classes **/
            AddClassComponents();
            /** Set State info **/
            state = data.baseInfo.state;
        }

        /** Called by the Constructer to make sure that all the classes are valid **/
        public void ValidateClasses()
        {
            if (data.baseInfo.classes.length > 0)
            {
                foreach (StringsDictionaryItem item in data.baseInfo.classes.dictionary)
                {                
                    /** Check if any of the classes can't be found, alert the user and change the Class name to default **/
                    if (Type.GetType(item.value) == null)
                    {
                        Debug.LogWarning("Pawn " + data.baseInfo.pawnName + ": User defined "+item.key+" couldn't be found, rolling back to default "+PawnManager.classDefaults.Get(item.key)+"!");
                        data.baseInfo.classes.Set(item.key, PawnManager.classDefaults.Get(item.key));
                    }
                }
            }
            else
            {
                /** If there are no classes added, load Classes from Defaults **/
                Debug.LogWarning("Pawn's classes do not exist, filling with defaults!");
                data.baseInfo.classes.Load(PawnManager.classDefaults);
            }
        }

        /** Called by the Constructer to remove previously added components **/
        public void RemovePreviousClasses()
        {
            foreach(StringsDictionaryItem item in PawnManager.classDefaults.dictionary)
            {
                if (Type.GetType(item.key) != null)
                {
                    DestroyImmediate(GetComponent(Type.GetType(item.key)));
                }
            }
        }

        /** Called by the Constructor to Add Component classes and set their PawnClass values **/
        public void AddClassComponents()
        {
            foreach(StringsDictionaryItem item in data.baseInfo.classes.dictionary)
            {
                gameObject.AddComponent(Type.GetType(item.value));
                PawnClass pawnClass = (PawnClass) gameObject.GetComponent(Type.GetType(item.value));
                pawnClass.controller = controller;
                pawnClass.pawn = gameObject.GetComponent<Pawn>();
            }
        }


        /** Override this function for Pawn classes to set their own default classes when spawned into the game **/
        public virtual StringsDictionary GetDefaultClasses()
        {
            return null;
        }

        public void Reset()
        {
            RemovePreviousClasses();
        }
    }
}
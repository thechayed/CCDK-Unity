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
        /* This pawn's name */
        public string pawnName = "NoName";

        /** Whether the Constructor method has already been called **/
        [HideInInspector]
        public bool constructed;

        /** A dictionary storing all the Class names to add to the pawn on Construction **/
        /** The Key stores the name of the original class, and the Value stores the new class **/
        public StringsDictionary classes =
            new StringsDictionary
            (
                    new List<StringsDictionaryItem>
                    {
                    new StringsDictionaryItem("PawnMovement", "CCDKGame.PawnMovement"),
                    new StringsDictionaryItem("PawnLife", "CCDKGame.PawnLife"),
                    new StringsDictionaryItem("PawnAudio", "CCDKGame.PawnAudio"),
                    new StringsDictionaryItem("PawnInventoryManager", "CCDKGame.PawnInventoryManager")
                    }
            );

        /** Componenet References **/
        PawnMovement pawnMovement;
        PawnInventoryManager pawnInventoryManager;
        PawnAudio pawnAudio;
        PawnLife pawnLife;


        /** Override this function as a replacement for Start **/
        public override void Start()
        {
            base.Start();
            /** If the Pawn hasn't be completely constructed, construct it on StartUp! **/
            if (gameObject.GetComponent<PawnMovement>() == null || gameObject.GetComponent<PawnLife>() == null || gameObject.GetComponent<PawnInventoryManager>() == null || gameObject.GetComponent<PawnAudio>() == null)
            {
                PawnConstructer();
                Debug.LogWarning("Pawn " + pawnName + ": has not be Constructed! Constructing now!");
            }
        }

        /** The Pawn constructor allows a Pawn of any type to be created anywhere **/
        public Pawn(bool construct = false, StringsDictionary newClasses = null)
        {
            if (construct)
            {
                classes = newClasses;
                PawnConstructer();
            }
        }

        /** Create children Functionality Classes that enable the Pawn's behavior in the game **/
        public void PawnConstructer(StringsDictionary newClasses = null)
        {
            /** If we recieve newClasses, set classes to it **/
            if(newClasses != null)
            {
                classes.Load(newClasses);
            }

            /** Make sure that the Classes exist **/
            ValidateClasses();
            /** Remove components that have been previously added **/
            RemovePreviousClasses();
            /** Add the Component Classes **/
            AddClassComponents();
        }

        /** Called by the Constructer to make sure that all the classes are valid **/
        public void ValidateClasses()
        {
            if (classes.length > 0)
            {
                foreach (StringsDictionaryItem item in classes.dictionary)
                {                
                    /** Check if any of the classes can't be found, alert the user and change the Class name to default **/
                    if (Type.GetType(item.value) == null)
                    {
                        Debug.LogWarning("Pawn " + pawnName + ": User defined "+item.key+" couldn't be found, rolling back to default "+PawnManager.classDefaults.Get(item.key)+"!");
                        classes.Set(item.key, PawnManager.classDefaults.Get(item.key));
                    }
                }
            }
            else
            {
                /** If there are no classes added, load Classes from Defaults **/
                Debug.LogWarning("Pawn's classes do not exist, filling with defaults!");
                classes.Load(PawnManager.classDefaults);
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
            foreach(StringsDictionaryItem item in classes.dictionary)
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
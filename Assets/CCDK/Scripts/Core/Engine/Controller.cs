/** Controllers interact with the Game Wolrd in some way, most often by possessing a Pawn. Input
 * from either an AI or Player is given to the Controller, and communicated to elements in the world
 * under it's control. **/

using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using CCDKGame;

namespace CCDKEngine
{
    public class Controller : CCDKEngine.Object
    {
        /** The Pawn that the controller has possessed **/
        public Pawn possessedPawn;
        /** Call this when the Pawn has been possessed **/
        public delegate void OnPossess();
        public OnPossess Possessed;
        /** The ID of this Controller **/
        public int IID;

        public CCDKObjects.Controller data;

        /**The Camera that this Player Controller is using*/
        public Camera possessedCamera;
        /**Class of the Camera assigned to the Player Controller*/
        public CameraClass cameraClass;

        /** A dictionary storing all the Component Classes that have been added to this object **/
        public Dictionary<Type> classes;

        [HideInInspector]
        public ControllerInput input;

        public ComponentConstructor componentConstructor;

        public void PCConstructor()
        {
            /** Call the Component Constructor for the list of Components **/
            componentConstructor = new ComponentConstructor(gameObject, data.classes, null);

            /** Loop through the components (Cast as Possessable Objects) and set their Controller values **/
            foreach (DictionaryItem<Type> item in componentConstructor.classes.dictionary)
            {
                PossessableObject component = (PossessableObject) gameObject.GetComponent(item.value);
                component.controller = this;
            }
        }

        public bool Possess(Pawn pawn)
        {
            if (possessedPawn == null)
            {
                /** If the Pawn can set this Controller as it's Controller, update this Controller's Pawn value **/
                if (pawn.SetController(this))
                {
                    possessedPawn = pawn;
                    CommandChildren();
                    Possessed();
                }
                return true;
            }
            CommandChildren();
            return false;
        }

        /** Communicate information to the Controller's children **/
        public void CommandChildren()
        {
            /** Loop through the components (Cast as Possessable Objects) and set their Controller values **/
            foreach (DictionaryItem<Type> item in componentConstructor.classes.dictionary)
            {
                ControllerClass component = (ControllerClass) gameObject.GetComponent(item.value);
                component.pawn = possessedPawn;
                component.possessingPawn = possessedPawn != null;
            }
        }
    }
}

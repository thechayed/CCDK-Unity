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
        /** The ID of this Controller **/
        public int IID;

        public CCDKObjects.Controller data;

        /**The Camera that this Player Controller is using*/
        public Camera possessedCamera;
        /**Class of the Camera assigned to the Player Controller*/
        public CameraClass cameraClass;


        [HideInInspector]
        public ControllerInput input;

        /** Controller Called Delegates :: The Controller calls the events, and the unique Pawn decides what to do with it. **/
        public delegate void Move();
        public event Move MovePawn;


        public void PCConstructor()
        {
            if (Type.GetType(data.classes.Get("inputClass")) == null)
            {
                Debug.LogWarning("Player Controller " + IID + ": User defined Input class couldn't be found, rolling back to default PlayerInventory!");
                data.classes.Set("inputClass", "PlayerInput");
            }

            /** Add the Pawn Movement script to the Pawn and set it's Player Controller and Pawn class respectively**/
            gameObject.AddComponent(Type.GetType(data.classes.Get("inputClass")));
            input = (ControllerInput)gameObject.GetComponent(Type.GetType(data.classes.Get("inputClass")));
            input.controller = this;
        }

        public bool Possess(Pawn pawn)
        {
            if (possessedPawn == null)
            {
                /** Possess the Pawn **/
                possessedPawn = pawn;
                pawn.controller = this;

                /** Add Pawn's methods to Controller events **/
                MovePawn += pawn.Move;

                /** Return true when Pawn is possessed **/
                return true;
            }
            return false;
        }

        public void MoveInput()
        {
            MovePawn();
        }
    }
}

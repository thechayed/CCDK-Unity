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
        /** The ID of this Controller **/
        public int IID;

        public CCDKObjects.Controller data;

        /**The Camera that this Player Controller is using*/
        public Camera possessedCamera;
        /**Class of the Camera assigned to the Player Controller*/
        public CameraClass cameraClass;


        [HideInInspector]
        public ControllerInput input;

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
                possessedPawn = pawn;
                pawn.controller = this;
                return true;
            }
            return false;
        }
    }
}

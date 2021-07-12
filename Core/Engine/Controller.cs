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

        /**The Camera that this Player Controller is using*/
        public Camera possessedCamera;
        /**Class of the Camera assigned to the Player Controller*/
        public CameraClass cameraClass;

        /** A dictionary storing all the Class names to add to the pawn on Construction **/
        public StringsDictionary classes =
            new StringsDictionary
            (
                    new List<StringsDictionaryItem>
                    {
                    new StringsDictionaryItem("cameraClass", "Camera"),
                    new StringsDictionaryItem("inputClass", "PlayerInput"),
                    }
            );

        [HideInInspector]
        public ControllerInput input;

        public void PCConstructor()
        {
            if (Type.GetType(classes.Get("inputClass")) == null)
            {
                Debug.LogWarning("Player Controller " + IID + ": User defined Input class couldn't be found, rolling back to default PlayerInventory!");
                classes.Set("inputClass", "PlayerInput");
            }

            /** Add the Pawn Movement script to the Pawn and set it's Player Controller and Pawn class respectively**/
            gameObject.AddComponent(Type.GetType(classes.Get("inputClass")));
            input = (ControllerInput)gameObject.GetComponent(Type.GetType(classes.Get("inputClass")));
            input.controller = this;
        }
    }
}

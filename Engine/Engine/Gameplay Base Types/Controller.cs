/** Controllers interact with the Game Wolrd in some way, most often by possessing a Pawn. Input
 * from either an AI or Player is given to the Controller, and communicated to elements in the world
 * under it's control. **/

using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using CCDKGame;
using UnityEngine.SceneManagement;

namespace CCDKEngine
{
    public class Controller : Object
    {
        /** The Pawn that the controller has possessed **/
        public Pawn possessedPawn;
        /** Call this when the Pawn has been possessed **/
        public delegate void OnPossess();
        public OnPossess Possessed;
        /** The ID of this Controller **/
        public int IID;

        /**The Camera that this Player Controller is using*/
        public Camera possessedCamera;
        /**Class of the Camera assigned to the Player Controller*/
        public CameraClass cameraClass;

        /** A dictionary storing all the Component Classes that have been added to this object **/
        public Dictionary<Type> classes;

        [HideInInspector]
        public ControllerInput input = new ControllerInput();

        public ComponentConstructor<Script.ControllerClass> componentConstructor;



        public void PCConstructor()
        {
            input.controller = this;
        }

        public bool Possess(Pawn pawn)
        {

            if (possessedPawn == null)
            {
                /** If the Pawn can set this Controller as it's Controller, update this Controller's Pawn value **/
                if (pawn.SetController(this))
                {
                    possessedPawn = pawn;
                }
                return true;
            }

            SceneManager.MoveGameObjectToScene(gameObject, LevelManager.engineScene);
            return false;
        }

    }
}

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
        public CCDKObjects.Controller controllerData;
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

        /**Tells the Pawn where to go if it is using the Nav Mesh Agent.**/
        public Vector3 navMeshAgentDestination;
        public Transform navMeshAgentDestionationTransform;

        public override void Start()
        {
            base.Start();
            controllerData = (CCDKObjects.Controller)data;
        }

        public void PCConstructor()
        {
            input.controller = this;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (possessedPawn.pawnCamera != null)
                possessedCamera = possessedPawn.pawnCamera;
        }

        public bool Possess(Pawn pawn)
        {

            if (possessedPawn == null)
            {
                /** If the Pawn can set this Controller as it's Controller, update this Controller's Pawn value **/
                if (pawn.SetController(this))
                {
                    possessedPawn = pawn;
                    possessedCamera = pawn.pawnCamera;
                }
                return true;
            }

            SceneManager.MoveGameObjectToScene(gameObject, LevelManager.engineScene);
            return false;
        }

    }
}

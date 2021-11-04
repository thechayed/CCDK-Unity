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
        [Header(" - Controller Properties - ")]
        /** The ID of this Controller **/
        public int IID;
        public CCDKObjects.Controller controllerData;

        [Header(" - Possession Properties - ")]
        /** The Pawn that the controller has possessed **/
        public Pawn possessedPawn;
        /**The Camera that this Player Controller is using*/
        public Camera possessedCamera;
        /** Call this when the Pawn has been possessed **/
        public delegate void OnPossess();
        public OnPossess Possessed;

        [Header(" - Pawn Navmesh Agent Properties - ")]
        /**Tells the Pawn where to go if it is using the Nav Mesh Agent.**/
        public Vector3 navMeshAgentDestination;
        public Transform navMeshAgentDestionationTransform;

        public override void Start()
        {
            base.Start();
            controllerData = (CCDKObjects.Controller)data;
            replicate = true;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (possessedPawn != null)
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

        /**<summary>Send Commands to the possessed Pawn.</summary>**/
        public void Command(string commandName, object[] parameters = null)
        {
            if (possessedPawn != null)
                possessedPawn.BroadcastMessage(commandName, parameters);
        }

        /**<summary>Send Commands to the possessed Pawn.</summary>**/
        public void Command(string commandName, object parameter = null)
        {
            if (possessedPawn != null)
                possessedPawn.BroadcastMessage(commandName, parameter);
        }

        public void Destroy()
        {
            if (possessedPawn != null)
                GameObject.DestroyImmediate(possessedPawn.gameObject);

            GameObject.DestroyImmediate(gameObject);
        }
    }
}

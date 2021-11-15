/** Controllers interact with the Game Wolrd in some way, most often by possessing a Pawn. Input
 * from either an AI or Player is given to the Controller, and communicated to elements in the world
 * under it's control. **/

using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using CCDKGame;
using UnityEngine.SceneManagement;
using ToolBox.Serialization;
#if USING_NETCODE
using Unity.Netcode;
#endif

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

        [Tooltip("The Camera that this Player Controller is using. Unity Cameras are not CCDKEngine Possessable objects, so they must be treated differently.")]
        public Camera possessedCamera;
        [Tooltip("List of the Possessable Objects the Controller has possessed and can communicate with.")]
        public List<PossessableObject> possessedObjects = new List<PossessableObject>();
        /** Call this when the Pawn has been possessed **/
        public delegate void OnPossess();
        public OnPossess Possessed;

        [Header(" - Pawn Navmesh Agent Properties - ")]
        /**Tells the Pawn where to go if it is using the Nav Mesh Agent.**/
        public Vector3 navMeshAgentDestination;
        public Transform navMeshAgentDestionationTransform;

        public override void Awake()
        {
            base.Awake();
            controllerData = (CCDKObjects.Controller)data;
        }

        public override void Start()
        {
            base.Start();
            controllerData = (CCDKObjects.Controller)data;
            replicate = true;
            engineObject = true;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            foreach(PossessableObject possessableObject in possessedObjects)
            {
                Pawn pawn = (Pawn)possessableObject;

                /**Get the Possessed Pawn's Camera.**/
                if (pawn!=null)
                    if (pawn.pawnCamera != null && pawn.ccdkEnabled)
                        possessedCamera = pawn.pawnCamera;
            }
        }

        public bool Possess(PossessableObject possessableObject, bool calledFromRPC = false)
        {
            ///**If we're forcing one Pawn for the Controller, Remove the previously possessed Pawn.**/
            //if(controllerData.forceOnePawn)
            //    foreach(PossessableObject possessable in possessedObjects.ToArray())
            //    {
            //        if(possessable.gameTypeState == possessableObject.gameTypeState)
            //        {
            //            possessedObjects.Remove(possessable);
            //        }
            //    }


            /** If the Pawn can set this Controller as it's Controller, update this Controller's Pawn value **/
            if (possessableObject.SetController(this))
            {
#if USING_NETCODE
                /**If net is enabled**/
                if (Engine.enableNetworking)
                    if (NetworkManager.Singleton.IsHost&&!calledFromRPC)
                        PossessClientRPC(possessableObject.GetComponent<NetworkObject>().NetworkObjectId);
#endif
                if(NetworkManager.Singleton.IsHost&&!calledFromRPC)
                    possessedObjects.Add(possessableObject);
                if(!NetworkManager.Singleton.IsHost)
                    possessedObjects.Add(possessableObject);
            }
            return true;
        }

        [ClientRpc]
        /**<summary>Possess the Pawn on the client side by it's Network Object ID.</summary>**/
        public void PossessClientRPC(ulong pawnNetworkObjectID)
        {
            Possess(NetworkManager.SpawnManager.SpawnedObjects[pawnNetworkObjectID].GetComponent<Pawn>(), true);
        }

        /**<summary>Send Commands to the possessed Pawn.</summary>**/
        public void Command(string commandName, object[] parameters)
        {
            foreach(PossessableObject possessable in possessedObjects)
            {
                if(possessable.ccdkEnabled)
                    if (possessable.GetType().GetMethod(commandName) != null)
                        possessable.GetType().GetMethod(commandName).Invoke(possessable, parameters);
            }

#if USING_NETCODE
            if (Engine.enableNetworking)
                if(!NetworkManager.IsHost&&net.IsOwner)
                    SendCommandServerRPC(commandName, DataSerializer.Serialize<object>(parameters));
#endif
        }

        /**<summary>Send Commands to the possessed Pawn.</summary>**/
        public void Command(string commandName, object parameter = default)
        {
            foreach (PossessableObject possessable in possessedObjects)
            {
                
                if (possessable.ccdkEnabled)
                    if(possessable.GetType().GetMethod(commandName)!=null)
                        possessable.GetType().GetMethod(commandName).Invoke(possessable, new object[] { parameter });
            }

#if USING_NETCODE
            if (Engine.enableNetworking)
                if (!NetworkManager.IsHost&&net.IsOwner)
                    SendCommandServerRPC(commandName, DataSerializer.Serialize<object>(parameter));
#endif
        }

#if USING_NETCODE
        [ServerRpc]
        /**<summary>Sends the command to the Host when the instance is a Network Connected Client.</summary>**/
        public void SendCommandServerRPC(string commandName, byte[] parameter = default)
        {
            var paramData = DataSerializer.Deserialize<object>(parameter);
            if(paramData==null)
                paramData = DataSerializer.Deserialize<object[]>(parameter);

            Debug.Log("message: " + commandName + " params: " + paramData);

            Command(commandName, paramData);
        }
#endif

        public void Destroy()
        {
            foreach(PossessableObject possessable in possessedObjects)
                GameObject.DestroyImmediate(possessable.gameObject);


            GameObject.DestroyImmediate(gameObject);
        }

        /**<summary>Gets the Player that this Controller belongs to.</summary>**/
        public Player GetPlayer()
        {
            return null;
        }

        public Pawn GetPawn()
        {
            Debug.Log(possessedObjects.Count);
            foreach (PossessableObject possessable in possessedObjects.ToArray())
            {
                Debug.Log(possessable.name);
                if (possessable.gameTypeState == Engine.currentGameType.state)
                {
                    Pawn getPawn = (Pawn)possessable;
                    Debug.Log(getPawn);
                    if(getPawn != null)
                        return getPawn;
                }
            }
            return null;
        }
    }
}

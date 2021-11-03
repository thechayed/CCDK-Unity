/* When a Level is set at the current Level, it is given the Game Type component to control
 Game Type information identically to Unreal's Game Info system.*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CCDKEngine;
using System;
#if USING_NETCODE
using Unity.Netcode;
#endif

namespace CCDKGame
{
    public class GameType : FSM.Component
    {
        [Header(" - GameType - ")]
        public bool init = false;
        public bool isHost = true;
        public bool allowMultiplayer = true;

        public bool gameplayStarted;
        public CCDKObjects.GameTypeInfo gameTypeData;

        /**<summary>When the Game Type is starting, initialize it.</summary>**/
        private void Update()
        {
            if (Engine.singleton.initialized&&!init)
            {
                this.Invoke("LocalInitialization", 0f);
            }

        }


        #region Game Type Overrides
        /**<summary>Override to add extra Initialization functionality.</summary>**/
        public virtual void Init()
        {
        }

        /**<summary>Call to start the game, Override to add additional Game Start functionality.</summary>**/
        public virtual void StartGame()
        {
        }

        /**<summary>Call to End the game, Override to add additional Game End functionality.</summary>**/
        public virtual void EndGame()
        {
        }

        /**<summary>A Callback for when a Player Joins the game.</summary>**/
        public virtual void PlayerJoined(ulong clientId)
        {
            Debug.Log(clientId+" joined");
            PlayerController newController = PlayerManager.CreatePC(gameTypeData.defaultPlayerController);
            SetControllerID(newController, clientId);
            SetUpPlayer(newController);
        }


        /**<summary>A Callback for when a Player Leaves the game.</summary>**/
        public virtual void PlayerLeft(ulong clientId)
        {
            Debug.Log(clientId + " left.");
        }


        /**<summary>Controls what happens to a Player when they join the Game.</summary>**/
        public virtual void SetUpPlayer(PlayerController controller)
        {

        }

        /**<summary>Undo any SetUp modifications when the Player leaves the game.</summary>**/
        public virtual void RemovePlayer(PlayerController controller)
        {

        }

        public virtual void NetworkStart()
        {
#if USING_NETCODE
                isHost = NetworkManager.Singleton.IsHost;
#endif
        }

        public virtual void NetworkEnd()
        {

        }

        /**<summary>Another class can request the spawning of a pawn in the Game using this method. Override to add additional conditions for possessing a pawn.</summary>**/
        public virtual Pawn TrySpawn(CCDKObjects.Pawn pawnToSpawn, Transform spawnTransform = default)
        {
            return PawnManager.CreatePawn(pawnToSpawn, spawnTransform);
        }
        
        /**<summary>Asks the Game Type to let the Player Controller possess a Pawn. Override to add conditions to the possession requirements.</summary>**/
        public virtual Pawn RequestPossession(CCDKObjects.Pawn pawnToPossess)
        {
            return GetPawnFromDataType(pawnToPossess);
        }

        /**<summary>Asks the Game Type of possession of the Pawn is allowed. Override to add conditions to the possession requirements.</summary>**/
        public virtual bool IsPossessionAllowed(Pawn pawn)
        {
            return true;
        }
        #endregion

        #region Absolute Methods

        /**Initialization called in this script only.**/
        private void LocalInitialization()
        {
            Engine.currentGameType = this;

#if USING_NETCODE
            Engine.PlayerJoined += PlayerJoined;
            Engine.PlayerLeft += PlayerLeft;
            Engine.NetworkConnect += NetworkStart;
            Engine.NetworkDisconnect += NetworkEnd;

            if (gameTypeData.networkManager != null)
            {
                Engine.CreateNetworkManager(gameTypeData.networkManager);
            }
            else
            {
                allowMultiplayer = false;
                Debug.Log("No Network Manager prefab has been given to the active Game Type, Multiplayer is not supported.");
            }
#endif

            /**Set the Default Player Controller for currently active Players whenever the Game Type begins**/
            if (gameTypeData.defaultPlayerController != null)
            {
                foreach (Player player in PlayerManager.singleton.players.ToArray())
                {
                    PlayerManager.SetPlayerController(player.playerID.ID, gameTypeData.defaultPlayerController);
                    SetUpPlayer((PlayerController)GetControllerWithoutPawn());
                    Debug.Log("New Player Controller for Host, and spawned a pawn for it.");
                }
            }
            if(isHost)
                this.Invoke("Init", 0f);

            if (gameTypeData.startInMultiplayer)
                MultiplayerStart();

            init = true;
        }

        /**<summary>Return an unpossessed Pawn in the game that is paired with the passed data type.</summary>**/
        public Pawn GetPawnFromDataType(CCDKObjects.Pawn pawnToPossess)
        {
            foreach (Pawn pawn in PawnManager.PawnsInGame)
            {
                if (pawn.data == pawnToPossess&&pawn.controller==null)
                {
                    return pawn;
                }
            }
            return null;
        }
        #endregion

        /**<summary>Spawns the default pawn into the game and returns it's Game Object</summary>**/
        //Add For Multiplayer: GameObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        public Pawn Spawn(Transform spawnTransform = null)
        {
            if (!isHost)
                return null;

                if (spawnTransform == null)
            {
                Transform spawnerTransform = LevelManager.FindSpawn();
                if(spawnerTransform == null)
                {
                    Debug.LogError("No spawn point or Spawn Position was given in GameType.Spawn(), please make a Spawn Point!");
                    return PawnManager.CreatePawn(gameTypeData.defaultPawn, default);
                }

                return PawnManager.CreatePawn(gameTypeData.defaultPawn, spawnerTransform);
            }
            return PawnManager.CreatePawn(gameTypeData.defaultPawn, spawnTransform);
        }

        public void SpawnForController(Controller controller, Pawn pawn = null, Transform spawnTransform = null)
        {
            if (!isHost)
                return;

            if (controller == null)
                return;

            if (pawn == null)
            {
                controller.Possess(Spawn(spawnTransform));
            }
        }

        public Controller GetControllerWithoutPawn()
        {
            Controller controller = null;

            foreach(Controller item in PlayerManager.controllers)
            {
                if(item.possessedPawn == null)
                {
                    controller = item;
                    break;
                }
            }

            return controller;
        }

        public void MultiplayerStart()
        {
#if USING_NETCODE

            if (allowMultiplayer)
            {
                /**Delete local Player Controllers at the beginning of the game.**/
                /**Replace this with total Replicated Object Deletion instead.**/
                foreach (Controller controller in PlayerManager.controllers.ToArray())
                {
                    PlayerManager.RemovePC(controller);
                }
                ///**Check if we can Start as a Host given our current values, otherwise Start the client.**/
                //bool success = NetworkManager.Singleton.StartHost();
                //if (!success)
                //{
                //    NetworkManager.Singleton.StartClient();
                //}

                /**If we became the host, Create a new Player Controller and assign it to ourself.**/
                if (NetworkManager.Singleton.IsHost)
                {
                    SetControllerID(PlayerManager.CreatePC(gameTypeData.defaultPlayerController), NetworkManager.Singleton.LocalClientId);
                }
            }
#endif
        }

#if USING_NETCODE
        /**<summary>Sets the Controller as owned by the ClientID.</summary>**/
        public void SetControllerID(PlayerController controller, ulong clientID)
        {
            controller.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID);
        }
#endif

        //// A ClientRpc can be invoked by the server to be executed on a client
        //[ClientRpc]
        //private void SpawnClientRpc(ulong objectId)
        //{
        //    NetworkSpawnManager
        //    NetworkObject player = NetworkSpawnManager.SpawnedObjects[objectId];
        //}
    }
}

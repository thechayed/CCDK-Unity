/* When a Level is set at the current Level, it is given the Game Type component to control
 Game Type information identically to Unreal's Game Info system.*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CCDKEngine;
#if USING_NETCODE
using Unity.Netcode;
#endif

namespace CCDKGame
{
    public class GameType : FSM.Component
    {
        public bool init = false;
        public bool isHost = true;

        public bool gameplayStarted;
        public CCDKObjects.GameTypeInfo data;

        /**The GameType class of the Game Type that is being played.**/
        public static GameType currentGame;

        /**<summary>When the Game Type is starting, initialize it.</summary>**/
        private void Update()
        {
            if (Engine.singleton.initialized&&!init)
            {
                this.Invoke("LocalInitialization", 0f);
            }
            /** If this game uses Netcode and we're not the host, disable the GameType behaviour **/
#if USING_NETCODE
            if (NetworkManager.Singleton.IsServer)
                isHost = NetworkManager.Singleton.IsHost;
#endif
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
        public virtual void PlayerJoined()
        {
        }

        /**<summary>A Callback for when a Player Leaves the game.</summary>**/
        public virtual void PlayerLeft()
        {
        }

        /**<summary>Another class can request the spawning of a pawn in the Game using this method. Override to add additional conditions for possessing a pawn.</summary>**/
        public virtual Pawn TrySpawn(CCDKObjects.Pawn pawnToSpawn, Transform spawnTransform = default(Transform))
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



            /**Set the Default Player Controller for currently active Players whenever the Game Type begins**/
            if (data.defaultPlayerController != null)
            {
                foreach (Player player in PlayerManager.singleton.players)
                {
                    PlayerManager.SetPlayerController(player.ID, data.defaultPlayerController);
                }
            }
            if(isHost)
                this.Invoke("Init", 0f);

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
                    return PawnManager.CreatePawn(data.defaultPawn, default(Transform));
                }

                return PawnManager.CreatePawn(data.defaultPawn, spawnerTransform);
            }
            return PawnManager.CreatePawn(data.defaultPawn, spawnTransform);
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
            bool success = NetworkManager.Singleton.StartHost();
            if(!success)
            {
                NetworkManager.Singleton.StartClient();
            }
        }


    }
}

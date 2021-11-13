/* When a Level is set at the current Level, it is given the Game Type component to control
 Game Type information identically to Unreal's Game Info system.*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CCDKEngine;
using System;
using System.Collections.Generic;
#if USING_NETCODE
using Unity.Netcode;
#endif

namespace CCDKGame
{
    public class GameType : FSM.Component
    { //<Class>
        [Header(" - GameType - ")]
        public bool init = false;
        public bool isHost = true;
        public bool allowMultiplayer = true;

        public bool gameplayStarted;
        public CCDKObjects.GameTypeInfo gameTypeData;

        public Queue<Controller> controllerInitQueue = new Queue<Controller>();
        public Queue<Controller> controllerPossessionQueue = new Queue<Controller>();
        public Queue<Pawn> pawnInitQueue = new Queue<Pawn>();
        public Queue<Pawn> pawnFreeQueue = new Queue<Pawn>();

        /**The amount of Teams in the game.**/
        public int teamCount;
        public List<Team> teams = new List<Team>();

        /**<summary>When Objects are added to the game, they are referenced in this list to be enabled/disabled depending on the State the Game Type was in at the time of their creation.</summary>**/
        public Dictionary<List<CCDKEngine.Object>> stateObjectPairs = new Dictionary<List<CCDKEngine.Object>>();
        public bool stateObjectPairingEnabled = false;

        /**The size of an individual team.**/
        public int maxTeamSize = 1;


        /**<summary>When the Game Type is starting, initialize it.</summary>**/
        private void Update()
        {
            /**Initialize the Game Type**/
            if (Engine.singleton.initialized&&!init)
            {
                this.Invoke("LocalInitialization", 0f);
                init = true;
            }

            #region scoring
            /**If this is a Game Type that uses the Default scoring system, declare a win when a Team reaches the Goal Score**/
            if (gameTypeData.teamGame)
              foreach(Team team in teams)
              {
                if(team.score >= gameTypeData.goalScore&&gameTypeData.goalScore!=-1)
                {
                  DeclareWin(team);
                }
              }

           /**Always check if Win conditions have been met.**/
           /**Check if a team won, yo**/
            CheckWin();
            #endregion

            #region Possession
            /**If using Netcode, and it's enabled, wait until Pawns and Controllers are Spawned before using them.**/
#if USING_NETCODE
            if (Engine.enableNetworking)
            {
                if (controllerInitQueue.Count > 0)
                    /**Once the Player Controller is ready, call the SetUpPlayer method for extending a Player's Start functionality**/
                    if (controllerInitQueue.Peek().GetComponent<NetworkObject>().IsSpawned)
                        SetUpPlayer(controllerInitQueue.Dequeue());


                if (pawnInitQueue.Count > 0)
                    /**Once the Pawn is ready, pass it to the Free Queue to be possessed later.**/
                    if (pawnInitQueue.Peek().GetComponent<NetworkObject>().IsSpawned)
                        pawnFreeQueue.Enqueue(pawnInitQueue.Dequeue());
            }
            else
            {
                if (controllerInitQueue.Count > 0)
                    SetUpPlayer(controllerInitQueue.Dequeue());
                if(pawnInitQueue.Count>0)
                    pawnFreeQueue.Enqueue(pawnInitQueue.Dequeue());
            }
#else
                if (controllerInitQueue.Count > 0)
                    SetUpPlayer(controllerInitQueue.Dequeue());
                if(pawnInitQueue.Count>0)
                    pawnFreeQueue.Enqueue(pawnInitQueue.Dequeue());
#endif

            /**If there is a Controller wait for a Pawn to Possess and there is a Pawn Free to Possess.**/
            if (controllerPossessionQueue.Count > 0 && pawnFreeQueue.Count > 0)
                controllerPossessionQueue.Dequeue().Possess(pawnFreeQueue.Dequeue());
            #endregion
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
        public virtual void NetworkClientJoined(ulong clientId)
        {
            Debug.Log(clientId+" joined");


            //PlayerController newController = PlayerManager.CreatePC(gameTypeData.defaultPlayerController);
            //newController.SetOrigin();
            //SetControllerID(newController, clientId);
        }


        /**<summary>A Callback for when a Player Leaves the game.</summary>**/
        public virtual void NetworkClientLeft(ulong clientId)
        {
            Debug.Log(clientId + " left.");
        }

        public void PrepController(Controller controller)
        {
            controllerInitQueue.Enqueue(controller);
        }

        /**<summary>Controls what happens to a Player when they join the Game.</summary>**/
        public virtual void SetUpPlayer(Controller controller)
        {

        }

        /**<summary>Enqueue Controller for Posession and Spawn a Pawn to be Possessed. 
         * The Controller will automatically possess the Pawn once both their Network Objects are Spawned.</summary>**/
        public void QueueControllerAndSpawn(Controller controller)
        {
            controllerPossessionQueue.Enqueue(controller);
            Spawn();
        }

        /**<summary>Undo any SetUp modifications when the Player leaves the game.</summary>**/
        public virtual void RemovePlayer(PlayerController controller)
        {

        }

        /**<summary>Declare that the selected Team has won the match!</summary>**/
        public virtual void DeclareWin(Team team)
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
        {//<LocalInitialization>
            Engine.currentGameType = this;

#if USING_NETCODE
            Engine.NetworkClientJoined += NetworkClientJoined;
            Engine.NetworkClientLeft += NetworkClientLeft;
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
                foreach (Player player in PlayerManager.managers[0].pool.players.ToArray())
                {
                    PlayerController playerController = PlayerManager.managers[0].SetPlayerController(0, gameTypeData.defaultPlayerController);
                    SetUpPlayer(player.assignedController);
                }
            }
            if(isHost)
                this.Invoke("Init", 0f);

            if (gameTypeData.startInMultiplayer)
                MultiplayerStart();

            init = true;

            /**Initialize States for Object Pairing.**/
            foreach(string stateName in stateList)
            {
                stateObjectPairs.Set(stateName, new List<CCDKEngine.Object>());
            }
            stateObjectPairingEnabled = true;

        }//</LocalInitialization>

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
                    pawnInitQueue.Enqueue(PawnManager.CreatePawn(gameTypeData.defaultPawn, default));
                    return pawnInitQueue.Peek();
                }

                pawnInitQueue.Enqueue(PawnManager.CreatePawn(gameTypeData.defaultPawn, spawnerTransform));
                return pawnInitQueue.Peek();
            }
            pawnInitQueue.Enqueue(PawnManager.CreatePawn(gameTypeData.defaultPawn, spawnTransform));
            return pawnInitQueue.Peek();
        }

        public void SpawnForController(Controller controller, Pawn pawn = null, Transform spawnTransform = null)
        {
            if (!isHost)
                return;

            if (controller == null)
                return;

            if (pawn == null)
            {
                Pawn newPawn = Spawn(spawnTransform);
                newPawn.spawnAsClientObject = true;
                newPawn.clientID = controller.clientID;

                controller.Possess(newPawn);
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
                    PlayerController newController = PlayerManager.CreatePC(gameTypeData.defaultPlayerController);
                    newController.SetOrigin();
                    SetControllerID(newController, NetworkManager.Singleton.LocalClientId);
                    newController.SetOrigin();
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

        /**<summary>Add an Object to the State-Object pairing Dictionary.</summary>**/
        public void AddObjectToState(CCDKEngine.Object objectToAdd, string stateName)
        {
            stateObjectPairs.Get(stateName).Add(objectToAdd);
        }

        /**<summary>Check for Custom win condition</summary>**/
        public virtual Team CheckWin()
        {
            return null;
        }

        /**When the State has changed, **/
        public override void StateChanged(string prevState)
        {
            if (stateObjectPairingEnabled)
            {
                /**Destroy/Deactivate Objects used for the previous state.**/
                foreach(CCDKEngine.Object stateObject in stateObjectPairs.Get(prevState))
                {
                    Pawn asPawn = stateObject as Pawn;
                    if (asPawn != null)
                    {
                        if (asPawn.controller != null)
                        {
                            controllerPossessionQueue.Enqueue(asPawn.controller);
                            GameObject.Destroy(asPawn.gameObject);
                        }
                        else
                            GameObject.Destroy(asPawn.gameObject);
                    }
                    else
                        stateObject.gameObject.SetActive(false);
                }

                /**Destroy/Deactivate Objects used for the previous state.**/
                foreach (CCDKEngine.Object stateObject in stateObjectPairs.Get(prevState))
                {
                    Pawn asPawn = stateObject as Pawn;
                    if (asPawn != null)
                    {
                        if (asPawn.controller != null)
                        {
                            controllerPossessionQueue.Enqueue(asPawn.controller);
                            GameObject.Destroy(asPawn.gameObject);
                        }
                        else
                            GameObject.Destroy(asPawn.gameObject);
                    }
                    else
                        stateObject.gameObject.SetActive(false);
                }
            }
        }

    } //</Class>
}

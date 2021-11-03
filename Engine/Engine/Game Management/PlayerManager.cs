using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKGame;
using UnityEngine.SceneManagement;
using ToolBox.Serialization;

#if USING_NETCODE
using Unity.Netcode;
#endif


namespace CCDKEngine
{
    public class PlayerManager : Object
    {
        public static PlayerManager singleton;

        /**<summary>List of all the Players in the game and their information.</summary>**/
        public List<Player> players = new List<Player>();
        public PlayerPool pool = new PlayerPool();

        /**<summary>The list of Player Controllers in the game</summary>**/
        public static List<Controller> controllers = new List<Controller>();

        public Dictionary<Pawn> controllerPossessions = new Dictionary<Pawn>();

        /**The count of Player Controllers in the game**/
        public static int PCCount;

        public override void Awake()
        {
            PlayerManager.singleton = this;
            Engine.NetworkConnect += NetworkStart;
            Engine.NetworkDisconnect += NetworkEnd;
            Engine.PlayerLeft += PlayerLeft;

            /**Tell the engine not to handle this as a Level Object**/
            Independent = true;
        }

        /** Create a record of a created Player Controller in the Manager **/
        public static int AddPC(Controller controller)
        {
            controllers.Add(controller);
            PCCount += 1;
            return PCCount += 1;
        }

        public static void RemovePC(Controller controller)
        {
            controllers.Remove(controller);
        }

        /**<summary>Create an instance of a Player Controller from it's Scriptable Object.</summary>**/
        public static PlayerController CreatePC(CCDKObjects.Controller PCObject)
        {
            GameObject playerController = GameObject.Instantiate(PCObject.prefab);
            playerController.name = PCObject.prefab.name;
            SceneManager.MoveGameObjectToScene(playerController, LevelManager.engineScene);
            PlayerController playerControllerComponent = playerController.GetComponent<PlayerController>();
            playerControllerComponent.Independent = true;
            AddPC(playerControllerComponent);
            return playerControllerComponent;
        }

        /**<summary>Creates a New Player in the game (This is the Player's actual existence in the game, and can be assigned any Controller to interact with the game.</summary>**/
        public static Player NewPlayer(PlayerController controller, ulong clientID = 0)
        {
            Player newPlayer = new Player(controller, new PlayerID(), clientID);
            PlayerManager.singleton.pool.playerIDs.Add(newPlayer.playerID.clientID);
            return newPlayer;
        }

        public static void RegisterPlayers()
        {

        }

        /**<summary>Replaces a Player's Controller with a new one.</summary>**/
        public static void SetPlayerController(int ID, CCDKObjects.Controller controller)
        {
            foreach(Player player in PlayerManager.singleton.pool.players)
            {
                if(player.playerID.ID == ID)
                {
                    if (player.assignedController != null)
                        GameObject.Destroy(player.assignedController.gameObject);
                    player.assignedController = CreatePC(controller);
                }
            }
        }

        /**<summary>Try to make a Player Controller possess a pawn.</summary>**/
        public static void Possess(Controller controller, Pawn pawn)
        {
            if (controller.possessedPawn == null)
            {
                /** If the Pawn can set this Controller as it's Controller, update this Controller's Pawn value **/
                if (pawn.SetController(controller))
                {
                    controller.possessedPawn = pawn;
                }

                singleton.controllerPossessions.Set(controller.name, pawn);
            }
        }



#if USING_NETCODE
        public override void NetworkStart()
        {
            base.NetworkStart();

            foreach(Controller controller in PlayerManager.controllers.ToList())
            {
                if(controller.GetComponent<NetworkObject>()!=null)
                    if (controller.GetComponent<NetworkObject>().IsOwner)
                        RemovePC(controller);
            }


            if (NetworkManager.Singleton.IsHost)
                LogLocalPlayersAsClient();
            else
            {
                LogClientServerRPC(pool.players.Count, NetworkManager.Singleton.LocalClientId);
            }

        }

        public override void NetworkEnd()
        {
            base.NetworkStart();

        }

        /**<summary>When the Player joins a game as a client, it is commanded to log it's Local Players as the client.</summary>**/
        public void LogLocalPlayersAsClient()
        {
            foreach(ulong cliendID in pool.playerIDs.ToArray())
            {
                NewPlayer(CreatePC(Engine.currentGameType.gameTypeData.defaultPlayerController), NetworkManager.Singleton.LocalClientId);
            }
        }

        /**When a client connects, add the Client's Players as Server-Side Players, with their Player Controllers, and assign those Controllers to that client. **/
        [ServerRpc]
        public void LogClientServerRPC(int playerCount, ulong clientID)
        {
            for(var i = 0; i<playerCount; i++)
            {
                NewPlayer(CreatePC(Engine.currentGameType.gameTypeData.defaultPlayerController), clientID);
            }

            
            LogClientsClientRPC(DataSerializer.Serialize<List<ulong>>(pool.playerIDs));
        }

        /**When a player leaves, remove their ID from the Pool list and Destroy their Controller.**/
        public void PlayerLeft(ulong clientID)
        {
            foreach (Player player in pool.players.ToArray())
            {
                pool.playerIDs.Remove(player.playerID.clientID);
                GameObject.Destroy(player.assignedController);
            }

            LogClientsClientRPC(DataSerializer.Serialize<List<ulong>>(pool.playerIDs));
        }

        /**When a Client Connects, tell it and all clients to change their Player List to that of the Server's **/
        [ClientRpc]
        public void LogClientsClientRPC(byte[] bytes)
        {
            this.pool.playerIDs = DataSerializer.Deserialize<List<ulong>>(bytes); 
            //this.pool.LoadPlayers(Engine.currentGameType.gameTypeData.defaultPlayerController, null);
        }

#endif

        //#if USING_NETCODE
        //        /**<summary>Prep the Player Controllers for Networking</summary>**/
        //        public static void PrepForNetworking()
        //        {
        //            foreach(Controller controller in playerControllers)
        //            {
        //                NetworkPrefab thisNetworkObject = new NetworkPrefab() { Prefab = controller.gameObject };
        //                NetworkManager.Singleton.NetworkConfig.NetworkPrefabs.Add(thisNetworkObject);
        //            }
        //        }
        //#endif

    }
}
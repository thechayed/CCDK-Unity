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
    /**<summary>A Player Manager is the Manager of all Local Players (Players from this instance of the game). And is the Player Prefab in Netcode, allowing split screen players to play online.</summary>**/
    public class PlayerManager : CCDKEngine.Object
    {
        /**<summary>All the Player Managers that exist in the game.</summary>**/
        public static List<PlayerManager> managers = new List<PlayerManager>();

        /**<summary>Pool of all the Players in the game and their information.</summary>**/
        public PlayerPool pool = new PlayerPool();

        /**<summary>The list of Player Controllers in the game</summary>**/
        public static List<Controller> controllers = new List<Controller>();

        public override void Awake()
        {
            Independent = true;
            engineObject = true;
            managers.Add(this);
            if (managers[0] != this)
                replicate = true;
        }

        public override void Update()
        {
            if (net != null)
            {
                if (net.IsLocalPlayer)
                {
                    pool = managers[0].pool;
                }
            }
        }

        /** Create a record of a created Player Controller in the Manager **/
        public static void AddPC(Controller controller)
        {
            controllers.Add(controller);
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
        public static Player NewPlayer(PlayerController controller, int managerIndex = 0)
        {
            Player newPlayer = new Player(controller);
            managers[0].pool.players.Add(newPlayer);
            return newPlayer;
        }

        /**Used in Networking to Remove an Player by it's Client ID**/
        public static void RemovePlayerByClientID(ulong clientID = 0)
        {
            foreach(PlayerManager manager in managers)
            {
                if(manager.clientID == clientID)
                {
                    foreach (Player player in manager.pool.players.ToArray())
                    {
                        GameObject.Destroy(player.assignedController);
                        manager.pool.players.Remove(player);
                    }
                }
            }
        }
        /**Removes a Player Manager from the game. We must first destroy all Player Controller objects associated with the Manager.**/
        public static void RemovePlayerManager(ulong clientID = 0)
        {
            foreach(PlayerManager playerManager in managers)
            {
                if(playerManager.clientID == clientID)
                {
                    foreach (Player player in playerManager.pool.players.ToArray())
                    {
                        GameObject.Destroy(player.assignedController);
                    }

                    GameObject.Destroy(playerManager.gameObject);
                }
            }
        }

        public static void RegisterPlayers()
        {

        }

        /**<summary>Replaces a Player's Controller with a new one.</summary>**/
        public PlayerController SetPlayerController(int ID, CCDKObjects.Controller controller)
        {
            PlayerController newController = CreatePC(controller);
            newController.player = pool.players[ID];
            pool.players[ID].assignedController.Destroy();
            GameObject.Destroy(pool.players[ID].assignedController);
            pool.players[ID].assignedController = newController;
            pool.players[ID].assignedController.player = pool.players[ID];

            return newController;
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
            }
        }



#if USING_NETCODE
        public override void NetworkStart()
        {
            base.NetworkStart();

            /**If this is the PlayerManager assigned to the Client by the Network Manager, backup the Player pool from the original PlayerManager and Destroy it.**/
            if (net.IsLocalPlayer&&managers[0] != null)
            {
                pool = managers[0].pool;
                foreach (Player player in managers[0].pool.players)
                {
                    RemovePC(player.assignedController);
                }
                GameObject.DestroyImmediate(managers[0].gameObject);
            }

            if (NetworkManager.Singleton.IsHost&net.IsLocalPlayer)
            {
                LogLocalPlayersAsClient();
            }
            if(!NetworkManager.Singleton.IsHost&net.IsLocalPlayer)
            {
                LogClientPlayersServerRPC(pool.players.Count);
            }


            //foreach(Controller controller in PlayerManager.controllers.ToList())
            //{
            //    if(controller.GetComponent<NetworkObject>()!=null)
            //        if (controller.GetComponent<NetworkObject>().IsOwner)
            //            RemovePC(controller);
            //}

            //if (!net.IsSpawned)
            //{
            //    GameObject.Destroy(gameObject);
            //}
        }

        public override void NetworkEnd()
        {
            base.NetworkEnd();

        }

        /**<summary>When the Player joins a game as a client, it is commanded to log it's Local Players as the client.</summary>**/
        public void LogLocalPlayersAsClient()
        {

            PlayerManager manager = GetClientManager(NetworkManager.Singleton.LocalClientId);

            foreach (Player player in pool.players.ToArray())
            {
                PlayerController newController = manager.SetPlayerController(0, Engine.currentGameType.gameTypeData.defaultPlayerController);
                newController.SetOrigin();
                newController.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
                Engine.currentGameType.PrepController(newController);
            }
        }

        /**When a client connects, add the Client's Players as Server-Side Players, with their Player Controllers, and assign those Controllers to that client. **/
        [ServerRpc]
        public void LogClientPlayersServerRPC(int playerCount)
        {
            Debug.Log("Recieved Client Message! Client has this players: "+playerCount);
            for(var i = 0; i<playerCount; i++)
            {
                Player newPlayer = new Player(CreatePC(Engine.currentGameType.gameTypeData.defaultPlayerController));
                newPlayer.assignedController.clientID = net.OwnerClientId;
                newPlayer.assignedController.spawnAsClientObject = true;
                newPlayer.assignedController.player = newPlayer;
                pool.players.Add(newPlayer);
                Engine.currentGameType.SetUpPlayer(newPlayer.assignedController);
            }
        }

        /**When a Client Connects, tell it and all clients to change their Player List to that of the Server's **/
        [ClientRpc]
        public void LogLocalPlayersClientRPC(byte[] bytes)
        {
            
        }


        /**When a player leaves, remove their ID from the Pool list and Destroy their Controller.**/
        public override void NetworkClientLeft(ulong clientID)
        {
            foreach (Player player in pool.players.ToArray())
            {
                pool.players.Remove(player);
                GameObject.Destroy(player.assignedController);
            }

            //LogClientsClientRPC(DataSerializer.Serialize<List<ulong>>(pool.playerIDs));
        }



#endif

        public PlayerManager GetClientManager(ulong clientID)
        {
            foreach(PlayerManager manager in managers)
            {
                if (manager.clientID == clientID)
                    return manager;
            }
            return null;
        }

        public void SetManagerControllers(ulong clientID, CCDKObjects.Controller controller)
        {
            PlayerManager manager = GetClientManager(clientID);
            
            foreach(Player player in manager.pool.players)
            {
                player.assignedController = CreatePC(controller);
            }
        }

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
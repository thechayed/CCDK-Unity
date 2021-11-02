using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKGame;
using UnityEngine.SceneManagement;

#if USING_NETCODE
using Unity.Netcode;
#endif


namespace CCDKEngine
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager singleton;

        /**<summary>List of all the Players in the game and their information.</summary>**/
        public List<Player> players = new List<Player>();
        /**<summary>The list of Player Controllers in the game</summary>**/
        public static List<Controller> controllers = new List<Controller>();

        public Dictionary<Pawn> controllerPossessions = new Dictionary<Pawn>();

        /**The count of Player Controllers in the game**/
        public static int PCCount;

        private void Awake()
        {
            PlayerManager.singleton = this;
            Engine.NetworkConnect += NetworkStart;
            Engine.NetworkDisconnect += NetworkEnd;
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
        public static Player NewPlayer(PlayerController controller)
        {
            Player newPlayer = new Player(controller);
            PlayerManager.singleton.players.Add(newPlayer);
            return newPlayer;
        }

        /**<summary>Replaces a Player's Controller with a new one.</summary>**/
        public static void SetPlayerController(int ID, CCDKObjects.Controller controller)
        {
            foreach(Player player in PlayerManager.singleton.players)
            {
                if(player.ID == ID)
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


        public void NetworkStart()
        {
#if USING_NETCODE
            foreach(Controller controller in PlayerManager.controllers.ToList())
            {
                if(controller.GetComponent<NetworkObject>()!=null)
                    if (controller.GetComponent<NetworkObject>().IsOwner)
                        RemovePC(controller);
            }
            
#endif
        }

        public void NetworkEnd()
        {

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
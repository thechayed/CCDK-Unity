using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKGame;
using Unity.Netcode;

namespace CCDKEngine
{
    [System.Serializable]
    public class Player
    {
        public PlayerController assignedController;

        /**<summary>The team the player is on.</summary>**/
        public Team team;

        public Player(PlayerController assignedController = null)
        {
            this.assignedController = assignedController;
        }
    }
    
    [System.Serializable]
    public class PlayerID
    {
        public int ID;
        public ulong clientID = 0;
    }

    [System.Serializable]
    public class PlayerPool
    {
        public List<ulong> playerIDs = new List<ulong>();
        public List<Player> players = new List<Player>();

//        /**<summary>Must be given a Player Controller from the Game Type. Loads Players from a passed Serialized data set.</summary>**/
//        public void LoadPlayers(CCDKObjects.Controller controller, List<PlayerID> IDList)
//        {
//            /**Back up all the Players before acting on them**/
//            List<Player> playersBackup = players;

//            /**Loop through and destroy all Player's Controllers except for local ones**/
//            foreach(Player player in players)
//            {
//                if (player.playerID.clientID != NetworkManager.Singleton.LocalClientId)
//                    GameObject.Destroy(player.assignedController);
//            }

//            /**Reset the Player List**/
//            players = new List<Player>();
            
//            /**Loop through and create other clients and their controllers, but keep our Controller when recreating ours.**/
//            foreach(PlayerID ID in IDList)
//            {
//#if USING_NETCODE
//                bool wasLocal = false;

//                if (NetworkManager.Singleton.LocalClientId == ID.clientID)
//                {
//                    foreach(Player player in playersBackup)
//                    {
//                        if (player.playerID.clientID == NetworkManager.Singleton.LocalClientId)
//                        {
//                            wasLocal = true;
//                            PlayerManager.NewPlayer(player.assignedController, ID.clientID);
//                        }
//                    }
//                }

//                if(!wasLocal)
//                    PlayerManager.NewPlayer(PlayerManager.CreatePC(controller), ID.clientID);
//#endif
//            }
//        }

        public void GetPlayers()
        {

        }

        public void GetIDsFromPlayerPool()
        {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKGame;

namespace CCDKEngine
{
    [System.Serializable]
    public class Player
    {
        public int ID;
        public PlayerController assignedController;

        public Player(PlayerController assignedController)
        {
            ID = PlayerManager.playerControllers.Count;
            this.assignedController = assignedController;
        }
    }

}
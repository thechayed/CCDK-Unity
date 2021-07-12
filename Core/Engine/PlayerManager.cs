using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKGame;

namespace CCDKEngine
{
    public static class PlayerManager
    {
        /**The count of Player Controllers in the game**/
        public static int PCCount;
        /**The list of Player Controllers in the game**/
        public static List<Controller> playerControllers = new List<Controller>();

        public static int AddPC(Controller controller)
        {
            playerControllers.Add(controller);
            return PCCount += 1;
        }


        /** Create a PlayerController from it's Class Name **/
        public static void CreatePC(string className)
        {
            GameObject playerController =
                new GameObject()
                {
                    name = "PlayerController"
                };
            playerController.AddComponent(Type.GetType(className));
        }
    }

}
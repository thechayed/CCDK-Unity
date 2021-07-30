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
        /** The default Player Controller Scriptable object, for creating custom data sets **/
        public static CCDKObjects.Controller defaultPlayerControllerSO;

        /** Create a record of a created Player Controller in the Manager **/
        public static int AddPC(Controller controller)
        {
            playerControllers.Add(controller);
            return PCCount += 1;
        }


        /** Create a PlayerController from it's Class Name **/
        public static void CreatePC(string className, CCDKObjects.Controller PCObject = null)
        {
            
            GameObject playerController =
                new GameObject()
                {
                    name = "PlayerController"
                };
            playerController.AddComponent(Type.GetType(className));
            PlayerController pc = playerController.GetComponent<PlayerController>();

            /** If the method was given custom Player Controller data, set the PC's data to that, otherwise use defaults **/
            if (PCObject != null)
            {
                pc.data = PCObject;
            }
            else
            {
                pc.data = defaultPlayerControllerSO;
            }
            pc.PCConstructor();
        }
    }

}
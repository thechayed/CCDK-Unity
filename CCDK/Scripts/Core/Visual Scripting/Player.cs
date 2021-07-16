using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;
using System.Collections.Generic;

namespace CCDKVisualScripting
{
    public class Players : MonoBehaviour
    {
        /** Gets a Player Controller by it's index **/
        public static Controller GetPlayerController(int index)
        {
            return PlayerManager.playerControllers[index];
        }

        /** Gets all the player controllers in the game **/
        public static List<Controller> GetPlayerControllers()
        {
            return PlayerManager.playerControllers;
        }
    }
}
using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;

namespace CCDKVisualScripting
{
    public class Levels : MonoBehaviour
    {
        /** Go To a Level in the game, and return the Level Component for further modification **/
        public string GoToLevel(string levelName)
        {
            LevelManager.GoToLevel(levelName);
            return LevelManager.nextLevel;
        }
        
        /** Get the current Level from the Level Manager **/
        public string GetActiveLevel()
        {
            return LevelManager.currentLevel;
        }

        /** Get whether the first level has been loaded. **/
        public static bool StartingLevelLoaded()
        {
            return Engine.startingLevelLoaded;
        }

    }
}
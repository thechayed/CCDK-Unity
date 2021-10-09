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
            return LevelManager.currentLevelName;
        }

        /** Get whether the first level has been loaded. **/
        public static bool StartingLevelLoaded()
        {
            return Engine.singleton.data.startingLevelLoaded;
        }

        public static Transform GetLevelTransform(string name)
        {
            return LevelManager.GetLevel(name).transform;
        }

        public static void SetLevelTransform(string name,Vector3 position, Vector3 rotation, Vector3 scale)
        {
            GameObject level = LevelManager.GetLevelObject(name);
            level.transform.position = position;
            level.transform.eulerAngles = rotation;
            level.transform.localScale = scale;
        }

        public static bool GetLevelExists(string name)
        {
            return LevelManager.GetLevel(name)!=null;
        }


    }
}
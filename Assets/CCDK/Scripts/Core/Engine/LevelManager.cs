using System.Collections;
using UnityEngine;
using CCDKGame;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace CCDKEngine
{
    public class LevelManager
    {
        /** The Level who's scene is currently active **/
        public static string currentLevel;
        /** The Level to go to when it has finished loading **/
        public static string nextLevel;
        /** A List of all the currently loaded Levels **/
        public static List<GameObject> loadedLevels = new List<GameObject>();


        /** Simply returns whether any Level is currently loaded and active **/
        public static bool GetInLevel()
        {
            bool inLevel = false;
            if (SceneManager.GetActiveScene().name != "Engine" && currentLevel != null && nextLevel == currentLevel)
            {
                inLevel = true;
            }
            return inLevel;
        }

        /** Go to Level by Name **/
        public static void GoToLevel(string levelName)
        {
            Debug.Log("Went to Level");
            GameObject newLevel = CreateNewLevel(levelName);
            nextLevel = levelName;
            loadedLevels.Add(newLevel);
        }

        /** Create and Load a level by name **/
        public static GameObject CreateNewLevel(string levelName)
        {
            ManageEngine();
            GameObject levelObj = new GameObject();
            levelObj.name = "Level_" + levelName;
            levelObj.AddComponent<Level>();
            Level level = levelObj.GetComponent<Level>();
            level.scene = level.LoadScene(levelName);
            level.levelName = levelName;
            ReturnToGameplay();
            return levelObj;
        }

        /** Recieved from a Level object, tells the Manager that it has finished loading **/
        public static void LevelLoaded(string level)
        {
            if(level == nextLevel)
            {
                /** Manage the Engine scene so that we can make modifications to the Levels that exist **/
                ManageEngine();
                /** Set the current Level **/
                currentLevel = level;
                /** Drop any inactive levels, if the Engine is told to **/
                DropInactiveLevels();
                /** Return to the Game Scene **/
                ReturnToGameplay();
            
                if(level == Engine.sStartingLevelName)
                {
                    Engine.startingLevelLoaded = true;
                }
            }
        }

        /** Destroys levels that are no longer active **/
        public static void DropInactiveLevels()
        {
            if (Engine.sdropLevelOnLoad)
            {
                foreach (GameObject level in loadedLevels)
                {
                    if (level.GetComponent<Level>().levelName != currentLevel)
                    {
                        SceneManager.UnloadSceneAsync(level.GetComponent<Level>().scene);
                        GameObject.Destroy(level);
                    }
                }
            }
        }

        /** Sets the Engine scene active for making modifications to Levels **/
        public static void ManageEngine()
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Engine"));
        }

        /* ALWAYS CALL THIS FUNCTION AFTER ManageEngine()! 
         * If you don't, The Engine Scene will be active in Gameplay! 
         * Don't be Stupid! */
        public static void ReturnToGameplay()
        {
            if (currentLevel != null)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLevel));
            }
        }
    }
}
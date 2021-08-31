using System.Collections;
using UnityEngine;
using CCDKGame;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace CCDKEngine
{
    public class LevelManager
    {


        /**<summary> The Level who's scene is currently active </summary>**/
        public static string currentLevel;
        /**<summary> The Level to go to when it has finished loading </summary>**/
        public static string nextLevel;
        /**<summary> A List of all the currently loaded Levels </summary>**/
        public static List<GameObject> loadedLevels = new List<GameObject>();


        /**<summary> Get whether any Level is currently loaded and active </summary>**/
        public static bool GetInLevel()
        {
            bool inLevel = false;
            if (SceneManager.GetActiveScene().name != "Engine" && currentLevel != null && nextLevel == currentLevel)
            {
                inLevel = true;
            }
            return inLevel;
        }

        /**<summary> Go to Level by Name </summary>**/
        public static void GoToLevel(string levelName)
        {
            GameObject newLevel = CreateNewLevel(levelName);
            nextLevel = levelName;
            loadedLevels.Add(newLevel);
        }

        /**<summary> Create and Load a level by name </summary>**/
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

        /**<summary> Tell the Level Manaaager that the level has finished loading. </summary>**/
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
            
                if(level == Engine.data.startingLevelName)
                {
                    Engine.data.startingLevelLoaded = true;
                }
            }
        }

        /**<summary> Destroys levels that are no longer active </summary>**/
        public static void DropInactiveLevels()
        {
            if (Engine.data.dropLevelOnLoad)
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

        /**<summary> Sets the Engine scene active for making modifications to Levels </summary>**/
        public static void ManageEngine()
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Engine"));
        }

        /* <summary> ALWAYS CALL THIS FUNCTION AFTER ManageEngine()! 
         * If you don't, The Engine Scene will be active in Gameplay! </summary>*/
        public static void ReturnToGameplay()
        {
            if (currentLevel != null)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLevel));
            }
        }


        /**<summary>
         * Creates the Runtime Game Object from the Prefab given the Engine Scriptable Object
         * </summary>**/
        public static void MakeEngineScene()
        {
            SceneManager.CreateScene("Engine");
        }
    }
}
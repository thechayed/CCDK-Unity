using System.Collections;
using UnityEngine;
using CCDKGame;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace CCDKEngine
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager singleton;

        /**<summary> The Level who's scene is currently active </summary>**/
        public static string currentLevelName;
        public Transform currentLevelTransform;
        /**<summary> The Level to go to when it has finished loading </summary>**/
        public static string nextLevel;
        /**<summary> A List of all the currently loaded Levels </summary>**/
        public List<GameObject> loadedLevelObjects = new List<GameObject>();
        public List<Level> loadedLevels = new List<Level>();

        public static Scene engineScene;

        private void Awake()
        {
            LevelManager.singleton = this;
        }

        /**<summary>Reset all data on initialization</summary>**/
        public static void Initialize()
        {
            singleton.loadedLevelObjects = new List<GameObject>();
            currentLevelName = null;
            nextLevel = null;
        }

        /**<summary> Get whether any Level is currently loaded and active </summary>**/
        public static bool GetInLevel()
        {
            bool inLevel = false;
            if (SceneManager.GetActiveScene().name != "Engine" && currentLevelName != null && nextLevel == currentLevelName)
            {
                inLevel = true;
            }
            return inLevel;
        }

        /**<summary> Go to Level by Name, and load Game Type. /summary>**/
        public static void GoToLevel(string levelName)
        {
            GameObject newLevel = CreateNewLevel(levelName);
            nextLevel = levelName;

            /**If we haven't already requested a game type when going to the Level, add the first Game Type in the Level Info's list of compatible Game Types.**/
            if(Engine.singleton.requestedGameType == null)
            {
                newLevel.AddComponent(newLevel.GetComponent<Level>().levelData.compatibleGameTypes[0].gameTypeClass.GetAssemblyType());
                newLevel.GetComponent<GameType>().data = newLevel.GetComponent<Level>().levelData.compatibleGameTypes[0];
            }

            SceneManager.SetActiveScene(newLevel.scene);
            
        }

        /**<summary> Create and Load a level by name </summary>**/
        public static GameObject CreateNewLevel(string levelName)
        {
            ManageEngine();
            GameObject levelObj = new GameObject();
            levelObj.name = "Level_" + levelName;
            levelObj.AddComponent<Level>();
            Level level = levelObj.GetComponent<Level>();
            level.levelName = levelName;
            level.LoadScene(levelName);
            level.data = Engine.GetLevelInfoByName(levelName);
            ReturnToGameplay();
            return levelObj;
        }

        /**<summary>Creates a Level Object for a Scene that already exists (Typically used in the Editor when starting Play Mode on the open Scene).
         * Also, load the Level Data for the Level created for the given scene, as well as it's Game Type.</summary>**/
        public static GameObject CreateLevelFromScene(Scene scene)
        {
            ManageEngine();
            GameObject levelObj = new GameObject();
            levelObj.name = "Level_" + scene.name;
            levelObj.AddComponent<Level>();
            Level level = levelObj.GetComponent<Level>();
            level.scene = scene;
            level.levelName = scene.name;

            level.levelData = Engine.GetLevelInfoByName(scene.name);

            /**If we haven't already requested a game type when going to the Level, add the first Game Type in the Level Info's list of compatible Game Types.**/
            if (Engine.singleton.requestedGameType == null)
            {
                levelObj.AddComponent(level.levelData.compatibleGameTypes[0].gameTypeClass.GetAssemblyType());
                GameType gameType = levelObj.GetComponent<GameType>();

                gameType.data = level.levelData.compatibleGameTypes[0];
            }

            ReturnToGameplay();
            return levelObj;
        }

        /**<summary> Tell the Level Manager that a scene is done loading, in order to go to the next Level. </summary>**/
        public static void CheckIfNextLevel(string level, Transform lobject)
        {
            if(level == nextLevel)
            {
                /** Manage the Engine scene so that we can make modifications to the Levels that exist **/
                ManageEngine();
                /** Set the current Level **/
                currentLevelName = level;
                singleton.currentLevelTransform = lobject;
                /** Drop any inactive levels, if the Engine is told to **/
                DropInactiveLevels();
                /** Return to the Game Scene **/
                ReturnToGameplay();
            
                if(level == Engine.singleton.data.startingLevelName)
                {
                    Engine.singleton.data.startingLevelLoaded = true;
                }
            }
        }

        /**<summary> Destroys levels that are no longer active </summary>**/
        public static void DropInactiveLevels()
        {
            if (Engine.singleton.data.dropLevelOnLoad)
            {
                foreach (Level level in singleton.loadedLevels)
                {
                    if (level.levelName != currentLevelName)
                    {
                        SceneManager.UnloadSceneAsync(level.GetComponent<Level>().scene);
                        GameObject.Destroy(level);
                    }
                }
            }
        }

        /** Get the Game Object from a Level that has been loaded **/
        public static GameObject GetLevelObject(string name)
        {
            foreach(Level level in singleton.loadedLevels)
            {
                if (level.levelName == name)
                {
                    return level.gameObject;
                }
            }

            return null;
        }
        /** Get the Component from a Level that has been loaded **/
        public static Level GetLevel(string name)
        {
            foreach (Level level in singleton.loadedLevels)
            {
                if (level.levelName == name)
                {
                    return level.GetComponent<Level>();
                }
            }

            return null;
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
            if (currentLevelName != null)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLevelName));
            }
        }


        /**<summary>
         * Creates the Runtime Game Object from the Prefab given the Engine Scriptable Object
         * </summary>**/
        public static void MakeEngineScene()
        {
            engineScene = SceneManager.CreateScene("Engine");
        }

        /**<summary>Tell the Level Manager that the Level has been Loaded, and store references between the Level and Objects within it so they can communicate.
         * Also, Create the Scene. 
         * If the Scene belonging to the Level object has already been created, then Remove the Level Object.</summary> **/
        public static void AddLevel(GameObject level)
        {
            Level levelInfo = level.GetComponent<Level>();

            if (FindLevelWithScene(levelInfo.scene)==null)
            {

                singleton.loadedLevels.Add(level.GetComponent<Level>());
                singleton.loadedLevelObjects.Add(level);

                levelInfo.Objects.AddRange(levelInfo.scene.GetRootGameObjects());

                level.transform.SetParent(levelInfo.Objects[0].transform);
                level.transform.SetParent(null);

                foreach (GameObject lObject in levelInfo.Objects)
                {
                    if(lObject.transform.root == lObject.transform)
                    {
                        lObject.transform.SetParent(level.transform);
                    }

                    Object objectComponent = lObject.GetComponent<Object>();
                    if (objectComponent != null)
                        objectComponent.AppendToLevel(level);

                    if (lObject.GetComponent<SpawnPoint>() != null)
                    {
                        level.GetComponent<Level>().spawnPoints.Add(lObject);
                    }
                }
                if(levelInfo.scene == null)
                {
                    levelInfo.scene = levelInfo.LoadScene(levelInfo.levelName);
                }
            }
            else
            {
                RemoveLevel(level);
            }
        }

        /**<summary>Destroy a Level from the Engine (and it's References).</summary>**/
        public static void RemoveLevel(GameObject level)
        {
            singleton.loadedLevels.Remove(level.GetComponent<Level>());
            singleton.loadedLevelObjects.Remove(level);
            GameObject.Destroy(level);
        }

        /**<summary>Find the Level Object that is attached to an open scene.</summary>**/
        public static GameObject FindLevelWithScene(Scene scene)
        {
            foreach(Level level in singleton.loadedLevels)
            {
                if(level.scene == scene)
                {
                    return level.gameObject;
                }
            }

            return null;
        }

        /**<summary>Tell the Level and Object about each other if the Object wasn't created through normal means.</summary> **/
        public static void AcknowledgeObject(GameObject objectCreated)
        {
            if (FindLevelWithScene(objectCreated.scene) != null)
            {
                Level level = FindLevelWithScene(objectCreated.scene).GetComponent<Level>();
                level.Objects.Add(objectCreated);
                if(objectCreated.transform == objectCreated.transform.root)
                    objectCreated.transform.parent = level.transform;
                objectCreated.GetComponent<Object>().level = level;
            }
        }

        public void FindGameType(Level level)
        {

        }

        public static Level GetLevelByName(string name)
        {
            foreach(Level Level in singleton.loadedLevels)
            {
                if(Level.levelName == name)
                {
                    return Level;
                }
            }
            return null;
        }

        /**<summary>Get a Spawn Point from the current level.</summary>**/
        public static Transform FindSpawn()
        {
            Level curLevel = GetLevelByName(currentLevelName);
            if (curLevel != null)
                return curLevel.FindSpawn();
            else
                return null;
        }
    }
}
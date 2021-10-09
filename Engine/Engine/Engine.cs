using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using CCDKGame;
using B83.Unity.Attributes;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

#if MLAPI
using MLAPI;
#endif

namespace CCDKEngine
{
    public class Engine : MonoBehaviour
    {
        public static Engine singleton;

        /**<summary>An array of all Level Data found in the Resources folder.</summary>**/
        public CCDKObjects.Level[] levelInfos;
        private CCDKObjects.Level defaultLevel;

        public CCDKObjects.GameTypeInfo[] gameInfos;

        public CCDKObjects.Controller[] controllerInfos;

        /**<summary> The Engine Data, recieved from the last created Engine object </summary>**/
        public CCDKObjects.Engine data;

        public static int lastEditorObjectsLength = 0;
        /** This dictionary stores references to generated Pawn Object Prefabs **/
        public static List<CCDKObjects.PrefabSO> editorObjects = new List<CCDKObjects.PrefabSO>();

        /**<summary> Whether the Runtime has started </summary>**/
        public static bool running;

        public static GameObject objectPrefab;

        public static List<GameObject> pawns = new List<GameObject>();

        public static GameObject runtimeObj;

        public static StateMachine runtime;



        /**<summary>The Game Type that is requested for a Level that is to be loaded.</summary>**/
        [Tooltip("The Game Type that is requested for a Level that is to be loaded.")]
        public CCDKObjects.GameTypeInfo requestedGameType;

        /**<summary>True if multiplayer is running</summary>**/
        public static bool multiplayerRunning = false;

        /**<summary>The currently active scene's Level</summary>**/
        public static Level currentLevel;

        public bool initialized = false;
        public static bool initCalled = false;

        private static int initCount = 0;

        private static int ticksToWaitForData = 5;
        private static int ticks = 0;

        private bool usingMLAPI = false;

#if MLAPI
        public static NetworkManager networkManager;
#endif

        private void Start()
        {
            /*Begin play!*/
            running = true;
            initCalled = true;

#if MLAPI
            usingMLAPI = true;
#endif
        }

        /** Initialize the engine loop **/
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            /** Create the Engine scene so that Engine ops can be used in Runtime **/
            LevelManager.MakeEngineScene();

            GameObject engineObject = new GameObject();
            engineObject.name = "Engine";
            GameObject.DontDestroyOnLoad(engineObject);
            singleton = engineObject.AddComponent<Engine>();
        }


        private void Update()
        {
            if(data == null)
            {
                data = (CCDKObjects.Engine)Resources.Load<ScriptableObject>("CCDK/Engine");
                data.startingLevelLoaded = false;
                data.loaded = false;
            }

            /** Create the Engine scene and load the first level **/
            if (data != null && !data.startingLevelLoaded)
            {
                InitEngineTools();
                initialized = true;
            }
            if (initialized != true)
            {
                if (data == null)
                {
                    Debug.LogWarning("The CCDK Engine couldn't find the Data it needs, it will create a temporary replacement.");

                    data = ScriptableObject.CreateInstance<CCDKObjects.Engine>();
                    data.useOpenScene = true;
                }
                else
                {
                    data.startingLevelLoaded = false;
                }
            }

            if(PlayerManager.PCCount == 0)
            {
                data.defaultPlayerController = CCDKObjects.Controller.CreateInstance<CCDKObjects.Controller>();
                data.defaultPlayerController.prefab = new GameObject();
                data.defaultPlayerController.prefab.name = "Default Player Controller Object";
                data.defaultPlayerController.prefab.AddComponent<PlayerController>();
                PlayerManager.NewPlayer(PlayerManager.CreatePC(data.defaultPlayerController));
                GameObject.DestroyImmediate(data.defaultPlayerController.prefab);
            }
        }

        private void InitEngineTools()
        {
            if (!initialized)
            {
                /**Find all Level Infos and Game Type Infos before starting a Level.**/
                levelInfos = Resources.LoadAll<CCDKObjects.Level>("");
                gameInfos = Resources.LoadAll<CCDKObjects.GameTypeInfo>("");
                controllerInfos = Resources.LoadAll<CCDKObjects.Controller>("");

                /**Instantiate all Game Managers**/
                GameObject[] Managers = Resources.LoadAll<GameObject>("CCDK/Managers");
                foreach (GameObject manager in Managers)
                {
                    if (manager.name == "NetworkManager")
                    {
                        if (usingMLAPI)
                        {
                            GameObject managerInstance = GameObject.Instantiate(manager);
                            managerInstance.name = manager.name;
                            SceneManager.MoveGameObjectToScene(managerInstance,LevelManager.engineScene);
                        }
                    }
                    else
                    {
                        GameObject managerInstance = GameObject.Instantiate(manager);
                        managerInstance.name = manager.name;
                        SceneManager.MoveGameObjectToScene(managerInstance, LevelManager.engineScene);
                    }
                }

                /** Initialize the Level Manager for Scene control **/
                LevelManager.Initialize();

                /** Set the current scene to the one open in the editor, and destroy it if it's not to be used. **/
                Scene currentScene = SceneManager.GetActiveScene();
                if (!data.useOpenScene)
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                }

                /** Initialize the Audio Manager to control audio in the game **/
                AudioManager.Init(data.audioData, data.audioMixer);

                /** Create the Runtime **/
                if (runtimeObj == null)
                {
                    runtimeObj = new GameObject();
                    runtimeObj.name = "Runtime";
                    runtime = runtimeObj.AddComponent<StateMachine>();
                    runtime.nest.macro = data.runtimeGraph;
                    runtime.enabled = true;
                }

                /** Tell the Level Manager which level to go to at the start **/
                if (data.useOpenScene)
                {
                    LevelManager.currentLevelName = currentScene.name;
                    LevelManager.CreateLevelFromScene(currentScene);
                    data.startingLevelLoaded = true;
                }
                else
                {
                    LevelManager.GoToLevel(data.startingLevelName);
                    data.startingLevelLoaded = true;
                }
            }

        }

        public static void AddPawn(GameObject pawn)
        {
            pawns.Add(pawn);
        }
        public static void RemovePawn(GameObject pawn)
        {
            pawns.Remove(pawn);
        }

        /**<summary>Returns the Game Object of a Pawn stored in the Pawns List</summary>**/
        public static GameObject GetPawn(string pawnName)
        {
            foreach (GameObject item in pawns)
            {
                if ((item.GetComponent<Pawn>().data as CCDKObjects.PrefabSO).objectName == pawnName)
                {
                    return item;
                }
            }

            return null;
        }

        /** Add a Scriptable Object to the list if it hasn't already **/
        public static void AddEdObject(CCDKObjects.PrefabSO data)
        {
            bool add = true;
            foreach (CCDKObjects.PrefabSO scriptable in editorObjects)
            {
                if (scriptable == data)
                {
                    add = false;
                }
            }
            if (add)
            {
                editorObjects.Add(data);
            }
        }

        /**<summary>Get all the Levels that are compatible with a Game Type</summary>**/
        public static CCDKObjects.Level[] GetLevelsCompatibleWithGameType(CCDKObjects.GameTypeInfo gameType)
        {
            List<CCDKObjects.Level> levels = new List<CCDKObjects.Level>();

            foreach(CCDKObjects.Level levelInfo in Engine.singleton.levelInfos)
            {
                foreach(CCDKObjects.GameTypeInfo gameTypeInfo in levelInfo.compatibleGameTypes)
                {
                    if (gameTypeInfo == gameType)
                        levels.Add(levelInfo);
                }  
            }

            return levels.ToArray();
        }


        public static CCDKObjects.Level GetLevelInfoByName(string name)
        {
            foreach(CCDKObjects.Level level in Engine.singleton.levelInfos)
            {
                if(level.name == name)
                {
                    return level;
                }
                if(level.name == "Default")
                {
                    Engine.singleton.defaultLevel = level;
                }
            }

            return Engine.singleton.defaultLevel;
        }

        public static CCDKObjects.Controller GetControllerByName(string name)
        {
            foreach(CCDKObjects.Controller controller in Engine.singleton.controllerInfos)
            {
                if (controller.name == name)
                    return controller;
            }
            return null;
        }
    }
}
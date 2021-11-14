using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using CCDKGame;
using B83.Unity.Attributes;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

#if !UNITY_SERVER
using System.Threading;
using System.Threading.Tasks;
#endif

#if USING_NETCODE
using Unity.Netcode;
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
        public bool initialControllerSpawned = false;

        /**<summary> The Engine Data, recieved from the last created Engine object </summary>**/
        public CCDKObjects.Engine data;

        public static int lastEditorObjectsLength = 0;
        /** This dictionary stores references to generated Pawn Object Prefabs **/
        public static List<CCDKObjects.PrefabSO> editorObjects = new List<CCDKObjects.PrefabSO>();

        public static GameType currentGameType;

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

        public static bool enableNetworking = false;

        public delegate void NetworkConnectAction();
        public static event NetworkConnectAction NetworkConnect;
        public delegate void NetworkDisconnectAction();
        public static event NetworkDisconnectAction NetworkDisconnect;
        public delegate void PlayerJoinedAction(ulong client);
        public static event PlayerJoinedAction NetworkClientJoined;
        public delegate void PlayerLeftAction(ulong client);
        public static event PlayerLeftAction NetworkClientLeft;
        public List<ulong> registeredClients = new List<ulong>();

        public static bool engineEditorsEnabled = false;

        public static Camera activeCamera;

        public static string inputDevice = "Gamepad";

#if USING_NETCODE
        public static NetworkManager networkManager;
        public static NetworkPrefabHandler networkPrefabHandler;
        public int localNetState = 0;
#endif

        private void Start()
        {
            /*Begin play!*/
            running = true;
            initCalled = true;
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


            if (data == null)
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

            if(!initialControllerSpawned)
            {
                data.defaultPlayerController = CCDKObjects.Controller.CreateInstance<CCDKObjects.Controller>();
                data.defaultPlayerController.prefab = new GameObject();
                data.defaultPlayerController.prefab.name = "Default Player Controller Object";
                data.defaultPlayerController.prefab.AddComponent<PlayerController>();
                PlayerController newPlayerController = PlayerManager.CreatePC(data.defaultPlayerController);
                newPlayerController.controllerData = data.defaultPlayerController;
                newPlayerController.SetOrigin();
                PlayerManager.NewPlayer(newPlayerController);

                GameObject.DestroyImmediate(data.defaultPlayerController.prefab);

                initialControllerSpawned = true;
            }

#if USING_NETCODE
            if(NetworkManager.Singleton!=null)
            {
                if(NetworkManager.Singleton.IsClient)
                    enableNetworking = true;
                if (NetworkManager.Singleton.IsHost)
                {
                    /**If their are less registered Clients than exist in the game, add those that aren't registered**/
                    if (registeredClients.Count < NetworkManager.Singleton.ConnectedClientsIds.Count)
                    {
                        ulong client = 0;
                        foreach (ulong item in NetworkManager.Singleton.ConnectedClientsIds)
                        {
                            bool found = true;
                            client = item;
                            foreach (ulong nextItem in registeredClients)
                            {
                                if (nextItem == item)
                                {
                                    found = false;
                                }
                            }
                            if (found)
                            {
                                registeredClients.Add(client);
                                NetworkClientJoined.Invoke(client);
                                break;
                            }
                        }
                    }

                    /**If their are more registered Clients than exist in the game, Remove those that no longer exist**/
                    if (registeredClients.Count > NetworkManager.Singleton.ConnectedClientsIds.Count)
                    {
                        ulong client = 0;
                        foreach (ulong item in NetworkManager.Singleton.ConnectedClientsIds)
                        {
                            bool found = true;
                            foreach (ulong nextItem in registeredClients)
                            {
                                if (nextItem == item)
                                {
                                    found = false;
                                }
                            }
                            if (found)
                            {
                                registeredClients.Remove(client);
                                NetworkClientLeft.Invoke(client);
                                break;
                            }
                        }

                    }
                }


                /**The Network Manager connected to a Game.**/
                if (!NetworkManager.Singleton.IsClient && singleton.localNetState != 0)
                {
                    singleton.localNetState = 0;
                    NetworkDisconnect();
                    /**Once disconnected from the Network, create a new Player Manager**/
                    GameObject Manager = Resources.Load<GameObject>("CCDK/Managers/PlayerManager");
                    GameObject managerInstance = GameObject.Instantiate(Manager);
                    managerInstance.name = Manager.name;
                    SceneManager.MoveGameObjectToScene(managerInstance, LevelManager.engineScene);
                }

                /**The Network Manager has Disconnected from a Game.**/
                if (NetworkManager.Singleton.IsClient && singleton.localNetState != 1)
                {
                    singleton.localNetState = 1;
                    NetworkConnect();
                }
            }
#endif
        }

        private void InitEngineTools()
        {
            if (!initialized)
            {
                /**Find all Level Infos and Game Type Infos before starting a Level.**/
                levelInfos = Resources.LoadAll<CCDKObjects.Level>("");
                gameInfos = Resources.LoadAll<CCDKObjects.GameTypeInfo>("");
                controllerInfos = Resources.LoadAll<CCDKObjects.Controller>("");
                defaultLevel = levelInfos[0]; 

                /**Instantiate all Game Managers**/
                GameObject[] Managers = Resources.LoadAll<GameObject>("CCDK/Managers");
                foreach (GameObject manager in Managers)
                {
                    GameObject managerInstance = GameObject.Instantiate(manager);
                    managerInstance.name = manager.name;
                    SceneManager.MoveGameObjectToScene(managerInstance, LevelManager.engineScene);
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


#if USING_NETCODE
            
            //networkPrefabHandler
#endif
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
                if(level.levelName == name)
                {
                    return level;
                }
                if(level.levelName == "Default")
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

        /**<summary>Shorthand for Unity's Game Object Destroy function that checks if the object is valid first, instead of writing that same darn condition over and over again.</summary>**/
        public static bool TryDestroy(object objectToDestroy)
        {
            if (objectToDestroy == null)
                return false;
            else
            {
                if (((Component)objectToDestroy) == null)
                    GameObject.Destroy((GameObject)objectToDestroy);
                else
                    GameObject.Destroy(((Component)objectToDestroy).gameObject);
                return true;
            }
        }

        public static void CreateNetworkManager(GameObject managerObject)
        {
            if (NetworkManager.Singleton == null)
            {
                GameObject managerInstance = GameObject.Instantiate(managerObject);
                managerInstance.name = managerObject.name;
                SceneManager.MoveGameObjectToScene(managerInstance, LevelManager.engineScene);
            }
        }

    }
}
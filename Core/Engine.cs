using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using CCDKGame;

namespace CCDKEngine
{
    public class Engine : MonoBehaviour
    {
        /** Initialize **/
        /** The name of the first Level to go to **/
        public string startingLevelName = "";
        public static string sStartingLevelName = "";

        public static bool startingLevelLoaded = false;

        /**Whether the Runtime has started**/
        public static bool runtimeInit;
        /** Whether to use a Graph to control the Runtime **/
        public static bool useRGraph;
        public bool useRuntimeGraph;
        public static StateGraphAsset rGraph;
        public StateGraphAsset runtimeGraph;

        /** The object of the Game Mode that has been loaded **/
        public GameObject gameModeObject;
        public static GameMode gameMode;
        public static GameInfo gameInfo;

        /** Whether to destroy a Level when another is set to active **/
        public static bool sdropLevelOnLoad;
        /** Enable setting this value in the Inspector **/
        public bool dropLevelOnLoad;



        public static bool engineCreated;

        public static Engine engine;

        public static GameObject engineOBJ;

        public static string defaultPCClass = "CCDKGame.PlayerController";

        private void Awake()
        {
            engine = this;
            engineOBJ = gameObject;
        }

        // Use this for initialization
        void Start()
        {
            LevelManager.GoToLevel(startingLevelName);

            useRGraph = useRuntimeGraph;
            rGraph = runtimeGraph;
            if (gameMode == null)
            {
                Instantiate(gameModeObject);
                gameMode = gameModeObject.GetComponent<GameMode>();
                gameInfo = gameModeObject.GetComponent<GameInfo>();
                /*Begin play!*/
                runtimeInit = true;
            }
            sdropLevelOnLoad = dropLevelOnLoad;
            sStartingLevelName = startingLevelName;
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerManager.playerControllers.Count == 0)
            {
                PlayerManager.CreatePC(defaultPCClass);
            }
        }

        /** Get the Game Mode object from disk, and get the components **/
        public static void GoToGameMode(string name)
        {

        }


    }
}
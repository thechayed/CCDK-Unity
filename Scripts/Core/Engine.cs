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


        /** Whether the Engine has already been created **/
        public static bool engineCreated;

        /** The Engine object **/
        public static Engine engine;
        public static GameObject engineOBJ;

        /** Defaults for Player Controllers that aren't externally defined **/
        public static string defaultPCClass = "CCDKGame.PlayerController";
        public CCDKObjects.Controller defaultPlayerControllerData;

        private void Awake()
        {
            engine = this;
            engineOBJ = gameObject;
        }

        // Use this for initialization
        void Start()
        {
            /** If the default controller Data hasn't been created, create it **/
            if (defaultPlayerControllerData == null)
            {
                defaultPlayerControllerData = CCDKObjects.Controller.CreateInstance<CCDKObjects.Controller>();
            }
            /** Set the PlayerManager's default PC Data **/
            PlayerManager.defaultPlayerControllerSO = defaultPlayerControllerData;

            /** Go to the Starting level set in Engine settings **/
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
            /** Check if the Player Manager has any records of Player Controllers. If not, create a new one from Engine default values **/
            if (PlayerManager.playerControllers.Count == 0)
            {
                PlayerManager.defaultPlayerControllerSO = defaultPlayerControllerData;
                PlayerManager.CreatePC(defaultPCClass);
            }
        }

        /** Get the Game Mode object from disk, and get the components **/
        public static void GoToGameMode(string name)
        {

        }


    }
}
using B83.Unity.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Game Info/Game Type Info")]
    public class GameTypeInfo : ScriptableObject
    {
        [Header(" - Class - ")]
        [MonoScript(type = typeof(CCDKGame.GameType))] public string gameTypeClass = "CCDKGame.GameType";

        [Header(" - Defaults - ")]
        [Tooltip("Game Type's default objects.")]
        public Controller defaultPlayerController;
        [Tooltip("Controller's State will be changed when the Game Type's State Changes.")]
        public bool tieControllerStateToGameType = true;

        public Controller defualtAIController;
        public Pawn defaultPawn;
        [Tooltip("Each posible Game Type state, paired with a List of Types that belong solely to that State. Used to Create/Destroy the correct objects for the current Game State.")]
        public Dictionary<List<ObjectType>> stateObjectPairTypes = new Dictionary<List<ObjectType>>();
        [Tooltip("Pawns should be handled differently than other Objects in the game. A State Enabled Pawn for each State should be created for each Player Controller in the game.")]
        public Dictionary<Pawn> statePawnPairs = new Dictionary<Pawn>();
        public bool stateObjectPairing = true;

        public HUD defaultHUD;

        [Header(" - Gameplay - ")]
        public bool teamGame = true;
        public bool pausable = false;
        [Tooltip("The max team size for the game.")]
        public int teamSize = 4;
        [Tooltip("How many players are allowed to join the game.")]
        public int maxPlayers = 8;
        [Tooltip("When this score is reached by a team, that team wins.")]
        public int goalScore = -1;
        [Tooltip("Themaximum amount of time a game lasts ")]
        public int timeLimit = 20;
        
        [Header(" - Netcode - ")]
       [Tooltip("Starts Multiplayer at the beginning of the game if true.")]
        public bool startInMultiplayer = false;
        [Tooltip("The Network Manager to spawn to use Multiplayer.")]
        public GameObject networkManager;


        public Dictionary<AudioClip> music = new Dictionary<AudioClip>();
    }

    [System.Serializable]
    public class ObjectType
    {
        [MonoScript(type = typeof(CCDKEngine.Object))] public string type = "";
    }
}
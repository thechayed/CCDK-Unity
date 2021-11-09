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
        public Controller defualtAIController;
        public Pawn defaultPawn;
        public HUD defaultHUD;

        [Header(" - Gameplay - ")]
        public bool teamGame = true;
        public bool pausable = false;
        [Tooltip("The max team size for the game.")]
        public int teamSize = 4;
        [Tooltip("How many players are allowed to join the game.")]
        public int maxPlayers = 8;
        [Tooltip("When this score is reached by a team, that team wins.")]
        public int goalScore
        [Tooltip("Themaximum amount of time a game lasts ")]
        public int timeLimit = 20;
        
        [Header(" - Netcode - ")]
       [Tooltip("Starts Multiplayer at the beginning of the game if true.")]
        public bool startInMultiplayer = false;
        [Tooltip("The Network Manager to spawn to use Multiplayer.")]
        public GameObject networkManager;
      
    }
}
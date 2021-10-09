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
        [Tooltip("If this is set to false, the user must communicate game state to the Game Type. (in the case of using the Runtime State Graph to handle game state, it is typically best to omit Game Type entirely.)")]
        public bool useOwnGameState = true;
        [Tooltip("Should the Game Type handle the existence of Player Controllers?")]
        public bool managePlayerControler = true;
        [Tooltip("Should Player Input be handled by the Player Manager. Set per Game Type to enable/disable split screens, important for certain kinds of games.")]
        public bool usePlayerManager = false;
        [Tooltip("Should the Game Type handle the existence of Pawns?")]
        public bool managePawn = true;

        [Header(" - Common Properties - ")]
        public Dictionary<int> Stats =  new Dictionary<int>(){dictionary = new List<DictionaryItem<int>>()
        {
            new DictionaryItem<int>("MaxPlayers", 16),
            new DictionaryItem<int>("MaxPlayersAllowed", 16),
            new DictionaryItem<int>("GoalScore", 25),
            new DictionaryItem<int>("TimeLimit", 30)
        }};
        public Dictionary<bool> Flags = new Dictionary<bool>(){dictionary = new List<DictionaryItem<bool>>()
        {
            new DictionaryItem<bool>("RestartLevel", false),
            new DictionaryItem<bool>("TeamGame", true)
        }};

        [Header(" - unused - ")]

        private bool RestartLevel,         // Level should be restarted when player dies
                 Pauseable,               // Whether the game is pauseable.
                 TeamGame,            // This is a team game.
                 GameEnded,               // set when game ends
                 OverTime,
                 DelayedStart,
                 WaitingToStartMatch,
                 ChangeLevels,
                 AlreadyChanged,
                 GameRestarted,
                 LevelChange;          // level transition in progress

    }
}
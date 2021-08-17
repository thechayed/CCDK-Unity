/* The GameInfo class contains all the important global information for the Game that is currently being played */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CCDKEngine
{
    public class GameInfo : MonoBehaviour
    {
        /** The name of the level to be loaded **/
        public string level;

        [System.Serializable]
        public struct GIMP
        {
            [HideInInspector]
            public bool RestartLevel,         // Level should be restarted when player dies
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

            public bool AdminCanPause,
                        KickLiveIdlers;

            public int MaxPlayers,                // Maximum number of players allowed by this server.
                       MaxPlayersAllowed,      // Maximum number of players ever allowed (MaxPlayers is clamped to this in initgame()
                       NumPlayers,             // number of human players
                       NumBots;                 // number of non-human players (AI controlled but participating as a player)

            public int GoalScore,                // what score is needed to end the match
                       MaxLives,                    // max number of lives for match, unless overruled by level's GameDetails
                       TimeLimit;                // time limit in minutes
        }
        public GIMP multiplayer;

        /** The class name of the Default Pawn **/
        public string defaultPawn,
                      HUDType;                  // HUD class this game uses.


    }
}

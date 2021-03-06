using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using CCDKEngine;
#if USING_NETCODE
using Unity.Netcode;
#endif

namespace CCDKGame
{

    public class Team 
    {
        /**Index in the Game Type's Team List that this Team is at.**/
        public int index;

        public int score;
#if USING_NETCODE
        public NetworkVariable<int> networkScore;
#endif

        public Color teamColor;

        /**<summary>The instances of Players on this Team. If a Player scores in the Game Type, we add to this Team's score!</summary>**/
        public List<Player> playersOnTeam = new List<Player>();
    
        public Team(int index)
        {
            this.index = index;
        }
    }
}
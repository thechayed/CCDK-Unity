using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    public class GameMode : Object
    {
        public string gameModeName;

        /** Set up the Game for this Game Mode **/
        public virtual void State_StartGame()
        {

        }

        /** Right before the Gameplay commences, maybe an animation to show off the map? **/
        public virtual void State_PreGame()
        {

        }

        /** Gameplay related events **/
        public virtual void State_GamePlay()
        {

        }

        /** What happens after the game has ended **/
        public virtual void State_PostGame()
        {

        }
    }
}
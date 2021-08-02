/** This is extension of the State Enabled Mono Behavior that overrides Init to only 
 * initialize once the level has been loaded. 
 * LASE: Level Activated, State Enabled **/
using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    public class LASEMono : StateMachineComponent
    {
        public override void Init()
        {
            if (LevelManager.GetInLevel())
            {
                base.Init();
            }
        }
    }
}
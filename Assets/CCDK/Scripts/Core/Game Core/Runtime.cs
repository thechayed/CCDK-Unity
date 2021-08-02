/* Extend this to control the overall runtime events. The State of the Runtime tells us what level we're in
 * and various other information. */

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class Runtime : CCDKEngine.Object
    {
        StateMachineComponent stateMachine;

        public override void Start()
        {
            stateMachine = GetComponent<StateMachineComponent>();
            stateMachine.enabled = false;
            base.Start();
        }

        /** The State Machine component is disabled until the Runtime Object has been Initialized **/
        public void Init()
        {
            stateMachine.enabled = true;
        }
    }
}

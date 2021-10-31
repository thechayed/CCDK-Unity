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
        FSM.Component stateMachine;

        public override void Start()
        {
            base.Start();
            stateMachine = GetComponent<FSM.Component>();
            stateMachine.enabled = false;
        }

        /** The State Machine component is disabled until the Runtime Object has been Initialized **/
        public void Init()
        {
            stateMachine.enabled = true;
        }
    }
}

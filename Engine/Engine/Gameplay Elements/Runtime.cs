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

        public override void Start()
        {
            base.Start();
            stateMachine = GetComponent<FSM.Component>();
        }


    }
}

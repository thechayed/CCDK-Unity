/** An extendion of FSM States to be used with Scriptment **/

using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    public class ScriptmentState : FSM.State<ScriptmentState>
    {
        public int progress;
        public bool next;

        public override void Update()
        {
            base.Update();

            if(next)
                this.GetType().GetMethod(progress.ToString()).Invoke(this, null);
        }
    }
}
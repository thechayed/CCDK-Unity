/** Monobehavior's should use this class inside so the State Machine knows which classes to enable/disable **/

using System;
using System.Collections;
using UnityEngine;

namespace FSM
{
    public class State
    {
        protected bool enabled;
        protected GameObject gameObject;
        protected object selfObj;
        protected Machine machine;

        /** When the State is added to the game, it must be initialized with a GameObject **/
        public void Init(GameObject gameObject, object parent, Machine machine)
        {
            this.gameObject = gameObject;
            this.selfObj = parent;
            this.machine = machine;
        }

        public void Enable()
        {
            enabled = true;
            Enter();
        }

        public void Disable()
        {
            Exit();
            enabled = false;
        }

        public virtual void Enter()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Exit()
        {

        }

        public void SetValue(string name, object value)
        {
            selfObj.GetType().GetField(name).SetValue(selfObj, value);
        }

        public object GetValue(string name)
        {
            return selfObj.GetType().GetField(name).GetValue(selfObj);
        }
    }
}
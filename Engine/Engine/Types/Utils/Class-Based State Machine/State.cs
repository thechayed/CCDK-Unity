/** Monobehavior's should use this class inside so the State Machine knows which classes to enable/disable **/

using System;
using System.Collections;
using UnityEngine;

namespace FSM
{
    public class State<T>
    {
        protected bool enabled;
        protected GameObject gameObject;
        protected object selfObj;
        public T self;
        protected Machine machine;

        /** When the State is added to the game, it must be initialized with a GameObject **/
        public void Init(GameObject gameObject, object parent, Machine machine)
        {
            this.gameObject = gameObject;
            this.self = (T)parent;
            this.machine = machine;
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            Exit();
            enabled = false;
        }

        /**<summary>Event called when the State is Entered</summary>**/
        public virtual void Enter()
        {

        }

        /**<summary>Event called when the State is Updating</summary>**/
        public virtual void Update()
        {

        }

        /**<summary>Event called when the State is Updating, only on the Owner's side.</summary>**/
        public virtual void NetworkUpdate()
        {

        }

        /**<summary>Event called when the State is exitted.</summary>**/
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
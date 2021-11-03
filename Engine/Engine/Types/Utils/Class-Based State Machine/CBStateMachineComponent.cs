using System;
using System.Collections;
using UnityEngine;
using CCDKGame;
using System.Reflection;
#if USING_NETCODE
using Unity.Netcode;
#endif

namespace FSM
{
#if USING_NETCODE
    public class Component : NetworkBehaviour
#else
    public class Component : MonoBehaviour
#endif
    {
        [Header(" - Data - ")]
        public ScriptableObject data;

        [HideInInspector]
        public object stateMachine;
        public Type type;

        [HideInInspector]
        protected bool initialized = false;

        [Header(" - State Machine - ")]
        [Tooltip("The State this Object is in.")]
        [ReadOnly] public string state;

        public virtual void Start()
        {
            CreateStateMachine();
        }

        public void CreateStateMachine()
        {
            stateMachine = new FSM.Machine(this, gameObject);
            StateMachineInit();
        }

        /** Update the active State in the State Machine **/
        public virtual void FixedUpdate()
        {
            state = GetState();

            if (stateMachine != null)
            {
                if (initialized)
                {
                    CallMethod(GetState(), "Update"); ;
                }
            }
        }

        /** Override the Initialize event to change the conditions for which the State Machine can be active **/
        public virtual void StateMachineInit()
        {
            if (stateMachine != null && GetMethods() != null && GetMethods().length > 0)
            {
                if (!initialized)
                {
                    initialized = true;
                }
            }
        }

        /** Make the object go to the specified state **/
        public void GoToState(string state)
        {
            stateMachine.GetType().GetMethod("GoToState").Invoke(stateMachine, new object[]{state});
        }

        /** Get this Component's current State **/
        public string GetState()
        {
            return (string)stateMachine.GetType().GetField("curState").GetValue(stateMachine);
        }

        public Dictionary<MethodInfo[]> GetMethods()
        {
            return (Dictionary<MethodInfo[]>)stateMachine.GetType().GetField("stateMethods").GetValue(stateMachine);
        }

        public void CallMethod(string state, string name)
        {
            stateMachine.GetType().GetMethod("CallMethod").Invoke(stateMachine,null);
        }
    }
}
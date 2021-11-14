using System;
using System.Collections;
using UnityEngine;
using CCDKGame;
using System.Reflection;
using System.Collections.Generic;
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
        [ReadOnly] public List<string> stateList = new List<string>();
 
        public virtual void Start()
        {
            CreateStateMachine();
        }

        public void CreateStateMachine()
        {
            stateMachine = new FSM.Machine(this, gameObject);
            StateMachineInit();

            /**Add the names of the States in the Class to the StateList**/
            var stateMachineAsMachine = (FSM.Machine)stateMachine;
            foreach(DictionaryItem<object> state in stateMachineAsMachine.stateInstances.dictionary)
            {
                stateList.Add(state.value.GetType().Name);
            }
        }

        /** Update the active State in the State Machine **/
        public virtual void FixedUpdate()
        {
            state = GetState();

            if (stateMachine != null)
            {
                if (initialized)
                {
                    CallStateMethod(GetState(), "Update");
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
        public void GoToState(string stateTo)
        {
            stateMachine.GetType().GetMethod("GotoState").Invoke(stateMachine, new object[]{ stateTo });
        }

        /** Get this Component's current State **/
        public string GetState()
        {
            //Dictionary<object> localDic = (Dictionary<object>)stateMachine.GetType().GetField("stateInstances").GetValue(stateMachine);
            //List<DictionaryItem<object>> localList = (List<DictionaryItem<object>>)localDic.GetType().GetField("dictionary").GetValue(localDic);
            //Debug.Log(this.GetType());
            
            return (string)stateMachine.GetType().GetField("curState").GetValue(stateMachine);
        }

        public Dictionary<MethodInfo[]> GetMethods()
        {
            return (Dictionary<MethodInfo[]>)stateMachine.GetType().GetField("stateMethods").GetValue(stateMachine);
        }

        public void CallStateMethod(string state, string name, object[] parameters = null)
        {
            stateMachine.GetType().GetMethod("CallMethod").Invoke(stateMachine,new object[] { state , name, parameters});
        }

        /**<summary>The state of the machine has finished changing.</summary>**/
        public virtual void StateChanged(string prevState)
        {

        }
    }
}
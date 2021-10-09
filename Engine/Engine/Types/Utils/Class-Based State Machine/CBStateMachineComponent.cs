using System;
using System.Collections;
using UnityEngine;
using CCDKGame;

namespace FSM
{

    public class Component : DataDependentMonobehavior
    {

        [HideInInspector]
        protected FSM.Machine stateMachine;

        [HideInInspector]
        protected bool initialized = false;

        [Header(" - State Machine - ")]
        [Tooltip("The State this Object is in.")]
        [ReadOnly] public string state;

        /** When the Object Starts in the game, ask it to Initialize **/
        public virtual void Start()
        {
            stateMachine = new FSM.Machine(this, gameObject);
            Init();
        }        

        public void CreateStateMachine(Component component)
        {
        }

        /** Update the active State in the State Machine **/
        public virtual void FixedUpdate()
        {
            state = stateMachine.curState;

            if (stateMachine != null)
            {
                if (initialized)
                {
                    stateMachine.CallMethod(stateMachine.curState, "Update"); ;
                }
            }
        }

        /** Override the Initialize event to change the conditions for which the State Machine can be active **/
        public virtual void Init()
        {
            if (stateMachine != null && stateMachine.stateMethods != null && stateMachine.stateMethods.length > 0)
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
            this.stateMachine.GotoState(state);
        }

        /** Get this Component's current State **/
        public string GetState()
        {
            return stateMachine.curState;
        }
    }
}
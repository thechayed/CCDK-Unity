﻿using System.Collections;
using UnityEngine;

namespace FSM
{

    public class Component : MonoBehaviour
    {
        public FSM.Machine stateMachine;

        public bool initialized = false;

        /** When the Object Starts in the game, ask it to Initialize **/
        public virtual void Start()
        {
            stateMachine = new FSM.Machine(this, gameObject);
            Init();
        }        

        /** Update the active State in the State Machine **/
        public virtual void FixedUpdate()
        {
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
            if (stateMachine != null && stateMachine.states != null && stateMachine.states.length > 0)
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
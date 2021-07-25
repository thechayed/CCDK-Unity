using System.Collections;
using UnityEngine;

namespace CCDKEngine
{

    public class StateEnabledMonoBehavior : MonoBehaviour
    {
        public SimpleState state = new SimpleState
        {
            States = new string[]
                    {
                    "Default"
                    },
            curState = "",
            prevState = ""
        };

        public bool initialized = false;

        /** When the Object Starts in the game, ask it to Initialize **/
        public virtual void Start()
        {
            Init();
        }        

        /* Call the function related to the current State if it exists */
        public virtual void FixedUpdate()
        {
            if (state != null)
            {
                if (!initialized)
                {
                    Init();
                }
                StateUpdate();
            }
        }

        /** Override the Initialize event to change the conditions for which the State Machine can be active **/
        public virtual void Init()
        {
            if (state != null && state.States.Length > 0)
            {
                if (!initialized)
                {
                    this.GoToState(state.States[0]);
                    initialized = true;
                }
            }
        }

        /** Overriding the State Update gives control as to how the can Update **/
        public virtual void StateUpdate()
        {
            /** Always prefer using State functions for updates instead of the Update function! **/
            if (state.States.Length > 0)
            {
                CallStateMethod("State_" + state.curState);
            }
        }

        /** Make the object go to the specified state **/
        public void GoToState(string state)
        {
            /** If the State exists, go to it and call the Enter and Exit methods **/
            if(this.state.GetState(state))
            {
                CallStateMethod("State_" + this.state.curState + "_Exit");
                this.state.GotoState(state);
                CallStateMethod("State_" + this.state.curState + "_Enter");
            }
        }

        public string GetState()
        {
            return state.curState;
        }

        /** Set the array of states from another **/
        public void SetStateList(string[] states)
        {
            state.States = states;
        }

        /** Calls amy method that exists within this class (Checks if it exists first to dodge warning) **/
        public void CallStateMethod(string method)
        {
            if (this.GetType().GetMethod(method) != null)
            {
                Invoke(method, 0);
            }
        }
    }
}
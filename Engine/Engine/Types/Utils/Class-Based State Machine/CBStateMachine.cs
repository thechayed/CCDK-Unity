/** The Class Based State Machine uses reflection to check for State derived Classes in the Component that is passed to it
 * to store a Dictionary of States by their Name as the Key, and Methods as an Array as the Value. 
 * It is then used to call the methods for those State's based on basic State behaviors, such as Enter, Update, and Exit
 * which allow for a simple State based interface for Scripts. **/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FSM
{
    [System.Serializable]
    public class Machine
    {
        [SerializeField]
        /** The Class that this Simple State Machine is controlling **/
        public FSM.Component component;
        public GameObject gameObject;
        public Dictionary<MethodInfo[]> stateMethods;
        public Dictionary<State> stateInstances;
        public string curState;
        public string prevState;
        public object parent;


        bool stateInit = false;

        /** Construct the State Machine and Initialize all the States **/
        public Machine(object component, GameObject gameObject)
        {
            this.component = (FSM.Component) component;
            this.gameObject = gameObject;
            this.parent = component;

            stateMethods = new Dictionary<MethodInfo[]>();
            stateInstances = new Dictionary<State>();

            /** Loop through all the Types in our given SEMB, and add it to our State Type List **/
            foreach (Type type in component.GetType().GetNestedTypes())
            {
                if(type.BaseType.Name == "State")
                {
                    /** Set up the State Dictionary and Initialize States **/
                    stateMethods.Set(type.Name, type.GetMethods());
                    stateInstances.Set(type.Name, (State) Activator.CreateInstance(type));
                    CallMethod(type.Name, "Init", new object[] { gameObject, parent, this });

                    /** If the current state hasn't been set yet, go to this one. **/
                    if ((curState == "" | curState == null)&&!stateInit)
                    {
                        GotoState(type.Name);
                        stateInit = true;
                    }
                }
            }
        }


        /*-- Active State Control --*/
        /** Go to State by Name **/
        public void GotoState(string name)
        {
            foreach (DictionaryItem<MethodInfo[]> item in stateMethods.dictionary)
            {
                string s = item.key;
                if (s == name)
                {
                    CallMethod(curState, "Exit");
                    CallMethod(curState, "Disable");
                    prevState = curState;
                    curState = name;
                    CallMethod(curState, "Enable");
                    CallMethod(curState, "Enter");
                }
            }
        }

        /* Reflection */
        /** Call a method if it exists **/
        public void CallMethod(string state, string methodName, object[] parameters = null)
        {
            int index = GetMethodIndex(state, methodName);  
            if (index>-1)
            {
                stateMethods.Get(state)[index].Invoke(stateInstances.Get(state), parameters);
            }
        }

        /** Check if a Method exists **/
        public int GetMethodIndex(string typeName, string methodName)
        {
            int index = 0;
            if(typeName != null || typeName == "")
            {
                foreach (MethodInfo method in stateMethods.Get(typeName))
                {
                    if (method.Name == methodName)
                    {
                        return index;
                    }
                    index++;
                }
            }
            return -1;
        }
    }
}

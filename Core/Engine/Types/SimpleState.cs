using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    [System.Serializable]
    public class SimpleState
    {
        [SerializeField]
        public string[] States;
        public string curState;
        public string prevState;

        public void GotoState(string name)
        {
            foreach (string s in States)
            {
                if (s == name)
                {
                    prevState = curState;
                    curState = name;
                }
            }
        }

        public bool GetState(string name)
        {
            bool exists = false;
            foreach (string s in States)
            {
                if (s == name)
                {
                    exists = true;
                }
            }
            return exists;
        }
    }

}

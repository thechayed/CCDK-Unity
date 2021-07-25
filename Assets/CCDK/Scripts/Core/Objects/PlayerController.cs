using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace CCDKObjects
{
    public class PlayerController : Controller
    {
        [System.Serializable]
        public class Input : StateEnabledObject
        {
            /** Customizable Input events **/
            public delegate void InputAction(string name, string type, float value);
            public List<InputAction> input = new List<InputAction>();
        }
        public Input inputInfo;
    }
}
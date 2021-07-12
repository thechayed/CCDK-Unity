using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class PlayerInput : ControllerInput
    {
        public delegate void InputAction(string name, string type, float value);
        public List<InputAction> input = new List<InputAction>();

    }
}

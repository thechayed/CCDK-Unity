/* Controller Input handles interaction between the Controller and it's Possessed objects */

using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace CCDKEngine
{
    public class ControllerInput
    {
        public Controller controller;
        private PlayerInput input;

        public void Init()
        {
            input = controller.gameObject.AddComponent<PlayerInput>();
            //input.SwitchCurrentActionMap(controller.data.inputActionAsset.name);
        }
    }
}

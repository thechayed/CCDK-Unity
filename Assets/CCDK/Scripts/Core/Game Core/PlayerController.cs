using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;
using UnityEngine.InputSystem;

namespace CCDKGame
{
    public class PlayerController : Controller
    {
        /**Player Info*/
        public Player player;

        /** Input Action Collection script. Input actions are used in the Player Controller to get control from the Player **/
        IInputActionCollection inputActions;

        public override void Start()
        {
            base.Start();
            PlayerManager.AddPC(this);
            if (Type.GetType(data.inputInfo.inputActions).BaseType.Name == "IInputActionCollection")
            {
                inputActions = (IInputActionCollection)Type.GetType(data.inputInfo.inputActions);
            }
        }
    }

}
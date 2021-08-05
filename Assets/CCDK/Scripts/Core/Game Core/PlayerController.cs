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

        public override void Start()
        {
            base.Start();
            PlayerManager.AddPC(this);
        }
    }

}
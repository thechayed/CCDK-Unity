﻿using System.Collections;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class PawnCamera : CCDKEngine.Object
    {
        // Use this for initialization
        public override void Start()
        {
            transform.parent.SendMessageUpwards("SetCamera", gameObject.GetComponent<Camera>());
        }
    }
}
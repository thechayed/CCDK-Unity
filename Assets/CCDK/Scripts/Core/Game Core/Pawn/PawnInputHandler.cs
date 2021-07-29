using System.Collections;
using UnityEngine;
using CCDKEngine;
using System;

namespace CCDKGame
{
    public class PawnInputHandler : PawnClass
    {
        public Dictionary<Func<int>> inputs;

        public override void Start()
        {
            base.Start();
        }
    }
}
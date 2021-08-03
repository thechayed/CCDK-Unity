using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;
using System.Collections.Generic;

namespace TemplateGame
{
    public class TMPPawn : Pawn
    {
        public override void Awake()
        {
        }

        public class Normal : FSM.State
        {
            TMPPawn self;

            public override void Enter()
            {
                self = (TMPPawn)selfObj;
            }

            public override void Update()
            {
                base.Update();
            }
        }
    }
}
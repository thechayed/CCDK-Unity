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
            stateMachine = new CBStateMachine(this, gameObject);
            
        }

        public class Normal : State
        {
            public override void Enter()
            {
                base.Enter();
                Debug.Log("Hello World!");
            }
        }
    }
}
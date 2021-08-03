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
            stateMachine = new FSM.Machine(this, gameObject);
            
        }

        public class Normal : FSM.State
        {
            public override void Enter()
            {
                Debug.Log("Hello World!");
                base.Enter();
            }

            public override void Update()
            {

                Debug.Log("I'm updating!");
                base.Update();
            }
        }
    }
}
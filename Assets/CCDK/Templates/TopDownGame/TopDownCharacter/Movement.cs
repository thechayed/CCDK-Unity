using CCDKGame;
using System.Collections;
using UnityEngine;

namespace Template.TopDownGame
{
    public class Movement : PawnMovement
    {

        public class Default : FSM.State
        {
            public PawnMovement self;
            
            public override void Enter()
            {
                self = (PawnMovement)selfObj;
                self.GenerateController(typeof(CharacterController));
            }

            public override void Update()
            {
            }
        }
    }
}
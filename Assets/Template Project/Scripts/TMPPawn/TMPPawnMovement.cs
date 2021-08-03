using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;
using System;

namespace TemplateGame
{
    public class TMPPawnMovement : PawnMovement
    {
        public CharacterController characterController;

        public class Normal : FSM.State
        {
            TMPPawnMovement self;

            public override void Enter()
            {
                self = (TMPPawnMovement)selfObj;
                gameObject.AddComponent<CharacterController>();
                SetValue("characterController", gameObject.GetComponent<CharacterController>());
            }

            public override void Update()
            {
                if (self.characterController != null)
                {
                    CalcVelocity();
                    self.characterController.Move(self.velocity);
                }
            }

            public void CalcVelocity()
            {
                if (!self.characterController.isGrounded)
                {
                    self.velocity.y -= self.pawn.data.movementInfo.fallAccelRate * self.dt;
                }
                else
                {
                    self.velocity.y = 0;
                }
            }
        }
    }
}
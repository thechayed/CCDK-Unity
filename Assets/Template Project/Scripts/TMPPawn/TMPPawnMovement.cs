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
        public bool landed = false;
        public bool jumping = false;

        public class Normal : FSM.State
        {
            TMPPawnMovement self;

            public override void Enter()
            {
                self = (TMPPawnMovement)selfObj;
                if (gameObject.GetComponent<CharacterController>() == null)
                {
                    gameObject.AddComponent<CharacterController>();
                }
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
                    self.velocity.y -= self.pawn.data.movementInfo.fallAccelRate * Time.deltaTime;
                    self.landed = false;
                    if (self.velocity.y < 0)
                    {
                        self.jumping = false;
                    }
                }
                else if(!self.landed && !self.jumping)
                {
                    self.velocity.y = -0.1f;
                    self.landed = true;
                }
            }
        }
    }
}
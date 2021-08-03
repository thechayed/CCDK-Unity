using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;

namespace TemplateGame
{
    public class TMPPawnInputHandler : PawnInputHandler
    {
        TMPPawnMovement pawnMovement;
        public Vector3 movement = new Vector3(0, 0, 0);
        public byte[] walk = {0,0};

        public void MoveLeft()
        {
            movement.x = Mathf.Lerp(movement.x, -0.5f, 0.5f);
            walk[0] = 1;
        }
        public void MoveLeft_Cancel()
        {
            walk[0] = 0;
        }
        public void MoveRight()
        {
            movement.x = Mathf.Lerp(movement.x, 0.5f, 0.5f);
            walk[1] = 1;
        }
        public void MoveRight_Cancel()
        {
            walk[1] = 0;
        }

        public void Jump()
        {
            if (pawnMovement.characterController.isGrounded)
            {
                pawnMovement.velocity.y = 0.5f;
                pawnMovement.jumping = true;
            }
            Debug.Log("Jump was pressed!");
        }

        private void Update()
        {
            if(((int)walk[0]+(int)walk[1]) == 0)
            {
                movement.x = Mathf.Lerp(movement.x, 0, 0.1f);
            }
        }

        public class Enabled : FSM.State
        {
            TMPPawnInputHandler self;

            public override void Enter()
            {
                self = (TMPPawnInputHandler)selfObj;
                self.pawnMovement = (TMPPawnMovement)self.GetComponent<PawnMovement>();
            }

            public override void Update()
            {
                self.pawnMovement.velocity.x=self.movement.x;
                Debug.Log(self.movement);
            }
        }
    }
}
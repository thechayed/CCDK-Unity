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
        bool running = false;
        bool jumping = false;

        public void MoveLeft()
        {
            running = true;
            walk[0] = 1;
            GetComponent<PawnCostume>().MeshSetValue("Run", running);
            gameObject.transform.Find("Costume(Clone)").eulerAngles = new Vector3(0, 180, 0);
        }
        public void MoveLeft_Cancel()
        {
            walk[0] = 0;
        }
        public void MoveRight()
        {
            running = true;
            walk[1] = 1;
            GetComponent<PawnCostume>().MeshSetValue("Run", running);
            pawn.transform.Find("Costume(Clone)").eulerAngles = new Vector3(0, 0, 0);
        }
        public void MoveRight_Cancel()
        {
            walk[1] = 0;
        }

        public void Jump()
        {
            if (pawnMovement.characterController.isGrounded)
            {
                pawnMovement.velocity.y = pawn.data.movementInfo.JumpZ;
                pawnMovement.jumping = true;
            }
        }

        private void Update()
        {
            if(((int)walk[0]+(int)walk[1]) == 0)
            {
                running = false;
                movement.x = Mathf.Lerp(movement.x, 0, pawn.data.movementInfo.AccelRate * Time.deltaTime);
            }
            else if(walk[0] == 1)
            {
                movement.x = Mathf.Lerp(movement.x, -pawn.data.movementInfo.GroundSpeed * Time.deltaTime, pawn.data.movementInfo.AccelRate * Time.deltaTime);
            }
            else if (walk[1] == 1)
            {
                movement.x = Mathf.Lerp(movement.x, pawn.data.movementInfo.GroundSpeed * Time.deltaTime, pawn.data.movementInfo.AccelRate * Time.deltaTime);
            }
            if (Mathf.Abs(movement.x) > 0.1f)
            {
                GetComponent<PawnCostume>().MeshSetValue("Run", running);
                GetComponent<PawnCostume>().MeshSetValue("Speed", Mathf.Abs(movement.x) / 0.5f);
            }

            if (pawnMovement.characterController.isGrounded)
            {
                jumping = false;
            }
            else
            {
                jumping = true;
            }

            GetComponent<PawnCostume>().MeshSetValue("Jump", jumping);
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
            }
        }
    }
}
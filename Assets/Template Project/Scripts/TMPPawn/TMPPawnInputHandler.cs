using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;
using UnityEngine.InputSystem;

namespace TemplateGame
{
    public class TMPPawnInputHandler : PawnInputHandler
    {
        TMPPawnMovement pawnMovement;
        public Vector3 movement = new Vector3(0, 0, 0);
        public byte[] walk = {0,0};
        bool running = false;
        bool jumping = false;
        float speed = 0f;

        public void MoveLeft(InputAction.CallbackContext ctx)
        {
            running = true;
            walk[0] = 1;
            GetComponent<PawnCostume>().MeshSetValue("Run", running);
            gameObject.transform.Find("Costume(Clone)").eulerAngles = new Vector3(0, 180, 0);
        }
        public void MoveLeft_Cancel(InputAction.CallbackContext ctx)
        {
            walk[0] = 0;
        }
        public void MoveRight(InputAction.CallbackContext ctx)
        {
            running = true;
            walk[1] = 1;
            GetComponent<PawnCostume>().MeshSetValue("Run", running);
            pawn.transform.Find("Costume(Clone)").eulerAngles = new Vector3(0, 0, 0);
        }
        public void MoveRight_Cancel(InputAction.CallbackContext ctx)
        {
            walk[1] = 0;
        }

        public void Jump(InputAction.CallbackContext ctx)
        {
            if (pawnMovement.characterController.isGrounded)
            {
                pawnMovement.velocity.y = pawn.data.movementInfo.JumpZ;
                pawnMovement.jumping = true;
            }
        }

        public void Look(InputAction.CallbackContext ctx)
        {
            pawn.transform.Find("Costume(Clone)").Find("Mesh").Find("Rig 1").Find("HeadRig").Find("Cursor").transform.position = new Vector3(ctx.ReadValue<Vector2>().x, ctx.ReadValue<Vector2>().y,0);
        }

        private void Update()
        {
            speed = Mathf.Lerp(speed, Mathf.Abs(movement.x)/pawn.data.movementInfo.GroundSpeed, 0.2f);
            GetComponent<PawnCostume>().MeshSetValue("Speed", speed);
            if (((int)walk[0]+(int)walk[1]) == 0)
            {
                running = false;
                movement.x = Mathf.Lerp(movement.x, 0, pawn.data.movementInfo.AccelRate);
            }
            else if(walk[0] == 1)
            {
                movement.x = Mathf.Lerp(movement.x, -pawn.data.movementInfo.GroundSpeed, pawn.data.movementInfo.AccelRate * Time.deltaTime);
            }
            else if (walk[1] == 1)
            {
                movement.x = Mathf.Lerp(movement.x, pawn.data.movementInfo.GroundSpeed, pawn.data.movementInfo.AccelRate * Time.deltaTime);
            }
            if (Mathf.Abs(movement.x) > 0.1f)
            {
                GetComponent<PawnCostume>().MeshSetValue("Run", running);
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
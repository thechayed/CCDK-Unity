using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;

namespace TemplateGame
{
    public class TMPPawnInputHandler : PawnInputHandler
    {
        TMPPawnMovement pawnMovement;

        public override void Start()
        {
            pawnMovement = (TMPPawnMovement) GetComponent<PawnMovement>();
            base.Start();
        }
        public void MoveLeft()
        {
            pawnMovement.characterController.SimpleMove(new Vector3(-1, 0, 0));
        }
    }
}
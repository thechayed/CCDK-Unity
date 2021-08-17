using System.Collections;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class PawnMovement : PawnClass
    {
        public CCDKObjects.Pawn.Movement movement = new CCDKObjects.Pawn.Movement();
        public Vector3 velocity = new Vector3(0,0,0);
        public float dt;

        public override void Start()
        {
            base.Start();
            movement = pawn.data.movementInfo;
        }
        private void Update()
        {
            dt = Time.deltaTime;
        }
    }
}
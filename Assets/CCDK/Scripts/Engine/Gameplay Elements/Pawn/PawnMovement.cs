using System.Collections;
using UnityEngine;
using CCDKEngine;
using System;

namespace CCDKGame
{
    public class PawnMovement : PawnClass
    {
        public CCDKObjects.Pawn.Movement movement = new CCDKObjects.Pawn.Movement();
        public Vector3 velocity = new Vector3(0,0,0);
        public float dt;

        public CharacterController characterController;

        public override void Start()
        {
            base.Start();
            movement = pawn.data.movementInfo;
        }
        private void Update()
        {
            dt = Time.deltaTime;
        }

        /** Performs basic set up of Controllers for Pawn Movement **/
        public object GenerateController(Type type)
        {
            if (type == typeof(CharacterController))
            {
                characterController = gameObject.AddComponent<CharacterController>();
            }
        }
    }
}
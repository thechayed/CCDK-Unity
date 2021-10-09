using System.Collections;
using UnityEngine;
using CCDKEngine;
using System;

namespace CCDKGame
{
    public class PawnMovement : PawnClass
    {
        [HideInInspector] public CCDKObjects.Pawn.Movement movement;

        public Vector3 velocity = new Vector3(0,0,0);
        public float dt;

        [HideInInspector] public CharacterController characterController;

        public void Start()
        {
            movement = pawn.pawnData.movementInfo;
            characterController = pawn.gameObject.AddComponent<CharacterController>();
        }
        private void Update()
        {
            dt = Time.deltaTime;
        }
    }
}
using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;
using System;

public class TMPPawnMovement : PawnMovement
{
    public CharacterController characterController;

    public void State_Default_Enter()
    {
        gameObject.AddComponent<CharacterController>();
        characterController = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    public void State_Default()
    {
        if (characterController != null)
        {
            CalcVelocity();
            characterController.Move(velocity);
        }
    }

    public void CalcVelocity()
    {
        if (!characterController.isGrounded)
        {
            velocity.y -= pawn.data.movementInfo.fallAccelRate*dt;
        }
        else
        {
            velocity.y = 0;
        }
    }
}
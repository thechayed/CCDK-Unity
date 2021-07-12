using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;


public class TMPPawnMovement : PawnMovement
{
    public CharacterController characterController;

    public void State_Default_Enter()
    {
        pawn.AddComponent("CharacterController");
        characterController = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    public void State_Default()
    {
        Debug.Log("Pawn is being updated");
        if (characterController != null)
        {
            characterController.Move(new Vector3(0, 0, 1));
        }
    }
}
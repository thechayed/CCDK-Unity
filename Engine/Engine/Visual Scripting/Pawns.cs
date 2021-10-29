using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;
using UnityEngine.Events;
using System;

namespace CCDKVisualScripting
{
    public class Pawns : MonoBehaviour
    {
        public static UnityEvent pawnSpawned = new UnityEvent();

        /** Spawn a Pawn at a certain location **/
        public static Pawn SpawnPawn(CCDKObjects.Pawn pawnData, Transform spawnTransform)
        {
            pawnSpawned.Invoke();
            return PawnManager.CreatePawn(pawnData, spawnTransform);
        }

        /** Tell the Controller to attempt to possess the pawn. Return a boolean telling whether it was successful or not **/
        public static bool Possess(Pawn pawn, Controller controller)
        {
            return controller.Possess(pawn);
        }

        public static GameObject GetPawn(string pawnName)
        {
            return Engine.GetPawn(pawnName);
        }
    }
}
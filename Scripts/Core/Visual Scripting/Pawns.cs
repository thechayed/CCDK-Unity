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
        public static Pawn SpawnPawn(CCDKObjects.Pawn pawnData, Vector3 position)
        {
            GameObject pawnObject = Objects.CreateObject();
            if(Type.GetType(pawnData.pawnClass) == null)
            {
                pawnData.pawnClass = "CCDKGame.Pawn";
            }
            Pawn pawn = (Pawn)pawnObject.GetComponent(Objects.AddComponentByName(pawnObject, pawnData.pawnClass));
            pawnObject.transform.position = position;
            pawn.data = pawnData;
            pawn.PawnConstructer(pawn.GetDefaultClasses());
            pawnObject.name = pawnData.baseInfo.pawnName;
            pawnSpawned.Invoke();
            return pawn;
        }

        /** Tell the Controller to attempt to possess the pawn. Return a boolean telling whether it was successful or not **/
        public static bool Possess(Pawn pawn, Controller controller)
        {
            return controller.Possess(pawn);
        }
    }
}
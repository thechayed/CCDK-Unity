using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;

namespace CCDKVisualScripting
{
    public class Pawns : MonoBehaviour
    {
        /** Spawn a Pawn at a certain location **/
        public static Pawn SpawnPawn(string pawnClass, Vector3 position)
        {
            GameObject pawnObject = Objects.CreateObject();
            Pawn pawn = (Pawn)pawnObject.GetComponent(Objects.AddComponentByName(pawnObject, pawnClass));
            pawnObject.transform.position = position;
            pawn.PawnConstructer(pawn.GetDefaultClasses());
            pawnObject.name = pawnClass;
            return pawn;
        }


    }
}
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using CCDKGame;
using System;

namespace CCDKEngine
{
    public class PawnManager
    {

        /** A dictionary that stores the defaults for Pawn Classes. The Key is the original class, and Value is the new one (Which are always the same in this context) **/
        public static Dictionary<Script.PawnClass> classDefaults =
            new Dictionary<Script.PawnClass>
            (
                new List<DictionaryItem<Script.PawnClass>>
                {
                    new DictionaryItem<Script.PawnClass>("Input Handler", new Script.PawnClass{script = "CCDKGame.PawnInputHandler"}),
                    new DictionaryItem<Script.PawnClass>("Movement", new Script.PawnClass{script = "CCDKGame.PawnMovement"}),
                    new DictionaryItem<Script.PawnClass>("Life", new Script.PawnClass{script = "CCDKGame.PawnLife"}),
                    new DictionaryItem<Script.PawnClass>("Audio", new Script.PawnClass{script = "CCDKGame.PawnAudio"}),
                    new DictionaryItem<Script.PawnClass>("Inventory Manager", new Script.PawnClass{script = "CCDKGame.PawnInventoryManager"}),
                    new DictionaryItem<Script.PawnClass>("Costume", new Script.PawnClass{script = "CCDKGame.PawnCostume"})
                }
            );

        /**<summary>A list of all the Pawns that currently exist in the game.</summary>**/
        public static List<Pawn> PawnsInGame = new List<Pawn>();

        /**<summary>Create a pawn in the current Level</summary> **/
        public static Pawn CreatePawn(CCDKObjects.Pawn data, Vector3 position = default(Vector3))
        {
            GameObject pawnObject = GameObject.Instantiate(data.prefab);
            pawnObject.name = data.prefab.name;
            Pawn pawn = pawnObject.GetComponent<Pawn>();
            pawnObject.transform.position = position;
            pawn.data = data;
            pawn.pawnData = data;
            return pawn;
        }

        public static Transform CreatePawnAndGetRoot(CCDKObjects.Pawn data, Vector3 position = default(Vector3))
        {
            return CreatePawn(data, position).transform.root;
        }

        public static void GetPawn(Pawn pawn)
        {
            PawnsInGame.Add(pawn);
        }

        public static void RemovePawn(Pawn pawn)
        {
            PawnsInGame.Remove(pawn);
        }
    }
}
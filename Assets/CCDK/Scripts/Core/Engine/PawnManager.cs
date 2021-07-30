using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace CCDKEngine
{
    class PawnManager
    {

        /** A dictionary that stores the defaults for Pawn Classes. The Key is the original class, and Value is the new one (Which are always the same in this context) **/
        public static Dictionary<string> classDefaults =
            new Dictionary<string>
            (
                new List<DictionaryItem<string>>
                {
                    new DictionaryItem<string>("Movement", "CCDKGame.PawnMovement"),
                    new DictionaryItem<string>("Life", "CCDKGame.PawnLife"),
                    new DictionaryItem<string>("Audio", "CCDKGame.PawnAudio"),
                    new DictionaryItem<string>("InventoryManager", "CCDKGame.PawnInventoryManager"),
                    new DictionaryItem<string>("Costume", "CCDKGame.PawnCosume"),
                    new DictionaryItem<string>("InputHandler", "CCDKGame.PawnInputHandler")
                }
            );
    }
}
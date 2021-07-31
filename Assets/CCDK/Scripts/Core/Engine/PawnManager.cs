using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace CCDKEngine
{
    class PawnManager
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
    }
}
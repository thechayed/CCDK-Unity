using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace CCDKEngine
{
    class PawnManager
    {

        /** A dictionary that stores the defaults for Pawn Classes. The Key is the original class, and Value is the new one (Which are always the same in this context) **/
        public static StringsDictionary classDefaults =
            new StringsDictionary
            (
                new List<StringsDictionaryItem>
                {
                    new StringsDictionaryItem("PawnMovement", "CCDKGame.PawnMovement"),
                    new StringsDictionaryItem("PawnInput", "CCDKGame.PawnLife"),
                    new StringsDictionaryItem("PawnAudio", "CCDKGame.PawnAudio"),
                    new StringsDictionaryItem("PawnInventoryManager", "CCDKGame.PawnInventoryManager")
                }
            );
    }
}
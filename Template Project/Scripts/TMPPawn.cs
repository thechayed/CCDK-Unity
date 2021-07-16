using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;
using System.Collections.Generic;

public class TMPPawn : Pawn
{
    public override StringsDictionary GetDefaultClasses()
    {
        StringsDictionary defaultDic =
            new StringsDictionary
            (
                    new List<StringsDictionaryItem>
                    {
                    new StringsDictionaryItem("PawnMovement", "TMPPawnMovement"),
                    new StringsDictionaryItem("PawnLife", "CCDKGame.PawnLife"),
                    new StringsDictionaryItem("PawnAudio", "CCDKGame.PawnAudio"),
                    new StringsDictionaryItem("PawnInventoryManager", "CCDKGame.PawnInventoryManager")
                    }
            );

        return defaultDic;
    }
}
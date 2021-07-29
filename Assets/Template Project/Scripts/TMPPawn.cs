using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;
using System.Collections.Generic;

namespace TemplateGame
{
    public class TMPPawn : Pawn
    {
        public override Dictionary<string> GetDefaultClasses()
        {
            Dictionary<string> defaultDic =
                new Dictionary<string>
                (
                        new List<DictionaryItem<string>>
                        {
                            new DictionaryItem<string>("PawnMovement", "TMPPawnMovement"),
                            new DictionaryItem<string>("PawnLife", "CCDKGame.PawnLife"),
                            new DictionaryItem<string>("PawnAudio", "CCDKGame.PawnAudio"),
                            new DictionaryItem<string>("PawnInventoryManager", "CCDKGame.PawnInventoryManager")
                        }
                );

            return defaultDic;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class PawnInventoryManager : PawnClass
    {
        /** Attempt giving an item to the Pawn and return whether it was successful or not **/
        public bool Give(InventoryItem item)
        {
            foreach(string tag in pawn.pawnData.inventoryInfo.inventoryTags)
            {
                if(item.typeTag == tag)
                {
                    pawn.pawnData.inventoryInfo.inventory.Add(item);
                    return true;
                }
            }
            return false;
        }

        /** Throw an Item out into the world, removing it from the Pawn's inventory **/
        public void Throw(InventoryItem item)
        {
            pawn.pawnData.inventoryInfo.inventory.Remove(item);
            item.Thrown();
        }
    }
}
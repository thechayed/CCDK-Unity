using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class PawnInventoryManager : PawnClass
    {
        /** Attempt giving an item to the Pawn and return whether it was successful or not **/
        public bool Give(CCDKObjects.InventoryItem item)
        {
            foreach(string tag in pawn.pawnData.inventoryInfo.inventoryTags)
            {
                if(item.typeTag == tag)
                {
                    pawn.inventory.Add(item);
                    return true;
                }
            }
            return false;
        }

        /** Throw an Item out into the world, removing it from the Pawn's inventory **/
        public void Throw(CCDKObjects.InventoryItem item)
        {
            pawn.inventory.Remove(item);
            /**Don't do this! Replace with Pawn throwing interaction!**/
            item.instance.Thrown();
        }
    }
}
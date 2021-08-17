using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class PawnInventoryManager : PawnClass
    {
        public override void Start()
        {
            base.Start();
        }


        /** Attempt giving an item to the Pawn and return whether it was successful or not **/
        public bool Give(InventoryItem item)
        {
            foreach(string tag in pawn.data.inventoryInfo.inventoryTags)
            {
                if(item.typeTag == tag)
                {
                    pawn.data.inventoryInfo.inventory.Add(item);
                    return true;
                }
            }
            return false;
        }

        /** Throw an Item out into the world, removing it from the Pawn's inventory **/
        public void Throw(InventoryItem item)
        {
            pawn.data.inventoryInfo.inventory.Remove(item);
            item.Thrown();
        }
    }
}
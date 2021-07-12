using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class PawnInventoryManager : PawnClass
    {
        /** A list of all the Items currently in the Pawn's inventory **/
        public List<InventoryItem> inventory;

        /** A list defining what kings of Items a Pawn can hold **/
        public List<string> inventoryTags;


        /** Attempt giving an item to the Pawn and return whether it was successful or not **/
        public bool Give(InventoryItem item)
        {
            foreach(string tag in inventoryTags)
            {
                if(item.typeTag == tag)
                {
                    inventory.Add(item);
                    return true;
                }
            }
            return false;
        }

        /** Throw an Item out into the world, removing it from the Pawn's inventory **/
        public void Throw(InventoryItem item)
        {
            inventory.Remove(item);
            item.Thrown();
        }
    }
}
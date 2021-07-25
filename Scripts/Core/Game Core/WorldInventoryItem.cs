using System.Collections;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class WorldInventoryItem : InventoryItem
    {
        /** When the item is sent to the world, decide what to do with it here **/
        public override void State_EnterWorld()
        {


            /** Go to World State **/
            base.State_EnterWorld();
        }
    }

}
using System.Collections;
using UnityEngine;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Game Objects/Item")]
    public class InventoryItem : PrefabSO
    {

        /** The Item Type tag let's the Pawn know whether the item can be recieved by it or not **/
        protected string typeTag = "Default";

        /** How much should the pickup of this item be prioritized **/
        protected float Priority;

        /** Whether the player can throw this Item **/
        protected bool isThrowable;

    }
}
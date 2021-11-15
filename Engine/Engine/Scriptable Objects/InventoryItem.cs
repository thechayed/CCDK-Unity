using System.Collections;
using UnityEngine;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Game Objects/Item")]
    public class InventoryItem : PrefabSO
    {

        public CCDKGame.InventoryItem instance;

        /** The Item Type tag let's the Pawn know whether the item can be recieved by it or not **/
        public string typeTag = "Default";

        /** How much should the pickup of this item be prioritized **/
        public float Priority;

        /** Whether the player can throw this Item **/
        public bool isThrowable;

#if UNITY_EDITOR
        public override void OnEnable()
        {
            className = "CCDKGame.InventoryItem";
            base.OnEnable();
        }
#endif
    }
}
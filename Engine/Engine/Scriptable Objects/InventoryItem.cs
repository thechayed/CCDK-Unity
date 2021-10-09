using System.Collections;
using UnityEngine;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Game Objects/Item")]
    public class InventoryItem : ScriptableObject
    {

        /** The Item Type tag let's the Pawn know whether the item can be recieved by it or not **/
        protected string typeTag = "Default";

        /** How much should the pickup of this item be prioritized **/
        protected float Priority;

        /** The Mesh to apply to the weapon **/
        protected Mesh mesh;

        /** The Collider for the Item in the world **/
        protected Collider col;

        /** the reference to the GameObject that belongs to this Item whenever it's added to the world **/
        protected GameObject worldObject;

        /** Whether the player can throw this Item **/
        protected bool isThrowable;

    }
}
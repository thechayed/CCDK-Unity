using System.Collections;
using UnityEngine;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Gameplay/Item")]
    public class InventoryItem : ScriptableObject
    {

        /** The Item Type tag let's the Pawn know whether the item can be recieved by it or not **/
        public string typeTag = "Default";

        /** How much should the pickup of this item be prioritized **/
        public float Priority;

        /** The Mesh to apply to the weapon **/
        public Mesh mesh;

        /** The Collider for the Item in the world **/
        public Collider col;

        /** the reference to the GameObject that belongs to this Item whenever it's added to the world **/
        public GameObject worldObject;

        /** Whether the player can throw this Item **/
        public bool isThrowable;

    }
}
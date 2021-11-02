/* An Inventory Item stores basic information for the Data and Display of any item that
 * the player can pickup and use. */

using System.Collections;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class InventoryItem : CCDKEngine.Object
    {
        [Header(" - Iventory Item - ")]
        /** The Item Type tag let's the Pawn know whether the item can be recieved by it or not **/
        public string typeTag = "Default";

        /** How much should the pickup of this item be prioritized **/
        public float Priority;

        /** the reference to the GameObject that belongs to this Item whenever it's added to the world **/
        public GameObject worldObject;

        /** Whether the player can throw this Item **/
        public bool isThrowable;

        /** What to do with the Item when it's thrown **/
        public virtual void Thrown()
        {
            //state.GotoState("EnterWorld");
        }

        /** Careate the mesh and collider objets **/
        public void Draw()
        {
            MeshFilter meshComponent = gameObject.AddComponent<MeshFilter>();
        }
    }

}
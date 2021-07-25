/* An Inventory Item stores basic information for the Data and Display of any item that
 * the player can pickup and use. */

using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    public class InventoryItem : Object
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

        /** The array of states that the Inventory Item can have **/
        public string[] states = { "EnterWorld", "World", "Equipped", "Stashed" };
        public override void Start()
        {
            base.Start();
            /** Set Object state so that it can call relative update functions **/
            state.States = states;
        }

        /** When the item is sent to the world, decide what to do with it here **/
        public virtual void State_EnterWorld()
        {
            //Other functionality here
            state.GotoState("World");
        }

        /** When the InventoryItem exists in the world, extend this function
         * to determine how it is picked up and/or used in the world**/
        public virtual void State_World()
        {

        }

        /** When the player has this item equipped **/
        public virtual void State_Equipped()
        {

        }

        /** When the player has this item stored in the dictionary **/
        public virtual void State_Stashed()
        {

        }

        /** What to do with the Item when it's thrown **/
        public virtual void Thrown()
        {
            state.GotoState("EnterWorld");
        }

        /** Careate the mesh and collider objets **/
        public void Draw()
        {
            MeshFilter meshComponent = gameObject.AddComponent<MeshFilter>();
            meshComponent.mesh = mesh;
            col = gameObject.AddComponent<Collider>();
        }
    }

}
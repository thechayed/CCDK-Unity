using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class Weapon : InventoryItem
    {
        /** How long does it take to Equip this weapon */
        float EquipTime;

        /** How long does it take to put this weapon down */
        float PutDownTime;

        [SerializeField]
        /**The list of Fire types for this weapon**/
        public List<WeaponFireType> fireTypes;


        /** When the item is sent to the world, decide what to do with it here **/
        public override void State_EnterWorld()
        {
            //Other functionality here
            state.GotoState("World");
        }

        /** When the InventoryItem exists in the world, extend this function
         * to determine how it is picked up and/or used in the world**/
        public override void State_World()
        {

        }

        /** When the player has this item equipped **/
        public override void State_Equipped()
        {

        }

        /** When the player has this item stored in the dictionary **/
        public override void State_Stashed()
        {

        }

    }
}
using System.Collections;
using UnityEngine;
using CCDKGame;
using System.Collections.Generic;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Game Objects/Weapon")]
    public class Weapon : InventoryItem
    {
        /** How long does it take to Equip this weapon */
        float EquipTime;

        /** How long does it take to put this weapon down */
        float PutDownTime;

        [SerializeField]
        /**The list of Fire types for this weapon**/
        public List<WeaponFireType> fireTypes;

    }
}
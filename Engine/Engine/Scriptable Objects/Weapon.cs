using System.Collections;
using UnityEngine;
using CCDKGame;
using System.Collections.Generic;
using B83.Unity.Attributes;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Game Objects/Weapon")]
    public class Weapon : InventoryItem
    {
        [MonoScript(type = typeof(CCDKGame.Weapon))] public string weaponClass = "CCDKGame.Weapon";

        [Tooltip("A list of the different Fire Types, with their projectile Data and Shooting information.")]
        /**The list of Fire types for this weapon**/
        public List<WeaponFireType> fireTypes;

        /** How long does it take to Equip this weapon */
        float EquipTime;

        /**If the Weapon is set to autoattach, it will attach itself to the pawn when it is spawned**/
        public bool autoAttach = true;

        /** How long does it take to put this weapon down */
        float PutDownTime;



#if UNITY_EDITOR
        public override void OnEnable()
        {
            className = weaponClass;
            defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/Object");
            base.OnEnable();
        }

        public override void Awake()
        {
            className = weaponClass;
            defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/Object");
            base.Awake();
        }

        public override void OnValidate()
        {
            className = weaponClass;
            base.OnValidate();
        }
#endif
    }


    [System.Serializable]
    public class WeaponFireType
    {
        /**Fire Type determines how the weapon fires, whether it be an instant hit, 
            * shoot a projectile, or have a custom type of fire, or none at all**/
        public enum EWeaponFireType
        {
            InstantHit,
            Projectile,
            Custom,
            None
        };
        public EWeaponFireType fireType;

        /**The projectile class to be spawned**/
        public Projectile projectile;

        /** Holds the amount of time a single shot takes */
        public float fireInterval = 1f;

        /** How much of a spread between shots */
        public float spread;

        /** How much damage does a given instanthit shot do */
        public float InstantHitDamage;

        /** momentum transfer scaling for instant hit damage */
        public float InstantHitMomentum;

        /** DamageTypes for Instant Hit Weapons */
        public DamageType InstantHitDamageTypes;

        /** Holds an offest for spawning protectile effects. */
        public Vector3 FireOffset;

        /** Range of Weapon, used for Traces (InstantFire, ProjectileFire, AdjustAim...) */
        public float range;

        public WeaponFireType(Projectile projectile, float fireInterval, float spread)
        {
            this.projectile = projectile;
            this.fireInterval = fireInterval;
            this.spread = spread;
        }
    }

}
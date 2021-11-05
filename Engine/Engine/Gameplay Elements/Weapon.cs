using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class Weapon : InventoryItem
    {
        [Header(" - Weapon - ")]
        public CCDKObjects.Weapon weaponData;
        public Pawn pawn;
        public override void Start()
        {
            base.Start();
            weaponData = (CCDKObjects.Weapon)data;
        }

        public override void Update()
        {
            //if(weaponData.autoAttach)
            //{
            //    transform.position = pawnTransform.position;
            //    transform.rotation = pawnTransform.rotation;
            //}
        }
        public void Fire(Transform direction, int fireType = 0)
        {
            GameObject projectileObject = Instantiate(weaponData.projectiles[fireType].prefab);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.pawn = pawn;
            projectile.projectileData = weaponData.projectiles[fireType];

            projectileObject.transform.position = direction.position;
            projectileObject.transform.rotation = direction.rotation;
        }
    }
}
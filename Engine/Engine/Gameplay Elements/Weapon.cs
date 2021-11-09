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
        public float lastShotTime = 0f;

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
        public void Fire(Transform direction = default, int fireType = 0)
        {
            if ((life - lastShotTime) < weaponData.fireTypes[fireType].fireInterval)
                return;
            
            GameObject projectileObject = Instantiate(weaponData.fireTypes[fireType].projectile.prefab);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.pawn = pawn;
            projectile.projectileData = weaponData.fireTypes[fireType].projectile;

            projectileObject.transform.position = direction.position;
            projectileObject.transform.rotation = direction.rotation;
            lastShotTime = life;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class Projectile : CCDKEngine.Object
    {
        // Motion information.
        /** Initial speed of projectile. */
        public float Speed;
        /** Limit on speed of projectile (0 means no limit). */
        public float MaxSpeed;
        /**The angle of motion of the projectile**/
        public Vector3 movingAngle;

        // Damage parameters
        /** Damage done by the projectile */
        public float Damage;
        /** Radius of effect in which damage is applied. */
        public float DamageRadius;
        /** Momentum magnitude imparted by impacting projectile. */
        public float MomentumTransfer;

        /** The object we collided with**/
        public GameObject ImapactedObject;


        public float 
    }

}
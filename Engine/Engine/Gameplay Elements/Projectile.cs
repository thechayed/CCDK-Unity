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
        /**The angle of motion of the projectile**/
        public Vector3 movingAngle;



        /** The object we collided with**/
        public GameObject ImapactedObject;

    }

}
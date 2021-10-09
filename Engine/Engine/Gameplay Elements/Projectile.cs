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
        
        /**The target Transform of the object the parent was aiming at when the projectile was shot.**/
        public Transform target;
        
        public Pawn parent;


        /** The object we collided with**/
        public GameObject ImapactedObject;
        
        public void Spawn(CCDKObjects.Projectile data, Transform direction, Transform target, Pawn parent = null)

    }

}
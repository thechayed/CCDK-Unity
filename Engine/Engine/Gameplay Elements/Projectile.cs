using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class Projectile : CCDKEngine.Object
    {
     
        public CCDKObjects.Projectile projectileData;
      
        // Motion information.
        /** Initial speed of projectile. */
        public float speed;
        /**The angle of motion of the projectile**/
        public Vector3 movingAngle;

        
        /**The target Transform of the object the parent was aiming at when the projectile was shot.**/
        public Transform target;
        
        public Pawn pawn;

        /** The object we collided with**/
        public GameObject ImpactedObject;
        
        public override void Start()
        {
            base.Start();
            speed = projectileData.initialSpeed;
        }

        //public void Spawn(CCDKObjects.Projectile data, Transform direction, Transform target, Pawn pawn = null)
        //{
        //  this.data = data;
        //  this.target = target;
        //  this.pawn = pawn;

        //  transform.position = direction.position;
        //  transform.eulerAngles = direction.eulerAngles;

        //  projectileData = (CCDKObjects.Projectile) data;
        //}

        public override void Update()
        {
          /**Update the position of the projectile by its speed**/
          transform.position += (transform.forward*Time.deltaTime*speed);
          
          /**If using acceleration, interpolation to the Goal speed**/
          if(projectileData.accelerate)
          {
                speed = Mathf.Lerp(speed, projectileData.maxSpeed, projectileData.acceleration);
          }

            if (life > projectileData.lifeSpan)
                Destroy();
        }

    }

}
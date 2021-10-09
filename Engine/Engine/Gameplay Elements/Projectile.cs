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
        
        public Pawn parent;


        /** The object we collided with**/
        public GameObject ImapactedObject;
        
        public void Spawn(CCDKObjects.Projectile data, Transform direction, Transform target, Pawn parent = null)
        {
          this.data = data;
          this.target = target;
          this.parent = parent;
          
          transform.position = direction.position;
          transform.eulerAngles = direction.eulerAngles;
          
          projectileData = (CCDKObjects.Projectile) data;
        }
        
        private void Update()
        {
          /**Update the position of the projectile by its speed**/
          transform.position += (transform.etransform.forward*Time.deltaTime*speed);
          
          if(projectileData.accelerate)
          {
            speed = Marhf.Lerp(speed, projectileData.maxSpeed, projectileData.acceleration)
          }
        }

    }

}
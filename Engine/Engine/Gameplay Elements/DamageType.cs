/*Extend this script to make new types of damage, 
 * this class provides damage information for anything that can cause damage to an object*/
using System.Collections;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class DamageType
    {
        public bool causedByWorld;

        /** These values are used to modify the way the damage affects physics **/
        [SerializeField]
        public struct Physics
        {
            public float KDamageImpulse,              // magnitude of impulse applied to KActor due to this damage type.
                         KDeathVel,                    // How fast ragdoll moves upon death
                         KDeathUpKick;              // Amount of upwards kick ragdolls get when they die
        }
        public Physics physics =
        new Physics
        {
            KDamageImpulse = 800,
        };

        /**These values are used by Vehicle based Damage**/
        [SerializeField]
        public struct Vehicle
        {
            /** multiply damage by this for vehicles */
            public float VehicleDamageScaling,
                         /** multiply momentum by this for vehicles */
                         VehicleMomentumScaling;
        }
        public Vehicle vehicle =
        new Vehicle
        {
            VehicleDamageScaling = 1.0f,
            VehicleMomentumScaling = 1.0f
        };
    }
}
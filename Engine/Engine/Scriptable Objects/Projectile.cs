using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using CCDKEngine;
using UnityEngine.InputSystem;
using System;
using UnityEditor;
using B83.Unity.Attributes;
using CCDKGame;
using System.Threading.Tasks;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Game Objects/Projectile")]
    public class Projectile : PrefabSO
    {
        [MonoScript(type = typeof(CCDKGame.Projectile))] public string projectileClass = "CCDKGame.Projectile";

        [Header(" - Flags - ")]
        [Tooltip("Whether the projectile is physical or not/ moves forward from the point it was shot, over time.")]
        public bool physical = true;
        [Tooltip("Whether this projectile should accelerate toward the maxSpeed (slow down or speed up after being shot)")]
        public bool accelerate = false;
        [Tooltip("Use physics will enable gravity and wind influence.")]
        public bool usePhysics = true;
        [Tooltip("If the projectile uses physics, enables the influence of wind.")]
        public bool useWind = false;
        
        
        public bool tracking;
        public float baseTrackIntensity=0.01f;
        public float trackIntensityOnMoveMultiplier=0.1f;


        [Header(" - Base Properties - ")]
        [Tooltip("The amount of time that must pass before this Projectile is destroyed.")]
        public float lifeSpan = 0.5f;
        public float initialSpeed = 5f;
        public float maxSpeed;
        public float acceleration = 0.01f;
        [Tooltip("The length of the ray that is shot from the Projectile toward its moving angle")]
        public float rayLength = 1.0f;

        [Header(" - Damage Properties - ")]
        /** Damage done by the projectile */
        public float Damage;
        /** Radius of effect in which damage is applied. */
        public float DamageRadius;
        /** Momentum magnitude imparted by impacting projectile. */
        public float MomentumTransfer;


        [Header(" - Physics Properties - ")]
        public float gravity;


#if UNITY_EDITOR
        public override void OnEnable()
        {
            className = projectileClass;
            defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/Object");
            base.OnEnable();
        }

        public override void Awake()
        {
            className = projectileClass;
            defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/Object");
            base.Awake();
        }

        public override void OnValidate()
        {
            className = projectileClass;
            base.OnValidate();
        }
#endif
    }

}
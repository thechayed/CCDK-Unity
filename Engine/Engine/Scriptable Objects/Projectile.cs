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
        [Tooltip("Use physics will enable gravity and wind influence.")]
        public bool usePhysics = true;
        [Tooltip("If the projectile uses physics, enables the influence of wind.")]
        public bool useWind = false;
        
        [hHeader(" - Base Properties - ")]
        [Tooltip("The length of the ray that is shot from the Projectile toward its moving angle")]
        public float rayLength = 1.0f;

        [Header(" - Physics Properties - ")]
        public float gravity;
        public float speed;

        public override void OnEnable()
        {
            className = projectileClass;
            defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/Object");
            base.OnEnable();
        }

        public override void Awake()
        {
            className = pprojectileClass;
            defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/Object");
            base.Awake();
        }

        public override void OnValidate()
        {
            className = controllerClass;
            base.OnValidate();
        }
    }
}
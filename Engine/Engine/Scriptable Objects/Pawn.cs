using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using B83.Unity.Attributes;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.IO;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCDKObjects
{


    [CreateAssetMenu(menuName = "Game Objects/Pawn")]
    public class Pawn : PrefabSO
    {
        private GameObject gameObjectPrefab;
        /** The C# Pawn class **/
        [MonoScript(type = typeof(CCDKGame.Pawn))] public string pawnClass = "CCDKGame.Pawn";

        public bool possessable = true;

        public bool NavMeshAgent = true;
        public bool NavMeshAgentMove = false;

#if UNITY_EDITOR

        public override void OnEnable()
        {
            className = pawnClass;
            defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/Pawn");
            base.OnEnable();
        }

        public override void Awake()
        {
            className = pawnClass;
            defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/Pawn");
            base.Awake();
        }

        public override void OnValidate()
        {

            className = pawnClass;
            base.OnValidate();
        }
#endif

        [System.Serializable]
        public class Audio 
        {
            public Dictionary<AudioClip> audioClips;
        }
        public Audio audioInfo;

        [System.Serializable]
        public class Inventory 
        {
            /** How many weapons the player can hold **/
            public int weaponSlots = 1;

            /** List of InventoryItems the Pawn should start with **/
            public List<CCDKObjects.InventoryItem> inventory;

            /** A list defining what kings of Items a Pawn can hold **/
            public List<string> inventoryTags;
        }
        public Inventory inventoryInfo;


        [System.Serializable]
        public class Movement 
        {
            public float MaxStepHeight,
            MaxJumpHeight,
            WalkableFloorZ;
            public bool bRunPhysicsWithNoController,   // When there is no Controller, Walking Physics abort and force a velocity and acceleration of 0. Set this to TRUE to override.
                        bForceMaxAccel;    // ignores Acceleration component, and forces max AccelRate to drive Pawn at full velocity.
            public float GroundSpeed = 250,  // The maximum ground speed.
                         WaterSpeed,       // The maximum swimming speed.
                         AirSpeed,     // The maximum flying speed.
                         LadderSpeed,  // Ladder climbing speed
                         AccelRate,        // max acceleration rate
                         fallAccelRate = 1,
                         JumpZ,            // vertical acceleration w/ jump
                         OutofWaterZ,  /** z velocity applied when pawn tries to get out of water */
                         MaxOutOfWaterStepHeight;  /** Maximum step height for getting out of water */
            public bool bLimitFallAccel; // should acceleration be limited (by a factor of GroundSpeed and AirControl) when in PHYS_Falling?
            public float AirControl,       // amount of AirControl available to the pawn
                         WalkingPct,       // pct. of running speed that walking speed is
                         MovementSpeedModifier, // a modifier that can be used to override the movement speed.
                         CrouchedPct,  // pct. of running speed that crouched walking speed is
                         MaxFallSpeed;  // max speed pawn can land without taking damage
        }
        public Movement movementInfo;

        [System.Serializable]
        public class Input
        {
            /** A dictionary storing all the Input Actions recieved from the Player Controller that the Pawn should respond to. **/
            public Dictionary<string> actions = new Dictionary<string>();

            public Dictionary<UnityEvent<InputAction.CallbackContext>> events = new Dictionary<UnityEvent<InputAction.CallbackContext>>();


        }
        public Input inputinfo;
    }
}
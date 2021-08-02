using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using B83.Unity.Attributes;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Gameplay/Pawn")]
    public class Pawn : ScriptableObject
    {
        /** The C# Pawn class **/
       [MonoScript(type = typeof(CCDKGame.Pawn))]  public string pawnClass = "Pawn"; 

        [System.Serializable]
        public class Default 
        {
            /* This pawn's name */
            public string pawnName = "NoName";

            /** A dictionary storing all the Class names to add to the pawn on Construction **/
            /** The Key stores the name of the original class, and the Value stores the new class **/
            public Dictionary<Script.PawnClass> classes =
                new Dictionary<Script.PawnClass>
                (
                        new List<DictionaryItem<Script.PawnClass>>
                        {
                    new DictionaryItem<Script.PawnClass>("Input Handler", new Script.PawnClass{script = "CCDKGame.PawnInputHandler"}),
                    new DictionaryItem<Script.PawnClass>("Movement", new Script.PawnClass{script = "CCDKGame.PawnMovement"}),
                    new DictionaryItem<Script.PawnClass>("Life", new Script.PawnClass{script = "CCDKGame.PawnLife"}),
                    new DictionaryItem<Script.PawnClass>("Audio", new Script.PawnClass{script = "CCDKGame.PawnAudio"}),
                    new DictionaryItem<Script.PawnClass>("Inventory Manager", new Script.PawnClass{script = "CCDKGame.PawnInventoryManager"}),
                    new DictionaryItem<Script.PawnClass>("Costume", new Script.PawnClass{script = "CCDKGame.PawnCostume"})
                        }
                );

            /** Componenet References **/
            CCDKGame.PawnMovement pawnMovement;
            CCDKGame.PawnInventoryManager pawnInventoryManager;
            CCDKGame.PawnAudio pawnAudio;
            CCDKGame.PawnLife pawnLife;
        }
        public Default baseInfo = new Default();

        [System.Serializable]
        public class Life 
        {
            public float health;
        }
        public Life lifeInfo;
    
        [System.Serializable]
        public class Audio 
        {
            public Dictionary<AudioClip> audioClips;
        }
        public Audio audioInfo;
    
        [System.Serializable]
        public class Costume 
        {
            /** What kind of costume do we got **/
            public enum CostumeEnum
            {
                Mesh,
                Sprite
            }
            public CostumeEnum costumeType;

            /** The Mesh that will be displayed for this Pawn **/
            public Mesh mesh;
            /** The Sprite that will be displayed for this Pawn **/
            public Sprite sprite;

            /** Costume Game Object **/
            public GameObject costumeObject;
        }
        public Costume costumeInfo;

        [System.Serializable]
        public class Inventory 
        {
            /** A list of all the Items currently in the Pawn's inventory **/
            public List<CCDKEngine.InventoryItem> inventory;

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
    }
}
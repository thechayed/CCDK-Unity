using System.Collections;
using UnityEngine;
using System.Collections.Generic;


namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Gameplay/Pawn")]
    public class Pawn : ScriptableObject
    {
        /** The C# Pawn class **/
        public string pawnClass = "Pawn"; 

        [System.Serializable]
        public class Default : StateEnabledObject
        {
            /* This pawn's name */
            public string pawnName = "NoName";

            /** A dictionary storing all the Class names to add to the pawn on Construction **/
            /** The Key stores the name of the original class, and the Value stores the new class **/
            public StringsDictionary classes =
                new StringsDictionary
                (
                        new List<StringsDictionaryItem>
                        {
                    new StringsDictionaryItem("PawnMovement", "CCDKGame.PawnMovement"),
                    new StringsDictionaryItem("PawnLife", "CCDKGame.PawnLife"),
                    new StringsDictionaryItem("PawnAudio", "CCDKGame.PawnAudio"),
                    new StringsDictionaryItem("PawnInventoryManager", "CCDKGame.PawnInventoryManager"),
                    new StringsDictionaryItem("PawnCostume", "CCDKGame.PawnCostume")
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
        public class Life : StateEnabledObject
        {
            public float health;
        }
        public Life lifeInfo;
    
        [System.Serializable]
        public class Audio : StateEnabledObject
        {
            public AudioClipDictionary audioClips;
        }
        public Audio audioInfo;
    
        [System.Serializable]
        public class Costume : StateEnabledObject
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
        public class Inventory : StateEnabledObject
        {
            /** A list of all the Items currently in the Pawn's inventory **/
            public List<CCDKEngine.InventoryItem> inventory;

            /** A list defining what kings of Items a Pawn can hold **/
            public List<string> inventoryTags;
        }
        public Inventory inventoryInfo;


        [System.Serializable]
        public class Movement : StateEnabledObject
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
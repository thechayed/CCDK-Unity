using System.Collections;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class PawnMovement : PawnClass
    {
        public float MaxStepHeight,
                    MaxJumpHeight,
                    WalkableFloorZ;
        public bool bRunPhysicsWithNoController,   // When there is no Controller, Walking Physics abort and force a velocity and acceleration of 0. Set this to TRUE to override.
                    bForceMaxAccel;    // ignores Acceleration component, and forces max AccelRate to drive Pawn at full velocity.
        public float GroundSpeed,  // The maximum ground speed.
                     WaterSpeed,       // The maximum swimming speed.
                     AirSpeed,     // The maximum flying speed.
                     LadderSpeed,  // Ladder climbing speed
                     AccelRate,        // max acceleration rate
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
}
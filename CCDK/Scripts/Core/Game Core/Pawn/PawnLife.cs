using System;
using System.Collections;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class PawnLife : PawnClass
    {
        public delegate void DeadAction(Pawn Killer, Type DamageType, Vector3 HitLocation);
        public event DeadAction Die;

        public override void Start()
        {
            state = pawn.data.lifeInfo.state;
            base.Start();
        }

        /** This function is called when the pawn should die **/
        public void Died(Pawn Killer, Type DamageType, Vector3 HitLocation)
        {

        }
    }
}
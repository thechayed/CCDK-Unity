using System.Collections;
using UnityEngine;
using CCDKGame;

namespace CCDKEngine
{
    [HideInInspector]
    public class PawnClass
    {
        [HideInInspector]
        public Pawn pawn;
        /**This object's Player Controller **/
        [ReadOnly] protected Controller controller;

    }
}
/** Base Class for children components of a Controller **/

using System.Collections;
using UnityEngine;

namespace CCDKEngine
{
    public class ControllerClass : PossessableObject
    {
        /** True if the Controller has possessed a Pawn **/
        public bool possessingPawn;

        /** The Pawn that has been possessed **/
        public CCDKGame.Pawn pawn;

        public virtual void Communicate()
        {
            //Extend to recieve information
        }
    }
}
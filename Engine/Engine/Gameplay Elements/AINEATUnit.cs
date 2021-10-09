/** Extend this class to create unique NEAT AI IO's, then assign it to the AINEATController's data Scriptable Object **/
/** This class does nothing on it's own **/

using System.Collections;
using UnityEngine;
using CCDKEngine;
using SharpNeat.Phenomes;

namespace CCDKGame
{
    public class AINEATUnit : UnitController
    {
        /**This object's Controller **/
        public Controller controller;

        public override void Activate(IBlackBox box)
        {

        }

        public override void Stop()
        {

        }

        public override float GetFitness() 
        {
            return 0f;
        }
    }
}
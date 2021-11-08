/**An Objective is a game Object used for keeping score. **/
using System.Collections;
using UnityEngine;
using CCDKEngine;
using CCDKGame;

namespace CCDKGame
{
    public class Objective : CCDKEngine.Object
    {
        public CCDKObjects.Objective objectiveData;
        /**<summary>The collider to use for collision checks. Used most often with Zones.</summary>**/
        public Collider volume;

        public override void Start()
        {
            base.Start();
            objectiveData = (CCDKObjects.Objective)data;
        }
    }
}
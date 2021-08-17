using System.Collections;
using UnityEngine;
using CCDKEngine;
using System.Collections.Generic;

namespace CCDKDeprecated
{
    public class AIController_Genetic : Controller
    {
        /** This AI's Index in the algorithm pool **/
        public int index;

        /** The Parent Algorithm Manager **/
        public GeneticInputManager<List<float>> manager;
    }
}
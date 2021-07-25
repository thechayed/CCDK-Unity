/** Extend/Add to this to interact with a pool of AI agents based on their Input values **/

using System.Collections;
using UnityEngine;
using CCDKEngine;
using System.Collections.Generic;

namespace CCDKDeprecated
{
    public class GeneticInputManager<T> : GAManager<List<float>>
    {
        public override void Init()
        {
            FitnessFunction = GetFitness;
            GetRandomGene = GetRandomInputs;

            base.Init();
        }

        public List<float> GetRandomInputs()
        {
            return null;
        }

        public float GetFitness(int index)
        {
            return 0f;
        }
    }
}
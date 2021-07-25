/** The Genetic Algorithm Manager is a Class the should be extended in order to interface with the Genetic Algorithm **/
/** The manager stores a pool of DNA, and calls related functions **/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKEngine;

namespace CCDKDeprecated
{
    public class GAManager<T>
    {
        /* Global objects */
        private System.Random random;

        /* Manger Info */
        public GAPool<T> pool;
        public List<GameObject> objectPool;
        public Func<int, float> FitnessFunction;
        public Func<T> GetRandomGene;


        /*Pool Info*/
        int populationSize;
        int dnaSize;
        float mutationRate = 0.01f;

        /* Seed Info */
        /** The base seed that our algorith should use **/
        public int seed = 777;
        /** The current seed **/
        public int curSeed;
        /** The list of seeds applied to each Generation, may be useful in amplifying acceleration of fitness **/
        public List<float> seeds;
        /** Used to check whether the seed should be offset **/
        public float seedFluctuateRate = 0.1f;
        /** The range of values to be added to the Seed value **/
        public float seedRange = 100f;
        /** The System Random, to be used in combination with Unity Random Seeds**/
        System.Random sysRandom;


        /** Create the new Algorithm based on predefined values **/
        public virtual void Init()
        {
            /* Assign user defined methods to the FitnessFunction/GetRandomGene function, for example: */
            //FitnessFunction = test;


            pool = new GAPool<T>(populationSize, dnaSize, FitnessFunction, GetRandomGene, random, mutationRate, curSeed);
            seeds.Add(seed);
        }

        /** Create some fluctuations in the Random Seed **/
        private void FluctuateSeed()
        {
            float amount = Mathf.Clamp((float)sysRandom.NextDouble(), 0, seedFluctuateRate);
            curSeed = (int) Math.Round(seedRange * amount);
            UnityEngine.Random.InitState(curSeed);
        }

        public float test(int boop)
        {
            return 0;
        }
    }
}
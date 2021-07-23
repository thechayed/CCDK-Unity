/** This script contains all of the default behavior of the Genetic Learning Algorithm, extend the GeneticAlgorithmManager to create new GeneticAlgorithms **/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCDKEngine
{
    /** This is a generic class for Gentic Populations, extend it to create new types AI's **/
    public class GAPool<T>
    {
        /** The current generation number **/
        public int generation;
        /** The population of Individuals **/
        public List<DNA<T>> population { get; private set; }
        
        /** The fitness of the best Individual in the current Generation **/
        public float bestFitness { get; private set; }
        /** The genes of the most fit Individual **/
        public T[] bestGenes { get; private set; } 

        /** The seed that our algorith should use **/
        public int seed = 777;

        /** The rate of random mutations for an Individual **/
        public float mutationRate;
        /** The total fitness of the current Population **/
        public float fitnessSum;

        public GAPool(int populationSize, int dnaSize, Func<int, float> FitnessFunction, Func<T> GetRandomGene, System.Random random, float mutationRate = 0.01f, int seed = 777)
        {
            /** Get the current seed of the Algorithm **/
            this.seed = seed;
            UnityEngine.Random.InitState(seed);

            /** Set the Generation index to 1 **/
            generation = 1;
            /** Get the Mutation Rate from the constructor **/
            this.mutationRate = mutationRate;
            /** Create a new Population **/
            population = new List<DNA<T>>();

            /** Create the original Population **/
            for(int i = 0; i < populationSize; i++)
            {
                population.Add(new DNA<T>(dnaSize, FitnessFunction, GetRandomGene, shouldInit: true));
            }
        }

        /** Create a new Generation **/
        public void NewGeneration()
        {
            /** If the population is emptry, stop the function **/
            if(population.Count == 0)
            {
                return;
            }

            /** Calculate the Fitness of the Individuals **/
            CalculateFitness();

            /** Initialize a new Population **/
            List<DNA<T>> newPopulation = new List<DNA<T>>();

            /** Create all the children for the new Population **/
            for (int i = 0; i < population.Count; i++)
            {
                DNA<T> parent1 = ChooseParent();
                DNA<T> parent2 = ChooseParent();

                DNA<T> child = parent1.Crossover(parent2);

                child.Mutate(mutationRate);

                newPopulation.Add(child);
            }

            population = newPopulation;

            generation++;
        }

        /** Calculate the Fitness of the Individuals, add it to the Fitness Sum of the Population, and store it's information if it is best**/
        public void CalculateFitness()
        {
            fitnessSum = 0;
            DNA<T> best = population[0];

            for(int i = 0; i<population.Count; i++)
            {
                fitnessSum += population[i].CalculateFitness(i);

                if (best.fitness < population[i].fitness)
                {
                    best = population[i];
                }
            }

            bestFitness = best.fitness;
            bestGenes = best.genes;
        }

        /** Choose a Parent based on it's Fitness value **/
        private DNA<T> ChooseParent()
        {
            /** Generate a random number within our Fitness Sum **/
            Double randomNumber = UnityEngine.Random.value * fitnessSum;

            /** Loop through the Population to find an Individual that has a fitness value above the Random number. If one isn't found, the RandomNumber is
             * decremented in order to find the next most fit Individual **/
            for(int i = 0; i< population.Count; i++)
            {
                if (randomNumber < population[i].fitness)
                {
                    return population[i];
                }
                else
                {
                    randomNumber -= population[i].fitness;
                }
            }

            /** If nothing was found, a fit Individual cannot be found. Return the first Individual in the Population **/
            Debug.LogError("No Individauls were found in the Population while choosing a Parent!");
            return population[0];
            /* Note: This should almost never happen. If an Individual ever has a fitness value above 0, it will be returned. This means, 
             * the Fitness value of an Individual has somehow become less than 0. */
        }
    }

    /** The DNA pool of an Individual **/
    public class DNA<T>
    {
        /** The Genes in the pool **/
        public T[] genes { get; private set; }
        /** The fitness of this strand of DNA **/
        public float fitness;
        /** User defined function to get a random Gene **/
        private Func<T> GetRandomGene;
        /** User defined function to decide the fitness of this DNA **/
        private Func<int, float> FitnessFunction;

        public DNA(int size,Func<int, float> FitnessFunction, Func<T> GetRandomGene, bool shouldInit = true) 
        {
            /** Init Genes when the DNA is constructed **/
            genes = new T[size];
            /** Get the User Defined Functions from passed to the Constructor **/
            this.GetRandomGene = GetRandomGene;
            this.FitnessFunction = FitnessFunction;

            /** If we should Initialize default values, call the GetRandomGene function for each of our Genes **/
            if (shouldInit)
            {
                for (int i = 0; i < size; i++)
                {
                    genes[i] = GetRandomGene();
                }
            }
        }

        /** Calculate the fitness of this strand of DNA and return it **/
        public float CalculateFitness(int index)
        {
            fitness = FitnessFunction(index);
            return fitness;
        }

        /** Mix two strands of DNA together to create a child **/
        public DNA<T> Crossover(DNA<T> otherParent)
        {
            /** Construct the Child DNA **/
            DNA<T> child = new DNA<T>(genes.Length, FitnessFunction, GetRandomGene, false);

            /** Loopp through the Child's genes and use a random number to determine whether to use this DNA's gene or another Parent **/
            for (int i = 0; i < genes.Length; i++)
            {
                int pick = (int) UnityEngine.Random.Range(0,1);
                child.genes[i] = pick == 0 ? genes[i] : otherParent.genes[i];
            }

            /** With the Child fully constructed, return it **/
            return child;
        }

        /** Mutate the DNA strand to give it slightly random genes, based on the Mutation Rate **/
        public void Mutate(float mutationRate)
        {
            for(int i = 0; i< genes.Length; i++)
            {
                if (UnityEngine.Random.Range(0, 1) < mutationRate)
                {
                    genes[i] = GetRandomGene();
                }
            }
        }
    }
}
namespace GeneticAlgorithmLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Chromosomes;
    using Selections;

    public sealed class Population
    {
        private static Random generator = new Random();

        private readonly int iterationCount;
        public int IterationCount { get { return iterationCount; } }

        private readonly int populationSize;
        public int PopulationSize { get { return populationSize; } }

        private readonly double crossoverRate;
        public double CrossoverRate { get { return crossoverRate; } }

        private readonly double mutationRate;
        public double MutationRate { get { return mutationRate; } }

        private readonly double selectionRate;
        public double SelectionRate { get { return selectionRate; } }

        private ISelection selectionMethod;
        private List<BaseChromosome> chromosomes;

        public Population(int iterationCount, int populationSize, double crossoverRate, double mutationRate, double selectionRate, ISelection selectionMethod, BaseChromosome ancestor)
        {
            this.iterationCount = iterationCount <= 0 ? 100 : iterationCount;
            this.populationSize = populationSize <= 0 ? 30 : populationSize;
            this.crossoverRate = crossoverRate <= 0.0 || crossoverRate > 1.0 ? 0.75 : crossoverRate;
            this.mutationRate = mutationRate <= 0.0 || mutationRate > 1.0 ? 0.05 : mutationRate;
            this.selectionRate = selectionRate <= 0.0 || selectionRate > 1.0 ? 0.9 : selectionRate;
            this.selectionMethod = selectionMethod;
            this.chromosomes = new List<BaseChromosome>(populationSize);

            chromosomes.Add(ancestor);

            for (int i = 1; i < populationSize; i++)
                chromosomes.Add(ancestor.CreateNew());

            chromosomes.ForEach(individual => individual.Eval());
        }

        public BaseChromosome ExecuteGA()
        {
            for (int i = 0; i < iterationCount; i++)
            {
                ApplySelection();
                ApplyCrossover();
                ApplyMutation();   
            }
               
            return chromosomes.OrderByDescending(individual => individual.Fitness).ElementAt(0); 
        }

        private void ApplySelection()
        {
            var selectionSize = Convert.ToInt32(selectionRate * populationSize);
            selectionMethod.Select(chromosomes, selectionSize);

            for (int i = populationSize - selectionSize; i > 0; i--)
            {
                var chromosome = chromosomes[0].CreateNew();
                chromosome.Eval();
                chromosomes.Add(chromosome);
            }
        }

        private void ApplyCrossover()
        {
            for (int i = 1; i < populationSize; i+=2)
                if (generator.NextDouble() <= crossoverRate)
                {
                    BaseChromosome chromosome1 = chromosomes[i-1].Clone();
                    BaseChromosome chromosome2 = chromosomes[i].Clone();

                    chromosome1.Crossover(chromosome2);
                    chromosome1.Eval();
                    chromosome2.Eval();

                    chromosomes.Add(chromosome1);
                    chromosomes.Add(chromosome2);
                }
        }

        private void ApplyMutation()
        {
            for (int i = 0; i < populationSize; i++)
                if (generator.NextDouble() <= mutationRate)
                {
                    BaseChromosome chromosome = chromosomes[i].Clone();
                    chromosome.Mutate();
                    chromosome.Eval();
                    chromosomes.Add(chromosome);
                }
        }
    }
}

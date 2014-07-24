namespace GeneticAlgorithmLib.Selections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Chromosomes;

    public sealed class RouletteWheelSelection : ISelection
    {
        private static Random generator = new Random();

        public void Select(IList<BaseChromosome> chromosomes, int size)
        {
            var totalFitness = chromosomes.Sum(chromosome => chromosome.Fitness);
            var chances = chromosomes.Select(chromosome => chromosome.Fitness / totalFitness).ToArray();
            var population = new List<BaseChromosome>();

            int i;
            int j;
            double number;

            for (i = 0; i < size; i++)
            {
                number = generator.NextDouble();
                
                for (j = 0; number > 0; j++)
                    number -= chances[j];

                population.Add(j == chromosomes.Count ? chromosomes[j-1] : chromosomes[j]);
            }

            chromosomes.Clear();

            foreach (var individual in population)
                chromosomes.Add(individual);
        }
    }
}

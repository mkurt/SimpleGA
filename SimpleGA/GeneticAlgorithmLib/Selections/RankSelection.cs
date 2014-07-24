namespace GeneticAlgorithmLib.Selections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Chromosomes;

    public sealed class RankSelection : ISelection
    {
        private static Random generator = new Random();

        public void Select(IList<BaseChromosome> chromosomes, int size)
        {
            var arr = chromosomes.OrderByDescending(chromosome => chromosome.Fitness).ToArray();
            var ranked = arr.Select((chromosome, index) => arr.Length - index);
            var total = (double)ranked.Sum();
            var chances = ranked.Select(rank => rank / total).ToArray();
            var population = new List<BaseChromosome>();

            int i;
            int j;
            double number;

            for (i = 0; i < size; i++)
            {
                number = generator.NextDouble();

                for (j = 0; number > 0; j++)
                    number -= chances[j];
		
                population.Add(j == arr.Length ? arr[j-1] : arr[j]);
            }
            
            chromosomes.Clear();

            foreach (var individual in population)
                chromosomes.Add(individual);
        }
    }
}

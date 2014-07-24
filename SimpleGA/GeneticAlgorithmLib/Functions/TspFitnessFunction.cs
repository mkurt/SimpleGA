namespace GeneticAlgorithmLib.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Structures;
    using Chromosomes;

    public sealed class TspFitnessFunction : IFitnessFunction
    {
        private IDictionary<Pair<string,string>,double> data;

        public TspFitnessFunction(IDictionary<Pair<string,string>,double> tspData)
        {
            data = tspData;
        }

        public double Evaluate(BaseChromosome chromosome)
        {
            var tspChromosome = (TspChromosome)chromosome;
            var genes = new int[tspChromosome.Genes.Count + 1];
            
            genes[0] = tspChromosome.GetStartCityId();
            tspChromosome.Genes.CopyTo(genes, 1);

            var pairs = genes.Select((value,index) => new Pair<string,string>(tspChromosome.GetCityAddress(value), tspChromosome.GetCityAddress(genes[(index + 1) % genes.Length])));
            var totalDistance = pairs.Select(pair => data[pair]).Sum();
            return 1 / totalDistance;
        }
    }
}

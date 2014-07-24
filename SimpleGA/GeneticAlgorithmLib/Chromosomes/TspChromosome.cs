namespace GeneticAlgorithmLib.Chromosomes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Functions;
    using Structures;

    public sealed class TspChromosome : PermutationChromosome
    {
        private Dictionary<string,int> addressIdDict;

        public Pair<string,int>[] Tour
        {
            get
            {
                var sequenceVal = 0;
                var arr = new Pair<string,int>[genes.Length + 1];
                arr[0] = new Pair<string,int>(GetCityAddress(GetStartCityId()), sequenceVal);

                for (int i = 0; i < genes.Length; i++)
                    arr[i+1] = new Pair<string,int>(GetCityAddress(genes[i]), ++sequenceVal);

                return arr;
            }
        }

        public TspChromosome(IList<Pair<string,int>> cities, IFitnessFunction fitnessFunction) : base(fitnessFunction, cities.Count-1)
        {
            addressIdDict = new Dictionary<string,int>();
            
            var startCity = cities.First(city => city.Second == cities.Min(item => item.Second));
            addressIdDict.Add(startCity.First, 0);

            for (int idVal = 0, i = 0; i < cities.Count; i++)
                if (cities[i].First != startCity.First)
                    addressIdDict.Add(cities[i].First, ++idVal);
        }

        private TspChromosome(IFitnessFunction fitnessFunction, int chromosomeLength) : base(fitnessFunction, chromosomeLength)
        {
        }

        private TspChromosome(TspChromosome source) : base(source)
        {
            addressIdDict = source.addressIdDict;
        }

        public override BaseChromosome Clone()
        {
            return new TspChromosome(this);
        }

        public override BaseChromosome CreateNew()
        {
            var chromosome = new TspChromosome(function, genes.Length);
            chromosome.addressIdDict = addressIdDict;
            return chromosome;
        }

        private int[] CreateChild(TspChromosome p1, TspChromosome p2)
        {
            var length = p1.genes.Length;
            var isUsed = new bool[length];
            var child = Enumerable.Repeat(GetStartCityId(), length).ToArray();

            child[0] = p1.genes[0];
            isUsed[child[0] - 1] = true;

            var dumb = new TspChromosome(this);
            var dumber = new TspChromosome(this);

            for (int i = 1; i < length; i++)
            {
                var nextGene = p1.genes[(Array.IndexOf(p1.genes, child[i - 1]) + 1) % length];
                var nextGeneInMate = p2.genes[(Array.IndexOf(p2.genes, child[i - 1]) + 1) % length];
                var isValid = !isUsed[nextGene - 1];
                var isValidInMate = !isUsed[nextGeneInMate - 1];

                if (isValid && isValidInMate)
                {
                    dumb.genes = (int[]) child.Clone();
                    dumb.genes[i] = nextGene;
                    dumb.Eval();

                    dumber.genes = (int[]) child.Clone();
                    dumber.genes[i] = nextGeneInMate;
                    dumber.Eval();

                    child[i] = dumb.fitness > dumber.fitness ? nextGene : nextGeneInMate;
                }
                else if (!(isValid || isValidInMate))
                {
                    var randomIndex = generator.Next(length);

                    while (isUsed[p1.genes[randomIndex % length] - 1])
                        randomIndex++;

                    child[i] = p1.genes[randomIndex % length];
                }
                else
                {
                    child[i] = isValid ? nextGene : nextGeneInMate;
                }

                isUsed[child[i] - 1] = true;
            }
            
            return child;
        }

        public override void Mutate()
        {
            var previousFitness = fitness;

            for (int i = 0, idx1, idx2, tmp; i < genes.Length; i++)
            {
                idx1 = generator.Next(genes.Length);
                idx2 = generator.Next(genes.Length);

                if (idx1 != idx2)
                {
                    tmp = genes[idx1];
                    genes[idx1] = genes[idx2];
                    genes[idx2] = tmp;

                    Eval();

                    if (previousFitness < fitness)
                        return;

                    genes[idx2] = genes[idx1];
                    genes[idx1] = tmp;
                }
            }
        }

        public int GetStartCityId()
        {
            return 0;
        }

        public string GetCityAddress(int cityId)
        {
            return addressIdDict.Where(x => x.Value == cityId).First().Key;
        }
    }
}

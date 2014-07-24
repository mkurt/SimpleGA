namespace GeneticAlgorithmLib.Chromosomes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Functions;

    public class PermutationChromosome : Chromosome<int>
    {
        protected static Random generator = new Random();

        public PermutationChromosome(IFitnessFunction fitnessFunction, int chromosomeLength) : base(fitnessFunction, chromosomeLength)
        {
            GenerateValues();
        }

        protected PermutationChromosome(PermutationChromosome source) : base(source.function, source.genes.Length)
        {
            genes = (int[])source.genes.Clone();
        }

        public override BaseChromosome Clone()
        {
            return new PermutationChromosome(this);
        }

        public override BaseChromosome CreateNew()
        {
            return new PermutationChromosome(function, genes.Length);
        }

        public override void GenerateValues()
        {
            for (int i = 0; i < genes.Length; i++)
                genes[i] = i + 1;

            genes = genes.OrderBy(gene => Guid.NewGuid()).ToArray();
        }

        private int[] CreateChild(PermutationChromosome p1, PermutationChromosome p2)
        {
            var partitionPoint = (int)Math.Ceiling(p1.genes.Length / 2.0);
            var firstPart = p1.genes.Take(partitionPoint);
            var secondPart = p2.genes.Except(firstPart);

            return firstPart.Concat(secondPart).ToArray();
        }

        public override void Crossover(BaseChromosome other)
        {
            var mate = (PermutationChromosome)other;

            if (mate != null && this.genes.Length == mate.genes.Length)
            {
                var child1 = CreateChild(this, mate);
                var child2 = CreateChild(mate, this);

                this.genes = child1;
                mate.genes = child2;
            }
        }

        public override void Mutate()
        {
            int idx1 = generator.Next(genes.Length);
            int idx2 = generator.Next(genes.Length);

            if (idx1 != idx2)
            {
                int temp = genes[idx1];
                genes[idx1] = genes[idx2];
                genes[idx2] = temp;
            }
        }
    }
}
namespace GeneticAlgorithmLib.Chromosomes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Functions;

    public abstract class Chromosome<T> : BaseChromosome where T : struct
    {
        protected T[] genes;
        public ReadOnlyCollection<T> Genes { get { return new ReadOnlyCollection<T>(genes); } }

        protected Chromosome(IFitnessFunction fitnessFunction, int chromosomeLength) : base(fitnessFunction)
        {
            if (chromosomeLength <= 0)
                throw new ArgumentOutOfRangeException("length", "Chromosome length must be greater than zero.");

            genes = new T[chromosomeLength];
        }

        public abstract override BaseChromosome Clone();
        public abstract override BaseChromosome CreateNew();
        public abstract override void Crossover(BaseChromosome other);
        public abstract override void GenerateValues();
        public abstract override void Mutate();
    }
}

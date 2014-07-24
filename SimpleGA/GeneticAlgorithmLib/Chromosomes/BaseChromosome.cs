namespace GeneticAlgorithmLib.Chromosomes
{
    using System;
    using Functions; 

    public abstract class BaseChromosome
    {
        private static int total = 0;
        protected IFitnessFunction function;

        private readonly int id;
        public int Id { get { return id; } }

        protected double fitness;
        public double Fitness { get { return fitness; } }

        protected BaseChromosome(IFitnessFunction fitnessFunction)
        {
            if (fitnessFunction == null)
                throw new ArgumentNullException("fitnessFunction");

            id = ++total;
            fitness = 0.0;
            function = fitnessFunction;
        }

        public virtual void Eval()
        {
            fitness = function.Evaluate(this);
        }

        public abstract BaseChromosome Clone();
        public abstract BaseChromosome CreateNew();
        public abstract void Crossover(BaseChromosome other);
        public abstract void GenerateValues();
        public abstract void Mutate();
    }
}

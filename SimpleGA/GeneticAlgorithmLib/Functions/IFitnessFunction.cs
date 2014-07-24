namespace GeneticAlgorithmLib.Functions
{
    using System;
    using Chromosomes;

    public interface IFitnessFunction
    {
        double Evaluate(BaseChromosome chromosome);
    }
}

namespace GeneticAlgorithmLib.Selections
{
    using System;
    using System.Collections.Generic;
    using Chromosomes;

    public interface ISelection
    {
        void Select(IList<BaseChromosome> chromosomes, int size);
    }
}

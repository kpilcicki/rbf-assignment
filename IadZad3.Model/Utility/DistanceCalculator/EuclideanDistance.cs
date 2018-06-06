using MathNet.Numerics.LinearAlgebra;
using System;

namespace IadZad3.Model.Utility.DistanceCalculator
{
    public class EuclideanDistance : IDistanceCalculator
    {
        public double CalculateDistance(Vector<double> testInput, Vector<double> current)
        {
            return Math.Sqrt(current.Subtract(testInput).Map(val => val * val).Sum());
        }
    }
}
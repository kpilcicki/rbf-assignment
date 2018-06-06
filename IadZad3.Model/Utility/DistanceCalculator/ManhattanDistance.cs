using MathNet.Numerics.LinearAlgebra;

namespace IadZad3.Model.Utility.DistanceCalculator
{
    public class ManhattanDistance : IDistanceCalculator
    {
        public double CalculateDistance(Vector<double> testInput, Vector<double> current)
        {
            return current.Subtract(testInput).SumMagnitudes();
        }
    }
}
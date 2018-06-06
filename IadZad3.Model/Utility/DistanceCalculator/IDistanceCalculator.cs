using MathNet.Numerics.LinearAlgebra;

namespace IadZad3.Model.Utility.DistanceCalculator
{
    public interface IDistanceCalculator
    {
        double CalculateDistance(Vector<double> testInput, Vector<double> current);
    }
}
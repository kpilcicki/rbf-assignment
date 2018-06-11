using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using System.Collections.Generic;

namespace IadZad3.Model
{
    public class RadialNeuron
    {
        public Vector<double> Position { get; set; }

        public RadialNeuron(int dimensions)
        {
            Position = Vector<double>.Build.Dense(dimensions);
            Outputs = new List<double>();
        }

        public double Width { get; set; }

        public List<double> Outputs { get; set; }
        

    }
}

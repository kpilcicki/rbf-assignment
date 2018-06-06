using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace IadZad3.Model.Utility
{
    public interface INeuronPositioner
    {
        void PositionNeurons(List<Vector<double>> inputPoints, List<RadialNeuron> radialNeurons);
    }
}

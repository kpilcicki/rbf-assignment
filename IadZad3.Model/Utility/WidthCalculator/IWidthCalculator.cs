using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IadZad3.Model.Utility.WidthCalculator
{
    public interface IWidthCalculator
    {
        void CalculateWidths(List<RadialNeuron> radialLayer);
    }
}

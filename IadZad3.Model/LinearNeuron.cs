using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace IadZad3.Model
{
    public class LinearNeuron
    {
        public Vector<double> Weights { get; set; }
        public double BiasWeight { get; set; }
    }
}

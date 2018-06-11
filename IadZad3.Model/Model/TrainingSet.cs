using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IadZad3.Model.Model
{
    public class TrainingSet
    {
        public Vector<double> Input { get; set; }

        public Vector<double> DesiredOutput { get; set; }

        public TrainingSet(List<double> input, List<double> desiredOutput)
        {
            Input = Vector<double>.Build.DenseOfEnumerable(input);
            DesiredOutput = Vector<double>.Build.DenseOfEnumerable(desiredOutput);
        }
    }
}

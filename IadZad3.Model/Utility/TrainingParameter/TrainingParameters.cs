using IadZad3.Model.Model;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IadZad3.Model.Utility.TrainingParameter
{
    public class TrainingParameters
    {
        public List<TrainingSet> InputPoints = new List<TrainingSet>();

        public int MinWeightValue { get; set; }
        public int MaxWeightValue { get; set; }

        public void Validate()
        {
            if (InputPoints.Count == 0) throw new ArgumentException("Input data set is empty");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IadZad3.Model.Utility.MathUtility;
using MathNet.Numerics.LinearAlgebra;

namespace IadZad3.Model
{
    public class LinearNeuron
    {
        public Vector<double> Weights { get; set; }
        public double BiasWeight { get; set; }

        public void InitNeuron(int radialNeuronCount, int minWeight, int maxWeight)
        {
            
            var weights = new double[radialNeuronCount];
            for (int i = 0; i < radialNeuronCount; i++)
            {
                weights[i] = RandomNumberProvider.GetRandomNumber(minWeight, maxWeight);
            }
           
            Weights = Vector<double>.Build.DenseOfArray(weights);
            BiasWeight = RandomNumberProvider.GetRandomNumber(minWeight, maxWeight);
        }
    }
}

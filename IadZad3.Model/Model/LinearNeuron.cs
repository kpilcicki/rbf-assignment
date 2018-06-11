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

        public Vector<double> WeightsDeltas { get; set; }

        public double BiasWeight { get; set; }
        public double BiasWeightDelta { get; set; }


        public void InitNeuron(int radialNeuronCount, double minWeight, double maxWeight)
        {
            
            var weights = new double[radialNeuronCount];
            for (int i = 0; i < radialNeuronCount; i++)
            {
                weights[i] = RandomNumberProvider.GetRandomNumber(minWeight, maxWeight);
            }
           
            Weights = Vector<double>.Build.DenseOfArray(weights);
            WeightsDeltas = Vector<double>.Build.DenseOfArray(new double[radialNeuronCount]);
            BiasWeight = RandomNumberProvider.GetRandomNumber(minWeight, maxWeight);
            BiasWeightDelta = 0;

        }
    }
}

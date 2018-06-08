using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IadZad3.Model.Utility;
using IadZad3.Model.Utility.ActivationFunctions;
using IadZad3.Model.Utility.DistanceCalculator;
using IadZad3.Model.Utility.TrainingParameter;
using IadZad3.Model.Utility.WidthCalculator;
using MathNet.Numerics.LinearAlgebra;

namespace IadZad3.Model
{
    public class RBFNetwork
    {

        private readonly IDistanceCalculator _distance;
        private readonly IGaussianFunction _gaussian;
        private readonly IWidthCalculator _widthCalculator;
        private readonly INeuronPositioner _neuronPositioner;


        public RBFNetwork(IDistanceCalculator distance, IGaussianFunction gaussian, IWidthCalculator widthCalculator, INeuronPositioner positioner, int radialNeuronCount, int linearNeuronCount)
        {
            _distance = distance;
            _gaussian = gaussian;
            _widthCalculator = widthCalculator;
            _neuronPositioner = positioner;
            //RadialNeuronCount = radialNeuronCount;
            RadialLayer = new List<RadialNeuron>();
            RadialLayer.AddRange(new RadialNeuron[radialNeuronCount]);
            //LinearNeuronCount = linearNeuronCount;
            LinearLayer = new List<LinearNeuron>();
            LinearLayer.AddRange(new LinearNeuron[linearNeuronCount]);
        }


        //public int RadialNeuronCount { get;}
        //public int LinearNeuronCount { get;}

        public List<RadialNeuron> RadialLayer { get; set; }
        public List<LinearNeuron> LinearLayer { get; set; }
        public double ScalingCoefficient { get; set; }
        public int KNNeighbors { get; set; }


        private void initLayers(TrainingParameters parameters)
        {
            _neuronPositioner.PositionNeurons(parameters, RadialLayer);

            foreach (var linear in LinearLayer)
            {
                linear.InitNeuron(RadialLayer.Count, parameters.MinWeightValue, parameters.MaxWeightValue);
            }
        }

        public void Train(double scalingCoefficient, int KNNeighbors, TrainingParameters parameters)
        {
            initLayers(parameters);

        }





        public Vector<double> ProcessInput(Vector<double> input)
        {
            Vector<double> tmpValues = Vector<double>.Build.DenseOfArray(new double[RadialLayer.Count]);

            for (int i = 0; i < RadialLayer.Count; i++)
            {
                var diff = _distance.CalculateDistance(input, RadialLayer[i].Position);
                var width = RadialLayer[i].Width * ScalingCoefficient;
                tmpValues[i] = _gaussian.Calculate(diff, width);

            }

            Vector<double> outputValues = Vector<double>.Build.DenseOfArray(new double[LinearLayer.Count]);

            for (int i = 0; i < LinearLayer.Count; i++)
            {
                var weights = LinearLayer[i].Weights;

                for (int j = 0; j < weights.Count; j++)
                {
                    outputValues[i] += weights[j] * tmpValues[j];
                }

                outputValues[i] += LinearLayer[i].BiasWeight * 1.0;

            }

            return outputValues;

        }



    }
}

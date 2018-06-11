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


        public RBFNetwork(IDistanceCalculator distance, IGaussianFunction gaussian, IWidthCalculator widthCalculator, INeuronPositioner positioner, int radialNeuronCount, int linearNeuronCount, int inputDimensions)
        {
            _distance = distance;
            _gaussian = gaussian;
            _widthCalculator = widthCalculator;
            _neuronPositioner = positioner;
            //RadialNeuronCount = radialNeuronCount;
            RadialLayer = new List<RadialNeuron>();
            //RadialLayer.AddRange(new RadialNeuron[radialNeuronCount]);
            for (int i = 0; i < radialNeuronCount; i++)
            {
                RadialLayer.Add(new RadialNeuron(inputDimensions));
            }
            //LinearNeuronCount = linearNeuronCount;
            LinearLayer = new List<LinearNeuron>();
            for (int i = 0; i < linearNeuronCount; i++)
            {
                LinearLayer.Add(new LinearNeuron());
            }
            MeanSquaredErrors = new List<double>();
            //LinearLayer.AddRange(new LinearNeuron[linearNeuronCount]);
        }


        //public int RadialNeuronCount { get;}
        //public int LinearNeuronCount { get;}

        public List<RadialNeuron> RadialLayer { get; set; }
        public List<LinearNeuron> LinearLayer { get; set; }
        //public double ScalingCoefficient { get; set; }
        //public int KNNeighbors { get; set; }
        public List<double> MeanSquaredErrors { get; set; }


        private void initLayers(TrainingParameters parameters)
        {
            _neuronPositioner.PositionNeurons(parameters, RadialLayer);
            _widthCalculator.CalculateWidths(RadialLayer);

            foreach (var linear in LinearLayer)
            {
                linear.InitNeuron(RadialLayer.Count, parameters.MinWeightValue, parameters.MaxWeightValue);
            }
        }

        public void Train(BackpropagationTrainingParameters parameters)
        {
            MeanSquaredErrors.Clear();
            initLayers(parameters);

            var epochs = parameters.Epochs;
            var learningRate = parameters.LearningRate;
            var maxWeightValue = parameters.MaxWeightValue;
            var minWeightValue = parameters.MinWeightValue;
            var momentum = parameters.Momentum;

            for (int i = 0; i < epochs; i++)
            {
                for(int j = 0; j < RadialLayer.Count; j++)
                { 
                    RadialLayer[j].Outputs.Clear();
                }
                double meanSquaredErrorAggregate = 0; // sum of all error squares of one epoch
                foreach ( var point in parameters.InputPoints)
                {
                    var networkOutput = ProcessInput(point.Input);
                    var deltas = (networkOutput - point.DesiredOutput);//(*1) differential of linear neuron function is 1

                    meanSquaredErrorAggregate += deltas.Sum(delta => delta * delta);// sqrt(SUM[Dij^2]) add to the aggregate
                    

                    for (int j = 0; j < LinearLayer.Count; j++)
                    {
                        //update weights based on radial neurons' outputs
                        for (int k = 0; k < RadialLayer.Count; k++)
                        {
                            var instantDelta = (deltas[j] * RadialLayer[k].Outputs.Last()) * learningRate;// LR * deltaM * xN                            

                            LinearLayer[j].WeightsDeltas[k] = -(instantDelta + (LinearLayer[j].WeightsDeltas[k] * momentum));// add momentum
                            LinearLayer[j].Weights[k] = LinearLayer[j].Weights[k] + LinearLayer[j].WeightsDeltas[k]; //update weights(-LR*POCHODNABLEDU)
                        }
                        //Bias update
                        LinearLayer[j].BiasWeightDelta = -((deltas[j] * learningRate) + (LinearLayer[j].BiasWeightDelta * momentum));
                        LinearLayer[j].BiasWeight = LinearLayer[j].BiasWeight + LinearLayer[j].BiasWeightDelta;
                    }

                }
                MeanSquaredErrors.Add(Math.Sqrt(meanSquaredErrorAggregate));// sqrt of the sum of all errors
            }


        }





        public Vector<double> ProcessInput(Vector<double> input)
        {
            Vector<double> tmpValues = Vector<double>.Build.DenseOfArray(new double[RadialLayer.Count]);

            for (int i = 0; i < RadialLayer.Count; i++)
            {
                var diff = _distance.CalculateDistance(input, RadialLayer[i].Position);
                var width = RadialLayer[i].Width;// * ScalingCoefficient;
                tmpValues[i] = _gaussian.Calculate(diff, width);

                RadialLayer[i].Outputs.Add(tmpValues[i]); // radial output saving test
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

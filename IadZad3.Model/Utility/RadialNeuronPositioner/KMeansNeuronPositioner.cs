using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAD_zad2.Model;
using IAD_zad2.Utilities.Generators;
using IadZad3.Model.Utility.TrainingParameter;

namespace IadZad3.Model.Utility.RadialNeuronPositioner
{
    public class KMeansNeuronPositioner : INeuronPositioner
    {
        public void PositionNeurons(BackpropagationTrainingParameters trainingParameters, List<RadialNeuron> radialNeurons)
        {
            if (!(trainingParameters is KMeansBackpropagationTrainingParameters)) throw new ArgumentException("k-means positioner passed non k-means arguments");

            var SOMTrainingParameters = new IAD_zad2.Model.Parameters.KMeansTrainingParameters()
            {
                TrainingData = trainingParameters.InputPoints.Select(set => set.Input).ToList(),
                Epochs = ((KMeansBackpropagationTrainingParameters)trainingParameters).KMeansEpochs
            };

            var inputPoints = trainingParameters.InputPoints;
            double minPosition = inputPoints.Min(elem => elem.Input.Min());
            double maxPosition = inputPoints.Max(elem => elem.Input.Max());
            int dimensions = inputPoints.First().Input.Count;

            //train the KMeansNetwork to distribute rbf centers evenly
            KMeansNetwork mapNetwork = new KMeansNetwork(radialNeurons.Count, new NeuronRandomRectangularInitializer(minPosition, maxPosition, dimensions), new IAD_zad2.Utilities.Distance.EuclideanDistance());
            mapNetwork.Train(SOMTrainingParameters);

            //Copy the trained positions of radial neurons
            for (int i = 0; i < radialNeurons.Count; i++)
            {
                radialNeurons[i].Position = mapNetwork.Neurons[i].CurrentWeights.Clone();
            }
        }
    }
}

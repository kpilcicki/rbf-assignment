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
    class KMeansNeuronPositioner : INeuronPositioner
    {
        public void PositionNeurons(TrainingParameters trainingParameters, List<RadialNeuron> radialNeurons)
        {
            var SOMTrainingParameters = new IAD_zad2.Model.Parameters.KMeansTrainingParameters()
            {
                Epochs = 
            };
            var inputPoints = trainingParameters.InputPoints;
            double minPosition = inputPoints.Min(elem => elem.Input.Min());
            double maxPosition = inputPoints.Max(elem => elem.Input.Max());
            int dimensions = inputPoints.First().Input.Count;


            KMeansNetwork mapNetwork = new KMeansNetwork(radialNeurons.Count, new NeuronRandomRectangularInitializer(minPosition, maxPosition, dimensions), new IAD_zad2.Utilities.Distance.EuclideanDistance());
            mapNetwork.Train(SOMTrainingParameters);
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IadZad3.Model.Model;

namespace IadZad3.Model.Utility.TrainingParameter
{
    public class KMeansBackpropagationTrainingParameters : BackpropagationTrainingParameters
    {
        public int KMeansEpochs{ get; set; }
        public KMeansBackpropagationTrainingParameters(double learningRate, int epochs, double momentum, double minWeightValue, double maxWeightValue, List<TrainingSet> trainingPoints, int kMeansEpochs) : base(learningRate, epochs, momentum, minWeightValue, maxWeightValue, trainingPoints)
        {
            KMeansEpochs = kMeansEpochs;
        }
    }
}

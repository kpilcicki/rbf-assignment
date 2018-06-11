using IadZad3.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IadZad3.Model.Utility.TrainingParameter
{
    public class BackpropagationTrainingParameters : TrainingParameters
    {
        public double LearningRate { get; set; }
        public int Epochs { get; set; }
        public double Momentum { get; set; }


        public BackpropagationTrainingParameters(double learningRate, int epochs, double momentum, double minWeightValue, double maxWeightValue, List<TrainingSet> trainingPoints) : base(trainingPoints, minWeightValue, maxWeightValue)
        {
            LearningRate = learningRate;
            Epochs = epochs;
            Momentum = momentum;
        }

        public void Validate()
        {
            base.Validate();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IadZad3.Model.Utility.TrainingParameter
{
    class BackpropagationTrainingParameters : TrainingParameters
    {
        public double LearningRate { get; set; }
        public int Epochs { get; set; }
        public double Momentum { get; set; }



        public void Validate()
        {
            base.Validate();
        }
    }
}

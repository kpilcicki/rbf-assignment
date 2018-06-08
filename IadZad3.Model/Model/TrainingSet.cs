using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IadZad3.Model.Model
{
    public class TrainingSet
    {
        public List<double> Input { get; set; }

        public List<double> DesiredOutput { get; set; }

        public TrainingSet(List<double> input, List<double> desiredOutput)
        {
            Input = input;
            DesiredOutput = desiredOutput;
        }
    }
}

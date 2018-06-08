using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IadZad3.Model.Utility.TrainingParameter;
using MathNet.Numerics.LinearAlgebra;

namespace IadZad3.Model.Utility
{
    public interface INeuronPositioner
    {
        void PositionNeurons(TrainingParameters trainingParameters, List<RadialNeuron> radialNeurons);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IadZad3.Model.Utility.MathUtility;
using IadZad3.Model.Utility.TrainingParameter;
using MathNet.Numerics.LinearAlgebra;

namespace IadZad3.Model.Utility
{
    public class RandomNeuronPositioner : INeuronPositioner
    {
        public void PositionNeurons(BackpropagationTrainingParameters trainingParameters, List<RadialNeuron> radialNeurons)
        {
            trainingParameters.Validate();

            var inputPoints = trainingParameters.InputPoints;

            List<int> randomPositions = new List<int>();
            for (int i = 0; i < radialNeurons.Count; i++)
            {
                int randomPosition;
                do
                {
                    randomPosition = (int) RandomNumberProvider.GetRandomNumber(0, inputPoints.Count);

                } while (randomPositions.Exists(e => e == randomPosition));
                randomPositions.Add(randomPosition);
            }

            int radialIndex = 0;
            foreach (var index in randomPositions)
            {
                for (int i = 0; i < inputPoints[0].Input.Count; i++)
                {
                    radialNeurons[radialIndex].Position[i] = inputPoints[index].Input[i];
                }
                radialIndex++;
            }
        }
    }
}

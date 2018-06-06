using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IadZad3.Model.Utility.MathUtility;
using MathNet.Numerics.LinearAlgebra;

namespace IadZad3.Model.Utility
{
    class RandomNeuronPositioner : INeuronPositioner
    {
        public void PositionNeurons(List<Vector<double>> inputPoints, List<RadialNeuron> radialNeurons)
        {
            List<int> randomPositions = new List<int>(radialNeurons.Count);
            for (int i = 0; i < randomPositions.Count; i++)
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
                for (int i = 0; i < inputPoints.First().Count; i++)
                {
                    radialNeurons[radialIndex++].Position[i] = inputPoints[index][i];
                }
            }
        }
    }
}

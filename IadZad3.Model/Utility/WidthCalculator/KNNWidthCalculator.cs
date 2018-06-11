using IadZad3.Model.Utility.DistanceCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IadZad3.Model.Utility.WidthCalculator
{
    public class KNNWidthCalculator : IWidthCalculator
    {
        private readonly IDistanceCalculator _distance;
        public int KNeighbors { get; set; }
        public double ScalingCoefficient { get; set; }

        public KNNWidthCalculator(IDistanceCalculator distance, int kNeighbors, double scalingCoefficient)
        {
            _distance = distance;
            KNeighbors = kNeighbors;
            ScalingCoefficient = scalingCoefficient;
        }

        public void CalculateWidths(List<RadialNeuron> radialLayer)
        {
            for (int i = 0; i < radialLayer.Count; i++)
            {
                List<Tuple<RadialNeuron, double>> centersWithDistances = new List<Tuple<RadialNeuron, double>>();
                for (int j = 0; j < radialLayer.Count; j++)
                {
                    if (i != j)
                    {
                        var calculatedDistance = _distance.CalculateDistance(radialLayer[i].Position, radialLayer[j].Position);
                        centersWithDistances.Add(new Tuple<RadialNeuron, double>(radialLayer[j], calculatedDistance));
                    }
                }

                var orderedByDistance = centersWithDistances.OrderBy(tuple => tuple.Item2).Take(KNeighbors);

                var toCount = ScalingCoefficient * Math.Sqrt( (1.0/KNeighbors) *  orderedByDistance.Sum(elem => elem.Item2 * elem.Item2)); // sqrt( alpha * 1/p * SUM[(Dij)^2] )

                radialLayer[i].Width = toCount; // should set wanted values
            }
        }
    }
}

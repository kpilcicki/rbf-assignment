using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IadZad3.Model.Utility.MathUtility
{
    public static class RandomNumberProvider
    {
        private static readonly Random Rand = new Random();

        public static double GetRandomNumber(double min, double max)
        {
            return (max - min) * Rand.NextDouble() + min;
        }
    }

}

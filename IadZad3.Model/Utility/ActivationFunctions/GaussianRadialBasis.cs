using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IadZad3.Model.Utility.ActivationFunctions
{
    public class GaussianRadialBasis : IGaussianFunction
    {
        public double Calculate(double diff, double width)
        {
            double exponent = (-1.0 * diff * diff ) / (2 * width * width);
            return Math.Pow(Math.E, exponent);
        }
    }
}

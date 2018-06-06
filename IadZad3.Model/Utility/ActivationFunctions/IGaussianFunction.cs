using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IadZad3.Model.Utility.ActivationFunctions
{
    public interface IGaussianFunction
    {
        double Calculate(double diff, double width);

    }
}

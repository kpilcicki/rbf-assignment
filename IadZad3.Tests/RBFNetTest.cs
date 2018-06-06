using System;
using IadZad3.Model;
using IadZad3.Model.Utility.ActivationFunctions;
using IadZad3.Model.Utility.DistanceCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IadZad3.Tests
{
    [TestClass]
    public class RBFNetTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            RBFNetwork network = new RBFNetwork(new EuclideanDistance(),
                                                new GaussianRadialBasis());
            
        }
    }
}

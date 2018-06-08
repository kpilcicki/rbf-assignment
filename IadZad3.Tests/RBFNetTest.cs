using System;
using IadZad3.Model;
using IadZad3.Model.Utility;
using IadZad3.Model.Utility.ActivationFunctions;
using IadZad3.Model.Utility.DataUtility;
using IadZad3.Model.Utility.DistanceCalculator;
using IadZad3.Model.Utility.WidthCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IadZad3.Tests
{
    [TestClass]
    public class RBFNetTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var distCal = new EuclideanDistance();
            RBFNetwork network = new RBFNetwork(distCal,
                                                new GaussianRadialBasis(),
                                                new KNNWidthCalculator(distCal, 9, 0.5),
                                                new RandomNeuronPositioner(),
                                                10, 3);


            
        }

        [TestMethod]
        public void DataGettingTest()
        {
            DataGetter dg = new DataGetter();
            var data = dg.GetData("approximation1.txt",' ');

            var one = dg.GetTrainingDataWithOneOutput("approximation1.txt", 1);

            var two = dg.GetTrainingDataWithChosenInputs("classification.txt",new bool[] { true, true, true, true });

            { Console.WriteLine("test"); }
        }
    }
}

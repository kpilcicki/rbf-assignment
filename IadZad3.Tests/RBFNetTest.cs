using System;
using IadZad3.Model;
using IadZad3.Model.Utility;
using IadZad3.Model.Utility.ActivationFunctions;
using IadZad3.Model.Utility.DataUtility;
using IadZad3.Model.Utility.DistanceCalculator;
using IadZad3.Model.Utility.TrainingParameter;
using IadZad3.Model.Utility.WidthCalculator;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IadZad3.Tests
{
    [TestClass]
    public class RBFNetTest
    {
        [TestMethod]
        public void TestMethod1()
        {


            
        }

        [TestMethod]
        public void DataGettingTest()
        {
            DataGetter dg = new DataGetter();
            var data = dg.GetData("approximation1.txt",' ');

            var one = dg.GetTrainingDataWithOneOutput("approximation1.txt", 1);

            var test = dg.GetTrainingDataWithOneOutput("approximation_test.txt", 1);

            var two = dg.GetTrainingDataWithChosenInputs("classification.txt",new bool[] { true, true, true, true });

            { Console.WriteLine("test"); }

            var distCal = new EuclideanDistance();
            RBFNetwork network = new RBFNetwork(distCal,
                                                new GaussianRadialBasis(),
                                                new KNNWidthCalculator(distCal, 2, 1),
                                                new RandomNeuronPositioner(),
                                                2, one[0].DesiredOutput.Count, one[0].Input.Count);

            network.Train(new BackpropagationTrainingParameters(0.5, 20, 0, -1, 1, one), test);

            var output = network.ProcessInput(test[0].Input);
        }
    }
}

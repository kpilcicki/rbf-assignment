using IadZad3.Model;
using IadZad3.Model.Model;
using IadZad3.Model.Utility;
using IadZad3.Model.Utility.ActivationFunctions;
using IadZad3.Model.Utility.DataUtility;
using IadZad3.Model.Utility.DistanceCalculator;
using IadZad3.Model.Utility.RadialNeuronPositioner;
using IadZad3.Model.Utility.TrainingParameter;
using IadZad3.Model.Utility.WidthCalculator;
using MathNet.Numerics.LinearAlgebra;
using OxyPlot;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IadZad3.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private int radialCount;
        private int linearCount;
        private int inputCount;
        private int KNeighbors;
        private double scalingCoefficient;

        private INeuronPositioner chosenPositioner = new RandomNeuronPositioner();

        public RBFNetwork Network { get; set; }

        public string TestingSetPath { get; set; }

        internal enum TaskSelection
        {
            APPROX,
            CLASS
        }

        private TaskSelection taskSelected = TaskSelection.APPROX;

        private void CreateNetworl_Click(object sender, RoutedEventArgs e)
        {
            radialCount = Int32.Parse(RadialNeuronCount.Text);
            linearCount = Int32.Parse(LinearNeuronCount.Text);
            inputCount = Int32.Parse(InputVectorCount.Text);
            KNeighbors = Int32.Parse(KNeighborsCount.Text);
            scalingCoefficient = Double.Parse(ScalingCoefficientInput.Text, CultureInfo.InvariantCulture);
            


            Network = new RBFNetwork(new EuclideanDistance(),
                                     new GaussianRadialBasis(),
                                     new KNNWidthCalculator(new EuclideanDistance(), KNeighbors, scalingCoefficient),
                                     new RandomNeuronPositioner(),
                                     radialCount, linearCount, inputCount);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedName = ((ComboBoxItem)TaskCombo.SelectedItem).Content;

            switch(selectedName)
            {
                case "Approximation":
                    taskSelected = TaskSelection.APPROX;
                    TrainingSetPath.Text = "approximation1.txt";
                    TestingSetPath = "approximation_test.txt";
                    chosenPositioner = new RandomNeuronPositioner();
                    break;
                case "Classification":
                    taskSelected = TaskSelection.CLASS;
                    TrainingSetPath.Text = "classification.txt";
                    TestingSetPath = "classification_test.txt";
                    chosenPositioner = new KMeansNeuronPositioner();
                    break;
            }
        }

        

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            var learningRate = Double.Parse(LearningRateInput.Text, CultureInfo.InvariantCulture);
            var epochs = Int32.Parse(EpochsCount.Text);
            var momentum = Double.Parse(MomentumInput.Text, CultureInfo.InvariantCulture);
            var minWeight = Double.Parse(MinInitialWeight.Text, CultureInfo.InvariantCulture);
            var maxWeight = Double.Parse(MaxInitialWeight.Text, CultureInfo.InvariantCulture);
            var trainingPath = TrainingSetPath.Text;
            var kMeansEpochs = Int32.Parse(EpochsCount.Text);
            bool[] chosenInputs = new bool[] {
                Input1Check.IsChecked.Value,
                Input2Check.IsChecked.Value,
                Input3Check.IsChecked.Value,
                Input4Check.IsChecked.Value
            };

            BackpropagationTrainingParameters parameters;

            DataGetter dg = new DataGetter();
            var trainingData = new List<TrainingSet>();
            var testingData = new List<TrainingSet>();
            switch (taskSelected)
            {
                case TaskSelection.APPROX:
                    trainingData = dg.GetTrainingDataWithOneOutput(trainingPath, inputCount);
                    testingData = dg.GetTrainingDataWithOneOutput(TestingSetPath, inputCount);
                    parameters = new BackpropagationTrainingParameters(learningRate, epochs, momentum, minWeight, maxWeight, trainingData);
                    Network.Train(parameters, testingData);

                    

                    ApproximationExample(trainingData, testingData);
                    break;
                case TaskSelection.CLASS:
                    trainingData = dg.GetTrainingDataWithChosenInputs(trainingPath, chosenInputs);
                    testingData = dg.GetTrainingDataWithChosenInputs(TestingSetPath, chosenInputs);
                    testingData = testingData.Select(set => {
                        set.DesiredOutput = classToVector(set.DesiredOutput.At(0));
                        return set;
                    }).ToList();
                    trainingData = trainingData.Select(set => {
                        set.DesiredOutput = classToVector(set.DesiredOutput.At(0));
                        return set;
                    }).ToList();
                    parameters = new KMeansBackpropagationTrainingParameters(learningRate, epochs, momentum, minWeight, maxWeight, trainingData, kMeansEpochs);
                    Network.Train(parameters, testingData);

                    

                    ClassificationExample(trainingData, testingData);
                    break;
                default:
                    throw new InvalidOperationException("use of nonexistent enum element");
            }


            

            
        }

        private void ClassificationExample(List<TrainingSet> trainingData, List<TrainingSet> testingData)
        {
            List<DataPoint> errorPoints = new List<DataPoint>();
            List<DataPoint> testErrorPoints = new List<DataPoint>();
            //int epoch = 0;
            //foreach(var MSE in Network.MeanSquaredErrors)
            //{
            //    errorPoints.Add(new DataPoint(++epoch, MSE));
            //}
            for (int i = 0; i < Network.MeanSquaredErrors.Count; i++)
            {
                errorPoints.Add(new DataPoint(i, Network.MeanSquaredErrors[i]));
                testErrorPoints.Add(new DataPoint(i, Network.TestMeanSquaredErrors[i]));
            }
            List<Series> errorSerieses = new List<Series>()
            {
                new LineSeries()
                {
                    ItemsSource = errorPoints,
                    Title = $"Training errors={errorPoints.Sum(e => e.Y):N3}"
                },
                new LineSeries()
                {
                    ItemsSource = testErrorPoints,
                    Title = $"Test errors={testErrorPoints.Sum(e => e.Y):N3}"
                }
            };

            GraphWindow errorGraph = new GraphWindow("Mean Squared Error by epoch", "epoch", "MSE", errorSerieses);
            errorGraph.Show();

            int[,] errorMatrix = new int[3, 3];
            List<int> gottenClasses = new List<int>();
            foreach(var set in testingData)
            {
                int gottenClass = vectorToClass(Network.ProcessInput(set.Input));
                int desiredClass = (int)Math.Ceiling(set.DesiredOutput.At(0));
                errorMatrix[desiredClass - 1, gottenClass - 1]++; //increment the corresponding 'box'
            }

            WindowDataGrid wdg = new WindowDataGrid(errorMatrix);
            wdg.Show();
        }

        private void ApproximationExample(List<TrainingSet> trainingData, List<TrainingSet> testingData)
        {
            List<DataPoint> graphTrainingPoints = new List<DataPoint>();
            List<DataPoint> graphTestingPoints = new List<DataPoint>();
            foreach (var set in trainingData.OrderBy(elem => elem.Input.At(0)))
            {
                var input = set.Input.At(0);
                graphTrainingPoints.Add(new DataPoint(input, set.DesiredOutput.At(0)));
                graphTestingPoints.Add(new DataPoint(input, Network.ProcessInput(set.Input).At(0)));
            }

            List<DataPoint> realGraphTrainingPoints = new List<DataPoint>();
            List<DataPoint> realGraphTestingPoints = new List<DataPoint>();
            foreach (var set in testingData.OrderBy(elem => elem.Input.At(0)))
            {
                var input = set.Input.At(0);
                realGraphTrainingPoints.Add(new DataPoint(input, set.DesiredOutput.At(0)));
                realGraphTestingPoints.Add(new DataPoint(input, Network.ProcessInput(set.Input).At(0)));
            }

            List<Series> functionSerieses = new List<Series>();
            functionSerieses.Add(new LineSeries()
            {
                ItemsSource = graphTrainingPoints,
                Title = "Training set"
            });
            //functionSerieses.Add(new LineSeries()
            //{
            //    ItemsSource = graphTestingPoints,
            //    Title = "Training set approximation"
            //});
            functionSerieses.Add(new LineSeries()
            {
                ItemsSource = realGraphTrainingPoints,
                Title = "Test set points"
            });
            functionSerieses.Add(new LineSeries()
            {
                ItemsSource = realGraphTestingPoints,
                Title = "Test set approximation"
            });


            //GraphWindow realGw = new GraphWindow("Test function graph", "x", "y", realFunctionSerieses);
            GraphWindow gw = new GraphWindow("Function graph", "x", "y", functionSerieses);
            gw.Show();
            //realGw.Show();


            List<DataPoint> meanSquaredErrors = new List<DataPoint>();
            List<DataPoint> testMeanSE = new List<DataPoint>();
            //fill lists with error data points from each epoch
            for (int i = 0; i < Network.MeanSquaredErrors.Count; i++)
            {
                meanSquaredErrors.Add(new DataPoint(i,Network.MeanSquaredErrors[i]));
                testMeanSE.Add(new DataPoint(i, Network.TestMeanSquaredErrors[i]));
            }

            List<Series> errorSerieses = new List<Series>();
            errorSerieses.Add(new LineSeries()
            {
                Title = $"Training error={meanSquaredErrors.Sum(elem => elem.Y):N3}",
                ItemsSource = meanSquaredErrors
            });
            errorSerieses.Add(new LineSeries()
            {
                Title = $"Testing error={testMeanSE.Sum(e => e.Y):N3}",
                ItemsSource = testMeanSE
            });

            GraphWindow errorGraphWindow = new GraphWindow("Mean squared error by epoch", "epoch", "MSE", errorSerieses);
            errorGraphWindow.Show();
        }

        private Vector<double> classToVector(double clazz) {
            double[] values = new double[3];

            values[(int)Math.Ceiling(clazz) - 1] = 1.0;

            return Vector<double>.Build.DenseOfArray(values);
        }

        private int vectorToClass(Vector<double> output)
        {
            return output.MaximumIndex() + 1;
        }
    }
}

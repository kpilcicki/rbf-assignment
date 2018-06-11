using IadZad3.Model;
using IadZad3.Model.Model;
using IadZad3.Model.Utility;
using IadZad3.Model.Utility.ActivationFunctions;
using IadZad3.Model.Utility.DataUtility;
using IadZad3.Model.Utility.DistanceCalculator;
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
                    break;
                case "Classification":
                    taskSelected = TaskSelection.CLASS;
                    TrainingSetPath.Text = "classification.txt";
                    TestingSetPath = "classification_test.txt";
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

            DataGetter dg = new DataGetter();
            var trainingData = new List<TrainingSet>();
            switch (taskSelected)
            {
                case TaskSelection.APPROX:
                    trainingData = dg.GetTrainingDataWithOneOutput(trainingPath, inputCount);
                    break;
            }
            BackpropagationTrainingParameters parameters = new BackpropagationTrainingParameters(learningRate, epochs, momentum, minWeight, maxWeight,trainingData);

            Network.Train(parameters);

            


            List<DataPoint> graphTrainingPoints = new List<DataPoint>();
            List<DataPoint> graphTestingPoints = new List<DataPoint>();
            foreach(var set in trainingData.OrderBy(elem => elem.Input.At(0)))
            {
                var input = set.Input.At(0);
                graphTrainingPoints.Add(new DataPoint(input, set.DesiredOutput.At(0)));
                graphTestingPoints.Add(new DataPoint(input, Network.ProcessInput(set.Input).At(0)));
            }
            

            List<Series> functionSerieses = new List<Series>();

            functionSerieses.Add(new LineSeries()
            {
                ItemsSource = graphTrainingPoints,
                Title = "Training set"
            });

            functionSerieses.Add(new LineSeries(){
                ItemsSource = graphTestingPoints,
                Title = "Approximation"
            });            

            GraphWindow gw = new GraphWindow("Function graph", "x", "y", functionSerieses);
            gw.Show();


            List<DataPoint> meanSquaredErrors = new List<DataPoint>();
            int epoch = 0;
            foreach(double MSE in Network.MeanSquaredErrors)
            {
                meanSquaredErrors.Add(new DataPoint(++epoch, MSE));
            }

            List<Series> errorSerieses = new List<Series>();
            errorSerieses.Add(new LineSeries() {
                    Title = "Mean squared eror",
                    ItemsSource = meanSquaredErrors
            });

            GraphWindow errorGraphWindow = new GraphWindow("Mean squared error by epoch", "epoch", "MSE", errorSerieses);
            errorGraphWindow.Show();
        }
    }
}

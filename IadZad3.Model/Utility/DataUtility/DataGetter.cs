using IadZad3.Model.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IadZad3.Model.Utility.DataUtility
{
    public class DataGetter
    {
        public IEnumerable<List<double>> GetData(string fileName, char delimiter)
        {
            List<List<double>> data = new List<List<double>>();

            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    while((line = sr.ReadLine()) != null)
                    {
                        var row = line.Split(delimiter).Select(elem => Double.Parse(elem, CultureInfo.InvariantCulture)).ToList();
                        data.Add(row);
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine("The file couldn't be read");
                Console.WriteLine(e.Message);
            }

            return data;
        }

        public List<TrainingSet> GetTrainingDataWithOneOutput(string path, int numberOfInputs)
        {
            var data = GetData(path, ' ').ToList();
            List<TrainingSet> trainingData = new List<TrainingSet>();
            
            foreach(var set in data)
            {
                set.Count();
                trainingData.Add(new TrainingSet(set.Take(numberOfInputs).ToList(), 
                                                 new List<double> { set.Last() }));
            }

            return trainingData;
        }

        public List<TrainingSet> GetTrainingDataWithChosenInputs(string path, bool[] chosenInputs)
        {
            List<List<double>> data = GetData(path, ' ').ToList();
            List<TrainingSet> setData = new List<TrainingSet>();

            int numberOfInputs = 0;

            foreach (var bit in chosenInputs)
            {
                if (bit) numberOfInputs++;
            }


            foreach (var example in data)
            {
                var input = new double[numberOfInputs];
                var output = new double[1];
                int j = 0;
                for (int i = 0; i < chosenInputs.Length; i++)
                {
                    if (chosenInputs[i])
                    {
                        input[j] = example.ElementAt(i);
                        j++;
                    }
                }
                output[0] = example[example.Count - 1];
                setData.Add(new TrainingSet(input.ToList(), output.ToList()));
            }

            return setData;
        }
    }
}

using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace IadZad3.View
{
    /// <summary>
    /// Interaction logic for WindowDataGrid.xaml
    /// </summary>
    public partial class WindowDataGrid : Window
    {
        public WindowDataGrid(int[,] data)
        {
            InitializeComponent();

            int rozmiar = 3;
            List<Wrapper> dataGridsData = new List<Wrapper>();
            for (int i = 0; i < rozmiar; i++)
            {
                dataGridsData.Add(new Wrapper()
                {
                    RowName = "Actual " + (i + 1),
                    First = data[i, 0].ToString(),
                    Second = data[i, 1].ToString(),
                    Third = data[i, 2].ToString()
                });
            }

            Accuracy.Text = $"{Compute.GetAccuracy(data):N3}";


            Data.ItemsSource = dataGridsData;
            Data.CanUserAddRows = false;


        }
    }
}

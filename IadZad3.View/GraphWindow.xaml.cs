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
    /// Interaction logic for GraphWindow.xaml
    /// </summary>
    public partial class GraphWindow : Window
    {
        public string GraphName { get; set; }
        public string NameX { get; set; }
        public string NameY { get; set; }
        public GraphWindow(string name, string nameX, string nameY, List<OxyPlot.Wpf.Series> serieses)
        {
            InitializeComponent();

            GraphName = name;
            NameX = nameX;
            NameY = nameY;
            Plot.LegendTitleFontSize = 20;
            Plot.LegendFontSize = 27;

            foreach (var series in serieses)
            {
                if (series is OxyPlot.Wpf.LineSeries) ((OxyPlot.Wpf.LineSeries)series).StrokeThickness = 5;
                Plot.Series.Add(series);
            }
            DataContext = this;
        }
    }
}

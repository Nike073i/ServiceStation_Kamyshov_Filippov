using ServiceStationBusinessLogic.HelperModels;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace ServiceStationWorkerView
{
    /// <summary>
    /// Логика взаимодействия для ChartsWindow.xaml
    /// </summary>
    public partial class ChartsWindow : Window
    {
        public ReportInfoesWorker reportInfoes { get; set; }
        public ChartsWindow()
        {
            InitializeComponent();
        }
        private void StatisticsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (reportInfoes != null)
            {
                ((PieSeries)FrequentlyPassedChart.Series[0]).ItemsSource = reportInfoes.TotalCount;
                ((ColumnSeries)CountByMounthChart.Series[0]).ItemsSource = reportInfoes.CountByMounth;
            }
        }
    }
}

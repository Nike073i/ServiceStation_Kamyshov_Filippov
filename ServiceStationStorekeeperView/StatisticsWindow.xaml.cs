using ServiceStationBusinessLogic.HelperModels;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace ServiceStationStorekeeperView
{
    /// <summary>
    /// Логика взаимодействия для StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        public ReportInfoes ReportInfoes { get; set; }

        public StatisticsWindow()
        {
            InitializeComponent();
        }

        private void StatisticsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (ReportInfoes != null)
            {
                ((PieSeries)TotalCountChart.Series[0]).ItemsSource = ReportInfoes.TotalCount;
                ((ColumnSeries)CountByMounthChart.Series[0]).ItemsSource = ReportInfoes.CountByDates;
            }
        }
    }
}

using ServiceStationBusinessLogic.HelperModels;
using System.Collections.Generic;
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

        private void LoadData()
        {
            ((PieSeries)mcChart.Series[0]).ItemsSource = ReportInfoes.TotalCount;
            ((ColumnSeries)mcChart2.Series[0]).ItemsSource = ReportInfoes.CountByDates;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}

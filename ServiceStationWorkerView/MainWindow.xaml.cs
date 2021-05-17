using System.Windows;
using Unity;

namespace ServiceStationWorkerView
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonCars_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<CarsWindow>();
            form.ShowDialog();
        }

        private void ButtonServiceRecordings_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<ServiceRecordingsWindow>();
            form.ShowDialog();
        }

        private void ButtonTechnicalMaintenances_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<TehcnicalMaintenancesWindow>();
            form.ShowDialog();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            App.Worker = null;
            var authWindow = Container.Resolve<AuthorizationWindow>();
            Close();
            authWindow.ShowDialog();
        }
        private void ButtonReport_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<ReportTechnicalMaintenanceSparePartCars>();
            form.ShowDialog();
        }
        private void ButtonSparePartsList_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<SparePartListWindow>();
            form.ShowDialog();
        }
    }
}
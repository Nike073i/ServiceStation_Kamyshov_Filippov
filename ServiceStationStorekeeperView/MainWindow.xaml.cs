using System.Windows;
using Unity;

namespace ServiceStationStorekeeperView
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

        private void ButtonSpareParts_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<SparePartsWindow>();
            window.ShowDialog();
        }

        private void ButtonWorks_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WorksWindow>();
            window.ShowDialog();
        }

        private void ButtonWorkDurations_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WorkDurationsWindow>();
            window.ShowDialog();
        }
        private void ButtonReport_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<ReportSparePartsWindow>();
            window.ShowDialog();
        }

        private void ButtonListCarWork_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<ListCarWorkWindow>();
            window.ShowDialog();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            App.Storekeeper = null;
            var authWindow = Container.Resolve<AuthorizationWindow>();
            Close();
            authWindow.ShowDialog();
        }
    }
}

using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.BusinessLogic;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Windows;
using Unity;
using NLog;

namespace ServiceStationWorkerView
{
    /// <summary>
    /// Логика взаимодействия для TehcnicalMaintenancesWindow.xaml
    /// </summary>
    public partial class TehcnicalMaintenancesWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly TechnicalMaintenanceLogic logic;
        private readonly Logger logger;
        public TehcnicalMaintenancesWindow(TechnicalMaintenanceLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void TechnicalMaintenancesWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = logic.Read(new TechnicalMaintenanceBindingModel { UserId = App.Worker.Id });
                if (list != null)
                {
                    dataGridTechnicalMaintenances.ItemsSource = list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Ошибка загрузки данных: " + ex.Message);
            }
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<TehcnicalMaintenanceWindow>();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ButtonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridTechnicalMaintenances.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<TehcnicalMaintenanceWindow>();
                form.Id = ((TechnicalMaintenanceViewModel)dataGridTechnicalMaintenances.SelectedItems[0]).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridTechnicalMaintenances.SelectedItems.Count == 1)
            {
                MessageBoxResult result = (MessageBoxResult)MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int id = ((TechnicalMaintenanceViewModel)dataGridTechnicalMaintenances.SelectedItems[0]).Id;
                    try
                    {
                        logic.Delete(new TechnicalMaintenanceBindingModel { Id = id });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        logger.Error("Ошибка удаления ТО: " + ex.Message);
                    }
                    LoadData();
                }
            }
        }
        private void ButtonRef_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void ButtonLinkingCars_Click(object sende, RoutedEventArgs e)
        {
            var form = Container.Resolve<CarsTechicalMaintenanceWindow>();
            form.ShowDialog();
        }
    }
}

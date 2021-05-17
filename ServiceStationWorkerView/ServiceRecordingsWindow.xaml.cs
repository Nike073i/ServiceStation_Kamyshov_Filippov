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
    /// Логика взаимодействия для ServiceRecordingsWindow.xaml
    /// </summary>
    public partial class ServiceRecordingsWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly ServiceRecordingLogic logic;
        private readonly Logger logger;
        public ServiceRecordingsWindow(ServiceRecordingLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void ServiceRecordingsWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = logic.Read(new ServiceRecordingBindingModel { UserId = App.Worker.Id });
                if (list != null)
                {
                    dataGridServiceRecordings.ItemsSource = list;
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
            var form = Container.Resolve<ServiceRecordingWindow>();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ButtonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridServiceRecordings.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<ServiceRecordingWindow>();
                form.Id = ((ServiceRecordingViewModel)dataGridServiceRecordings.SelectedItems[0]).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridServiceRecordings.SelectedItems.Count == 1)
            {
                MessageBoxResult result = (MessageBoxResult)MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int id = ((ServiceRecordingViewModel)dataGridServiceRecordings.SelectedItems[0]).Id;
                    try
                    {
                        logic.Delete(new ServiceRecordingBindingModel { Id = id });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        logger.Error("Ошибка удаления записи сервисов: " + ex.Message);
                    }
                    LoadData();
                }
            }
        }
        private void ButtonRef_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}

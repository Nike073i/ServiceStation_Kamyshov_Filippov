using NLog;
using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.BusinessLogic;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Windows;
using Unity;

namespace ServiceStationStorekeeperView
{
    /// <summary>
    /// Логика взаимодействия для WorkDurationsWindow.xaml
    /// </summary>
    public partial class WorkDurationsWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly WorkDurationLogic logic;
        private readonly Logger logger;

        public WorkDurationsWindow(WorkDurationLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }

        private void WorkDurationsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = logic.Read(new WorkDurationBindingModel { UserId = App.Storekeeper.Id });
                if (list != null)
                {
                    dataGridWorkDurations.ItemsSource = list;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка загрузки данных : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<WorkDurationWindow>();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ButtonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridWorkDurations.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<WorkDurationWindow>();
                int id = (dataGridWorkDurations.SelectedItems[0] as WorkDurationViewModel).WorkId;
                form.Id = id;
                form.Duration = (dataGridWorkDurations.SelectedItems[0] as WorkDurationViewModel).Duration;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridWorkDurations.SelectedItems.Count == 1)
            {
                MessageBoxResult result = (MessageBoxResult)MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int id = (dataGridWorkDurations.SelectedItems[0] as WorkDurationViewModel).WorkId;
                    try
                    {
                        logic.Delete(new WorkDurationBindingModel { WorkId = id });
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ошибка удаления продолжительности : " + ex.Message);
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

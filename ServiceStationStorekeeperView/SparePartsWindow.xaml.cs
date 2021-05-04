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
    /// Логика взаимодействия для SparePartsWindow.xaml
    /// </summary>
    public partial class SparePartsWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly SparePartLogic logic;
        private readonly Logger logger;
        public SparePartsWindow(SparePartLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void SparePartsWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = logic.Read(new SparePartBindingModel { UserId = App.Storekeeper.Id });
                if (list != null)
                {
                    dataGridSpareParts.ItemsSource = list;
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
            var form = Container.Resolve<SparePartWindow>();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ButtonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridSpareParts.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<SparePartWindow>();
                form.Id = (dataGridSpareParts.SelectedItems[0] as SparePartViewModel).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridSpareParts.SelectedItems.Count == 1)
            {
                MessageBoxResult result = MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int id = (dataGridSpareParts.SelectedItems[0] as SparePartViewModel).Id;
                    try
                    {
                        logic.Delete(new SparePartBindingModel { Id = id });
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ошибка удаления запчасти : " + ex.Message);
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

        private void ButtonFastening_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<CarSparePartsWindow>();
            form.ShowDialog();
        }
    }
}

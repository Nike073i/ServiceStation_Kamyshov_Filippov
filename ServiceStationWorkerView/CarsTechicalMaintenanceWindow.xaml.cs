using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.BusinessLogic;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;
using NLog;

namespace ServiceStationWorkerView
{
    /// <summary>
    /// Логика взаимодействия для CarsTechicalMaintenanceWindow.xaml
    /// </summary>
    public partial class CarsTechicalMaintenanceWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly TechnicalMaintenanceLogic logicTM;
        private readonly CarLogic logicC;
        private TechnicalMaintenanceViewModel tmView;
        private Dictionary<int, string> currenttechnicalMaintenanceCars;
        private Dictionary<int, (string, int)> technicalMaintenanceWorks;
        private readonly Logger logger;
        public CarsTechicalMaintenanceWindow(TechnicalMaintenanceLogic logicTM, CarLogic logicC)
        {
            InitializeComponent();
            this.logicC = logicC;
            this.logicTM = logicTM;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void CarsTechnicalMaintenanceWindow_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                var listTechnicalMaintenances = logicTM.Read(new TechnicalMaintenanceBindingModel { UserId = App.Worker.Id });
                comboBoxTechnicalMaintenances.ItemsSource = listTechnicalMaintenances;
                comboBoxTechnicalMaintenances.SelectedItem = null;
                var listCar = logicC.Read(new CarBindingModel { UserId = App.Worker.Id });
                listBoxAvailableCars.ItemsSource = listCar;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void ReloadList()
        {
            listBoxCurrentCars.Items.Clear();
            foreach (var tmc in currenttechnicalMaintenanceCars)
            {
                listBoxCurrentCars.Items.Add(new CarViewModel { Id = tmc.Key, CarName = tmc.Value });
            }
        }

        private void LoadData()
        {
            try
            {
                TechnicalMaintenanceViewModel view = logicTM.Read(new TechnicalMaintenanceBindingModel
                {
                    Id = (int)comboBoxTechnicalMaintenances.SelectedValue
                })?[0];
                if (view != null)
                {
                    tmView = view;
                    currenttechnicalMaintenanceCars = view.TechnicalMaintenanceCars;
                    technicalMaintenanceWorks = view.TechnicalMaintenanceWorks;
                }
                else
                {
                    currenttechnicalMaintenanceCars = new Dictionary<int, string>();
                    technicalMaintenanceWorks = new Dictionary<int, (string, int)>();
                }
                ReloadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxTechnicalMaintenances.SelectedValue != null)
            {
                if (!currenttechnicalMaintenanceCars.ContainsKey((int)listBoxAvailableCars.SelectedValue))
                {
                    currenttechnicalMaintenanceCars.Add((int)listBoxAvailableCars.SelectedValue, ((CarViewModel)listBoxAvailableCars.SelectedItem).CarName);
                    ReloadList();
                }
            }
            else
            {
                MessageBox.Show("Выберите ТО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбрано ТО для привязки машин");
                return;
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxTechnicalMaintenances.SelectedValue != null)
            {
                if (listBoxCurrentCars.SelectedItems.Count == 1)
                {
                    MessageBoxResult result = (MessageBoxResult)MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            currenttechnicalMaintenanceCars.Remove((int)listBoxCurrentCars.SelectedValue);
                            ReloadList();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            logger.Error("Ошибка удаления привязки машин к ТО: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите ТО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбрано ТО для привязки машин");
                return;
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxTechnicalMaintenances.SelectedValue == null)
            {
                MessageBox.Show("Выберите ТО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбрано ТО для привязки машин");
                return;
            }
            try
            {
                logicTM.CreateOrUpdate(new TechnicalMaintenanceBindingModel
                {
                    Id = tmView.Id,
                    TechnicalMaintenanceName = tmView.TechnicalMaintenanceName,
                    UserId = tmView.UserId,
                    Sum = tmView.Sum,
                    TechnicalMaintenanceWorks = technicalMaintenanceWorks,
                    TechnicalMaintenanceCars = currenttechnicalMaintenanceCars,
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Ошибка сохранения привязки машин к ТО: " + ex.Message);
            }
        }

        private void SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadData();
        }
    }
}
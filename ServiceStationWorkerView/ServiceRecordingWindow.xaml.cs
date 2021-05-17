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
    /// Логика взаимодействия для ServiceRecordingWindow.xaml
    /// </summary>
    public partial class ServiceRecordingWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly CarLogic logicC;
        private readonly TechnicalMaintenanceLogic logicTM;
        private readonly ServiceRecordingLogic logic;
        private int? id;
        private readonly Logger logger;

        public ServiceRecordingWindow(TechnicalMaintenanceLogic logicTM, CarLogic logicC, ServiceRecordingLogic logic)
        {
            InitializeComponent();
            this.logicTM = logicTM;
            this.logicC = logicC;
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void ServiceRecordingWindow_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                var carList = logicC.Read(null);
                carComboBox.ItemsSource = carList;

                if (id.HasValue)
                {
                    var serviceRecording = logic.Read(new ServiceRecordingBindingModel
                    {
                        Id = id
                    })?[0];
                    if (serviceRecording != null)
                    {
                        carComboBox.SelectedValue = serviceRecording.CarId;
                        technicalMaintenanceComboBox.SelectedValue = serviceRecording.TechnicalMaintenanceId;
                        dateCarPass.SelectedDate = serviceRecording.DatePassed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Ошибка загрузки данных: " + ex.Message);
            }
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (dateCarPass.SelectedDate == null)
            {
                MessageBox.Show("Заполните дату прохождения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнена дата прохождения ТО");
                return;
            }
            if (carComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите машину", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбрана машина для записи сервисов");
                return;
            }
            if (technicalMaintenanceComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите ТО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбрано ТО для записи сервисов");
                return;
            }
            try
            {
                logic.CreateOrUpdate(new ServiceRecordingBindingModel
                {
                    Id = id,
                    DatePassed = (DateTime)dateCarPass.SelectedDate,
                    UserId = App.Worker.Id,
                    CarId = (int)carComboBox.SelectedValue,
                    TechnicalMaintenanceId = (int)technicalMaintenanceComboBox.SelectedValue,
                });

                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Ошибка сохранения записи сервисов: " + ex.Message);
            }
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void LoadData()
        {
            try
            {
                var tmList = logicTM.Read(null);
                var car = logicC.Read(new CarBindingModel
                {
                    Id = (int)carComboBox.SelectedValue
                })?[0];
                var tmCarList = new List<TechnicalMaintenanceViewModel>();
                if (car != null)
                {
                    foreach (var tm in tmList)
                    {
                        if (tm.TechnicalMaintenanceCars.ContainsKey(car.Id))
                        {
                            tmCarList.Add(new TechnicalMaintenanceViewModel
                            {
                                Id = tm.Id,
                                TechnicalMaintenanceName = tm.TechnicalMaintenanceName
                            });
                        }
                    }
                }
                technicalMaintenanceComboBox.ItemsSource = tmCarList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void CarsSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadData();
        }
    }
}

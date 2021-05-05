using Microsoft.Win32;
using NLog;
using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.BusinessLogic;
using System;
using System.Windows;
using Unity;

namespace ServiceStationWorkerView
{
    /// <summary>
    /// Логика взаимодействия для ReportTechnicalMaintenanceSparePartCars.xaml
    /// </summary>
    public partial class ReportTechnicalMaintenanceSparePartCars : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly ReportLogicWorker logic;
        private readonly Logger logger;
        public ReportTechnicalMaintenanceSparePartCars(ReportLogicWorker logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void ButtonMake_Click(object sender, RoutedEventArgs e)
        {
            if (DatePikerTo.SelectedDate == null || DatePikerFrom.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбраны даты для отчета");
                return;
            }
            if (DatePikerFrom.SelectedDate >= DatePikerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Выбранная дата начала больше, чем дата окончания");
                return;
            }
            try
            {
                var dataSource = logic.GetSparePartTechnicalMaintenanceCar(new ReportWorkerBindingModel
                {
                    DateFrom = DatePikerFrom.SelectedDate,
                    DateTo = DatePikerTo.SelectedDate
                });
                dataGridTechnicalMaintenanceSparePartCars.ItemsSource = dataSource;
                textBoxDateFrom.Content = DatePikerFrom.SelectedDate.Value.ToLongDateString();
                textBoxDateTo.Content = DatePikerTo.SelectedDate.Value.ToLongDateString();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка формирования отчета: " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }

        private void ButtonToPdf_Click(object sender, RoutedEventArgs e)
        {
            if (DatePikerTo.SelectedDate == null || DatePikerFrom.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбраны даты для отчета");
                return;
            }

            if (DatePikerFrom.SelectedDate >= DatePikerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Выбранная дата начала больше, чем дата окончания");
                return;
            }
            var dialog = new SaveFileDialog { Filter = "pdf|*.pdf" };
            {
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        logic.SaveSparePartsToPdfFile(new ReportWorkerBindingModel
                        {
                            FileName = dialog.FileName,
                            DateFrom = DatePikerFrom.SelectedDate,
                            DateTo = DatePikerTo.SelectedDate
                        });
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
                       MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ошибка сохранения отчета: " + ex.Message);
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                       MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ButtonPDFToEmail_Click(object sender, RoutedEventArgs e)
        {
            if (DatePikerTo.SelectedDate == null || DatePikerFrom.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбраны даты для отчета");
                return;
            }

            if (DatePikerFrom.SelectedDate >= DatePikerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Выбранная дата начала больше, чем дата окончания");
                return;
            }
            try
            {
                var fileName = "Отчет.pdf";
                var subject = "СТО Руки-Крюки. Отчет по ТО от " + DatePikerFrom.SelectedDate + " по " + DatePikerTo;
                logic.SaveSparePartsToPdfFile(new ReportWorkerBindingModel
                {
                    FileName = fileName,
                    DateFrom = DatePikerFrom.SelectedDate,
                    DateTo = DatePikerTo.SelectedDate
                });
                logic.SendMail(App.Worker.Email, fileName, subject);
                MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка сохранения отчета: " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
        }
    }
}

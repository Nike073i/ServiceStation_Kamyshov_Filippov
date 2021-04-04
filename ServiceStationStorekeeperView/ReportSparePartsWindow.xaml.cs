using Microsoft.Win32;
using NLog;
using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.BusinessLogic;
using System;
using System.Windows;
using Unity;

namespace ServiceStationStorekeeperView
{
    /// <summary>
    /// Логика взаимодействия для ReportSparePartsWindow.xaml
    /// </summary>
    public partial class ReportSparePartsWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly ReportLogicStorekeeper logic;
        private readonly Logger logger;
        public ReportSparePartsWindow(ReportLogicStorekeeper logic)
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
                return;
            }
            if (DatePikerFrom.SelectedDate >= DatePikerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var dataSource = logic.GetSparePartWorkCar(new ReportStorekeeperBindingModel
                {
                    DateFrom = DatePikerFrom.SelectedDate,
                    DateTo = DatePikerTo.SelectedDate
                });
                dataGridSpareParts.ItemsSource = dataSource;
                textBoxDateFrom.Content = DatePikerFrom.SelectedDate.Value.ToLongDateString();
                textBoxDateTo.Content = DatePikerTo.SelectedDate.Value.ToLongDateString();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка формирования отчета : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }

        private void ButtonToPdf_Click(object sender, RoutedEventArgs e)
        {
            if (DatePikerTo.SelectedDate == null || DatePikerFrom.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DatePikerFrom.SelectedDate >= DatePikerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var dialog = new SaveFileDialog { Filter = "pdf|*.pdf" };
            {
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        logic.SaveSparePartsToPdfFile(new ReportStorekeeperBindingModel
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
                        logger.Error("Ошибка сохранения отчета : " + ex.Message);
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                       MessageBoxImage.Error);
                    }
                }
            }

        }

        private void ButtonPDFToEmail_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

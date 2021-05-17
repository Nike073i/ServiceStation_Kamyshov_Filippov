using Microsoft.Win32;
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
    /// Логика взаимодействия для SparePartsListWindow.xaml
    /// </summary>
    public partial class SparePartListWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly TechnicalMaintenanceLogic logicTM;
        private readonly ReportLogicWorker logicRW;
        private readonly Logger logger;
        public SparePartListWindow(TechnicalMaintenanceLogic logicTM, ReportLogicWorker logicRW)
        {
            InitializeComponent();
            this.logicTM = logicTM;
            this.logicRW = logicRW;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void SparePartsListWindowLoad(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = logicTM.Read(new TechnicalMaintenanceBindingModel { UserId = App.Worker.Id });
                if (list != null)
                {
                    dataGridTechnicalMaintenances.ItemsSource = list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Question);
                logger.Error("Ошибка загрузки данных: " + ex.Message);
            }
        }
        private void ButtonSaveToWord_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridTechnicalMaintenances.SelectedItem == null || dataGridTechnicalMaintenances.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите TO", "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                logger.Warn("Не выбрано ТО для отчета");
                return;
            }
            var dialog = new SaveFileDialog { Filter = "docx|*.docx" };
            try
            {
                if (dialog.ShowDialog() == true)
                {
                    var list = new List<TechnicalMaintenanceViewModel>();
                    foreach (var tm in dataGridTechnicalMaintenances.SelectedItems)
                    {
                        list.Add((TechnicalMaintenanceViewModel)tm);
                    }
                    logicRW.SaveTechnicalMaintenanceSparePartsToWordFile(new ReportWorkerBindingModel
                    {
                        FileName = dialog.FileName,
                        TechnicalMaintenances = list
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                logger.Error("Ошибка сохранения отчета в Word: " + ex.Message);
            }
        }
        private void ButtonSaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridTechnicalMaintenances.SelectedItem == null || dataGridTechnicalMaintenances.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите TO", "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                logger.Warn("Не выбрано ТО для отчета");
                return;
            }
            var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" };
            try
            {
                if (dialog.ShowDialog() == true)
                {
                    var list = new List<TechnicalMaintenanceViewModel>();
                    foreach (var tm in dataGridTechnicalMaintenances.SelectedItems)
                    {
                        list.Add((TechnicalMaintenanceViewModel)tm);
                    }
                    logicRW.SaveTechnicalMaintenanceSparePartsToExcelFile(new ReportWorkerBindingModel
                    {
                        FileName = dialog.FileName,
                        TechnicalMaintenances = list
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                logger.Error("Ошибка сохранения отчета в Excel: " + ex.Message);
            }
        }
    }
}

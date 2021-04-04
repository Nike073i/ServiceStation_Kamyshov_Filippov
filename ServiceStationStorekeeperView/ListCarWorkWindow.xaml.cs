using Microsoft.Win32;
using NLog;
using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.BusinessLogic;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;

namespace ServiceStationStorekeeperView
{
    /// <summary>
    /// Логика взаимодействия для ListCarWorkWindow.xaml
    /// </summary>
    public partial class ListCarWorkWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly WorkLogic logicW;
        private readonly ReportLogicStorekeeper logicR;
        private readonly Logger logger;

        public ListCarWorkWindow(WorkLogic logicW, ReportLogicStorekeeper logicR)
        {
            InitializeComponent();
            this.logicW = logicW;
            this.logicR = logicR;
            logger = LogManager.GetCurrentClassLogger();
        }

        private void WorksWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var list = logicW.Read(null);
                if (list != null)
                {
                    dataGridWorks.ItemsSource = list;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка загрузки данных : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }

        private void ButtonSaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridWorks.SelectedItem == null || dataGridWorks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите работу", "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                return;
            }
            SaveFileDialog dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" };
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var works = new List<WorkViewModel>();
                    foreach (var work in dataGridWorks.SelectedItems)
                    {
                        works.Add(work as WorkViewModel);
                    }
                    logicR.SaveWorkCarsToExcelFile(new ReportStorekeeperBindingModel
                    {
                        FileName = dialog.FileName,
                        Works = works
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    logger.Error("Ошибка формирования Excel файла : " + ex.Message);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                }
            }
        }

        private void ButtonSaveToWord_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridWorks.SelectedItem == null || dataGridWorks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите работу", "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                return;
            }
            var dialog = new SaveFileDialog { Filter = "docx|*.docx" };
            try
            {
                if (dialog.ShowDialog() == true)
                {
                    var list = new List<WorkViewModel>();
                    foreach (var work in dataGridWorks.SelectedItems)
                    {
                        list.Add((WorkViewModel)work);
                    }
                    logicR.SaveWorkCarsToWordFile(new ReportStorekeeperBindingModel
                    {
                        FileName = dialog.FileName,
                        Works = list
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка формирования Word файла : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }
    }
}

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
    /// Логика взаимодействия для TehcnicalMaintenanceWork.xaml
    /// </summary>
    public partial class TehcnicalMaintenanceWorkWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id
        {
            get { return (int)workComboBox.SelectedValue; }
            set { workComboBox.SelectedValue = value; }
        }
        public string WorkName { get { return workComboBox.Text; } }
        public int Count
        {
            get { return Convert.ToInt32(workCountTextBox.Text); }
            set
            {
                workCountTextBox.Text = value.ToString();
            }
        }
        private readonly Logger logger;
        public TehcnicalMaintenanceWorkWindow(WorkLogic logic)
        {
            InitializeComponent();
            List<WorkViewModel> list = logic.Read(null);
            if (list != null)
            {
                workComboBox.ItemsSource = list;
                workComboBox.SelectedItem = null;
            }
            logger = LogManager.GetCurrentClassLogger();
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(workCountTextBox.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнено поле Количество при добавлении работы в ТО");
                return;
            }
            if (workComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите работу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбрана работа при добавлении работы в ТО");
                return;
            }
            DialogResult = true;
            Close();
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}

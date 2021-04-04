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
    /// Логика взаимодействия для WorkDurationWindow.xaml
    /// </summary>
    public partial class WorkDurationWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id
        {
            set { comboBoxWork.SelectedValue = value; }
        }
        public int Duration
        {
            set
            {
                textBoxDuration.Text = value.ToString();
            }
        }

        private readonly Logger logger;
        private readonly WorkDurationLogic logicWD;
        public WorkDurationWindow(WorkDurationLogic logicWD, WorkLogic logicW)
        {
            logger = LogManager.GetCurrentClassLogger();
            InitializeComponent();
            try
            {
                List<WorkViewModel> list = logicW.Read(null);
                if (list != null)
                {
                    comboBoxWork.ItemsSource = list;
                    comboBoxWork.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка загрузки данных : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.logicWD = logicWD;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxDuration.Text))
            {
                MessageBox.Show("Заполните поле Продолжительность", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxWork.SelectedValue == null)
            {
                MessageBox.Show("Выберите работу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logicWD.CreateOrUpdate(new WorkDurationBindingModel
                {
                    WorkId = (int)comboBoxWork.SelectedValue,
                    Duration = Convert.ToInt32(textBoxDuration.Text),
                    UserId = App.Storekeeper.Id
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка сохранения продолжительности работы: " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}

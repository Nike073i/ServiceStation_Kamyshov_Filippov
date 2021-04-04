using NLog;
using ServiceStationBusinessLogic.BusinessLogic;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;

namespace ServiceStationStorekeeperView
{
    /// <summary>
    /// Логика взаимодействия для SparePartWorkWindow.xaml
    /// </summary>
    public partial class SparePartWorkWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly Logger logger;
        public int Id
        {
            get { return (int)comboBoxSparePart.SelectedValue; }
            set { comboBoxSparePart.SelectedValue = value; }
        }
        public string SparePartName { get { return comboBoxSparePart.Text; } }
        public int Count
        {
            get
            {
                try
                {
                    return Convert.ToInt32(textBoxCount.Text);
                }
                catch (Exception ex)
                {
                    logger.Error("Ошибка конвертации данных : " + ex.Message);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return 0;
                }
            }
            set
            {
                textBoxCount.Text = value.ToString();
            }
        }
        public SparePartWorkWindow(SparePartLogic logic)
        {
            logger = LogManager.GetCurrentClassLogger();
            InitializeComponent();
            try
            {
                List<SparePartViewModel> list = logic.Read(null);
                if (list != null)
                {
                    comboBoxSparePart.ItemsSource = list;
                    comboBoxSparePart.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка загрузки данных : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxSparePart.SelectedValue == null)
            {
                MessageBox.Show("Выберите запчасть", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

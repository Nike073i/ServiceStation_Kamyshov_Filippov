using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.BusinessLogic;
using System.Collections.Generic;
using System;
using System.Windows;
using Unity;
using NLog;

namespace ServiceStationWorkerView
{
    /// <summary>
    /// Логика взаимодействия для CarWindow.xaml
    /// </summary>
    public partial class CarWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly CarLogic logic;
        private int? id;
        private Dictionary<int, string> carSpareParts;
        private readonly Logger logger;
        public CarWindow(CarLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();

        }
        private void CarWindow_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new CarBindingModel { Id = id })?[0];
                    if (view != null)
                    {
                        textBoxCarName.Text = view.CarName;
                        carSpareParts = view.CarSpareParts;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    logger.Error("Ошибка загрузки данных: " + ex.Message);
                }
            }
            else
            {
                carSpareParts = new Dictionary<int, string>();
            }
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCarName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнено название машины для создания");
                return;
            }
            try
            {
                logic.CreateOrUpdate(new CarBindingModel
                {
                    Id = id,
                    CarName = textBoxCarName.Text,
                    UserId = App.Worker.Id,
                    CarSpareParts = carSpareParts  
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Ошибка сохранения машины: " + ex.Message);
            }
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}

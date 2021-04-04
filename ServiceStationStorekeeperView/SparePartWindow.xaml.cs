using NLog;
using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.BusinessLogic;
using System;
using System.Windows;
using Unity;

namespace ServiceStationStorekeeperView
{
    /// <summary>
    /// Логика взаимодействия для SparePartWindow.xaml
    /// </summary>
    public partial class SparePartWindow : Window
    {

        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly SparePartLogic logic;
        private int? id;
        private readonly Logger logger;

        public SparePartWindow(SparePartLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }

        private void SparePartWindow_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new SparePartBindingModel { Id = id })?[0];
                    if (view != null)
                    {
                        textBoxSparePartName.Text = view.SparePartName;
                        textBoxSparePartPrice.Text = view.Price.ToString();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Ошибка загрузки данных : " + ex.Message);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxSparePartName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new SparePartBindingModel
                {
                    Id = id,
                    SparePartName = textBoxSparePartName.Text,
                    Price = Convert.ToDecimal(textBoxSparePartPrice.Text),
                    UserId = App.Storekeeper.Id
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка сохранение запчасти : " + ex.Message);
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

using NLog;
using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.BusinessLogic;
using ServiceStationBusinessLogic.Enums;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using Unity;

namespace ServiceStationWorkerView
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly UserLogic logic;
        private readonly Logger logger;
        private int passwordMinLength = 5;
        private int passwordMaxLength = 15;
        public RegistrationWindow(UserLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Заполните \"ФИО\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнено поле ФИО при регистрации");
                return;
            }
            if (comboBoxPosition.SelectedValue == null)
            {
                MessageBox.Show("Выберите \"должность\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбрана должность сотрудника при регистрации");
                return;
            }
            if (string.IsNullOrEmpty(textBoxEmail.Text))
            {
                MessageBox.Show("Заполните поле \"Почта\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнено поле Почта при регистрации");
                return;
            }
            if (!Regex.IsMatch(textBoxEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                MessageBox.Show("Почта введена некорректно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Введена некорректная почта при регистрации");
                return;
            }
            if (string.IsNullOrEmpty(textBoxPassword.Text))
            {
                MessageBox.Show("Заполните поле \"пароль\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнено поле Пароль при регистрации");
                return;
            }
            if (textBoxPassword.Text.Length > passwordMaxLength || textBoxPassword.Text.Length < passwordMinLength)
            {
                MessageBox.Show($"Пароль должен быть длиной от {passwordMinLength} до {passwordMaxLength}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new UserBindingModel
                {
                    FIO = textBoxFIO.Text,
                    Position = (UserPosition)Enum.Parse(typeof(UserPosition), comboBoxPosition.SelectedValue.ToString()),
                    Email = textBoxEmail.Text,
                    Password = textBoxPassword.Text
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка сохранения пользователя: " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void RegistrationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxPosition.Items.Clear();
            foreach (string position in Enum.GetNames(typeof(UserPosition)))
            {
                comboBoxPosition.Items.Add(position);
            }
        }
    }
}

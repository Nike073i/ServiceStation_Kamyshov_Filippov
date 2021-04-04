using NLog;
using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.BusinessLogic;
using ServiceStationBusinessLogic.Enums;
using System;
using System.Windows;
using Unity;

namespace ServiceStationWorkerView
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly UserLogic logic;
        private readonly Logger logger;
        public AuthorizationWindow(UserLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxEmail.Text))
            {
                MessageBox.Show("Введите почту", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                logger.Warn("Не введена почта при авторизации");
                return;
            }
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                logger.Warn("Не введен пароль при авторизации");
                return;
            }
            try
            {
                var users = logic.Read(new UserBindingModel
                {
                    Email = textBoxEmail.Text,
                    Password = passwordBox.Password
                });
                if (users != null && users.Count > 0)
                {
                    var curUser = users[0];
                    if (curUser.Position == UserPosition.Работник)
                    {
                        App.Worker = curUser;
                        var MainWindow = Container.Resolve<MainWindow>();
                        MainWindow.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не является работником", "Ошибка", MessageBoxButton.OK,
                       MessageBoxImage.Error);
                        logger.Warn("Авторизация под другой должностью");
                    }
                }
                else
                {
                    MessageBox.Show("Неверно введен пароль или логин", "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                    logger.Warn("Неверные данные при авторизации");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка загрузки данных: " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<RegistrationWindow>();
            form.ShowDialog();
        }
    }
}

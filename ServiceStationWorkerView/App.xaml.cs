using ServiceStationBusinessLogic.BusinessLogic;
using ServiceStationBusinessLogic.HelperModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using ServiceStationDatabaseImplement.Implements;
using System;
using System.Configuration;
using System.Windows;
using Unity;
using Unity.Lifetime;

namespace ServiceStationWorkerView
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UserViewModel Worker { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var container = BuildUnityContainer();
            MailLogic.MailConfig(new MailConfig
            {
                SmtpClientHost = ConfigurationManager.AppSettings["SmtpClientHost"],
                SmtpClientPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpClientPort"]),
                MailLogin = ConfigurationManager.AppSettings["MailLogin"],
                MailPassword = ConfigurationManager.AppSettings["MailPassword"],
                MailName = ConfigurationManager.AppSettings["MailName"]
            });
            var authWindow = container.Resolve<AuthorizationWindow>();
            authWindow.ShowDialog();
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<ITechnicalMaintenanceStorage, TechnicalMaintenanceStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICarStorage, CarStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IServiceRecordingStorage, ServiceRecordingStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IUserStorage, UserStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWorkStorage, WorkStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ISparePartStorage, SparePartStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWorkDurationStorage, WorkDurationStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<UserLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<CarLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<TechnicalMaintenanceLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ServiceRecordingLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<WorkLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<SparePartLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<WorkDurationLogic>(new HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}
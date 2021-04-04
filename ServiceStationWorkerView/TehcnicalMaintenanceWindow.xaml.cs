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
    public class GridTechnicalMaintenanceWork
    {
        public int Id { get; set; }
        public string WorkName { get; set; }
        public decimal? Price { get; set; }
        public int Count { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для TehcnicalMaintenanceWindow.xaml
    /// </summary>
    public partial class TehcnicalMaintenanceWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly WorkLogic logicW;
        private readonly TechnicalMaintenanceLogic logicTM;
        private int? id;
        private Dictionary<int, (string, int)> technicalMaintenanceWorks;
        private Dictionary<int, string> technicalMaintenanceCars;
        private readonly Logger logger;
        public TehcnicalMaintenanceWindow(TechnicalMaintenanceLogic logicTM, WorkLogic logicW)
        {
            InitializeComponent();
            this.logicTM = logicTM;
            this.logicW = logicW;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void TechnicalMaintenanceWindow_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    TechnicalMaintenanceViewModel view = logicTM.Read(new TechnicalMaintenanceBindingModel
                    {
                        Id = id.Value
                    })?[0];
                    if (view != null)
                    {
                        textBoxTechnicalMaintenanceName.Text = view.TechnicalMaintenanceName;
                        textBoxTechnicalMaintenanceSum.Text = view.Sum.ToString();
                        technicalMaintenanceWorks = view.TechnicalMaintenanceWorks;
                        technicalMaintenanceCars = view.TechnicalMaintenanceCars;
                        LoadData();
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
                technicalMaintenanceWorks = new Dictionary<int, (string, int)>();
                technicalMaintenanceCars = new Dictionary<int, string>();
            }
        }
        private void CalcSum()
        {
            decimal sum = 0;
            if (technicalMaintenanceWorks != null)
            {
                foreach (var technicalMaintenanceWork in technicalMaintenanceWorks)
                {
                    var work = logicW.Read(new WorkBindingModel
                    {
                        Id = technicalMaintenanceWork.Key
                    })?[0];
                    sum += (work?.Price ?? 0) * technicalMaintenanceWork.Value.Item2;
                }
            }
            textBoxTechnicalMaintenanceSum.Text = sum.ToString();
        }

        private void LoadData()
        {
            try
            {
                if (technicalMaintenanceWorks != null)
                {
                    dataGridWorks.Items.Clear();
                    foreach (var tmw in technicalMaintenanceWorks)
                    {
                        var list = logicW.Read(new WorkBindingModel
                        {
                            Id = tmw.Key
                        })?[0];
                        dataGridWorks.Items.Add(new GridTechnicalMaintenanceWork
                        {
                            Id = tmw.Key,
                            WorkName = tmw.Value.Item1,
                            Price = list?.Price,
                            Count = tmw.Value.Item2
                        });
                    }
                }
                CalcSum();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<TehcnicalMaintenanceWorkWindow>();
            if (form.ShowDialog() == true)
            {
                if (technicalMaintenanceWorks.ContainsKey(form.Id))
                {
                    technicalMaintenanceWorks[form.Id] = (form.WorkName, form.Count);
                }
                else
                {
                    technicalMaintenanceWorks.Add(form.Id, (form.WorkName, form.Count));
                }
                LoadData();
            }
        }
        private void ButtonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridWorks.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<TehcnicalMaintenanceWorkWindow>();
                int id = ((GridTechnicalMaintenanceWork)dataGridWorks.SelectedItems[0]).Id;
                form.Id = id;
                form.Count = technicalMaintenanceWorks[id].Item2;
                if (form.ShowDialog() == true)
                {
                    technicalMaintenanceWorks[form.Id] = (form.WorkName, form.Count);
                    LoadData();
                }
            }
        }
        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridWorks.SelectedItems.Count == 1)
            {
                MessageBoxResult result = (MessageBoxResult)MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        technicalMaintenanceWorks.Remove(((GridTechnicalMaintenanceWork)dataGridWorks.SelectedItems[0]).Id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        logger.Error("Ошибка удаления работы из ТО: " + ex.Message);
                    }
                }
                LoadData();
            }
        }
        private void ButtonRef_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxTechnicalMaintenanceName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнено название ТО при создании");
                return;
            }
            if (technicalMaintenanceWorks == null || technicalMaintenanceWorks.Count == 0)
            {
                MessageBox.Show("Заполните работы", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                logger.Warn("Не добавлены работы в ТО при создании");
                return;
            }
            try
            {
                logicTM.CreateOrUpdate(new TechnicalMaintenanceBindingModel
                {
                    Id = id,
                    TechnicalMaintenanceName = textBoxTechnicalMaintenanceName.Text,
                    Sum = Convert.ToDecimal(textBoxTechnicalMaintenanceSum.Text),
                    TechnicalMaintenanceWorks = technicalMaintenanceWorks,
                    TechnicalMaintenanceCars = technicalMaintenanceCars,
                    UserId = App.Worker.Id
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error("Ошибка сохранения ТО: " + ex.Message);
            }
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}

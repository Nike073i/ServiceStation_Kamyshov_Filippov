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
    /// Логика взаимодействия для WorkWindow.xaml
    /// </summary>
    public class GridWorkSparePart
    {
        public int Id { get; set; }
        public string SparePartName { get; set; }
        public int Count { get; set; }
    }
    public partial class WorkWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly Logger logger;
        public int Id { set { id = value; } }
        private readonly WorkLogic logicW;
        private readonly SparePartLogic logicS;
        private int? id;
        private Dictionary<int, (string, int)> workSpareParts;

        public WorkWindow(WorkLogic logicW, SparePartLogic logicS)
        {
            InitializeComponent();
            this.logicW = logicW;
            this.logicS = logicS;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void WorkWindow_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    WorkViewModel view = logicW.Read(new WorkBindingModel
                    {
                        Id = id.Value
                    })?[0];
                    if (view != null)
                    {
                        textBoxWorkName.Text = view.WorkName;
                        textBoxWorkPrice.Text = view.Price.ToString();
                        workSpareParts = view.WorkSpareParts;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Ошибка загрузки окна изменения работы : " + ex.Message);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                workSpareParts = new Dictionary<int, (string, int)>();
            }
        }
        private void CalcSparePartsSum()
        {
            decimal sparePartsSum = 0;
            if (workSpareParts != null)
            {
                foreach (var workSparePart in workSpareParts)
                {
                    var sparePart = logicS.Read(new SparePartBindingModel
                    {
                        Id = workSparePart.Key
                    })?[0];
                    sparePartsSum += (sparePart?.Price ?? 0) * workSparePart.Value.Item2;
                }
            }
            labelSparePartsSum.Content = sparePartsSum;
        }

        private void LoadData()
        {
            try
            {
                if (workSpareParts != null)
                {
                    dataGridWork.Items.Clear();
                    foreach (var wsp in workSpareParts)
                    {
                        dataGridWork.Items.Add(new GridWorkSparePart
                        {
                            Id = wsp.Key,
                            SparePartName = wsp.Value.Item1,
                            Count = wsp.Value.Item2
                        });
                    }
                }
                CalcSparePartsSum();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка загрузки данных : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<SparePartWorkWindow>();
            if (form.ShowDialog() == true)
            {
                if (workSpareParts.ContainsKey(form.Id))
                {
                    workSpareParts[form.Id] = (form.SparePartName, form.Count);
                }
                else
                {
                    workSpareParts.Add(form.Id, (form.SparePartName, form.Count));
                }
                LoadData();
            }
        }
        private void ButtonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridWork.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<SparePartWorkWindow>();
                int id = ((GridWorkSparePart)dataGridWork.SelectedItems[0]).Id;
                form.Id = id;
                form.Count = workSpareParts[id].Item2;
                if (form.ShowDialog() == true)
                {
                    workSpareParts[form.Id] = (form.SparePartName, form.Count);
                    LoadData();
                }
            }
        }
        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridWork.SelectedItems.Count == 1)
            {
                MessageBoxResult result = MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        workSpareParts.Remove(((GridWorkSparePart)dataGridWork.SelectedItems[0]).Id);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ошибка удаления запчасти : " + ex.Message);
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }
        private void ButtonRef_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxWorkName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxWorkPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (Convert.ToDecimal(textBoxWorkPrice.Text) < Convert.ToDecimal(labelSparePartsSum.Content))
                {
                    MessageBox.Show("Цена не может быть ниже суммы запчастей", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                logicW.CreateOrUpdate(new WorkBindingModel
                {
                    Id = id,
                    WorkName = textBoxWorkName.Text,
                    Price = Convert.ToDecimal(textBoxWorkPrice.Text),
                    WorkSpareParts = workSpareParts,
                    UserId = App.Storekeeper.Id
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка сохранения работы : " + ex.Message);
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

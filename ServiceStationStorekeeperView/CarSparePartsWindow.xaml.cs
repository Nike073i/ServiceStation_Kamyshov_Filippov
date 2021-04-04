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
    /// Логика взаимодействия для CarSparePartsWindow.xaml
    /// </summary>
    public partial class CarSparePartsWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly CarLogic logicC;
        private readonly SparePartLogic logicS;
        private readonly Logger logger;
        private CarViewModel carView;
        private Dictionary<int, string> currentCarSpareParts;

        public CarSparePartsWindow(CarLogic logicC, SparePartLogic logicS)
        {
            InitializeComponent();
            this.logicC = logicC;
            this.logicS = logicS;
            logger = LogManager.GetCurrentClassLogger();
        }

        private void CarSparePartsWindow_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                var listCars = logicC.Read(null);
                comboBoxCars.ItemsSource = listCars;
                comboBoxCars.SelectedItem = null;
                var list = logicS.Read(null);
                listBoxAllSpareParts.ItemsSource = list;
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка загрузки данных : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReloadList()
        {
            listBoxCurrentSpareParts.Items.Clear();
            foreach (var cSP in currentCarSpareParts)
            {
                listBoxCurrentSpareParts.Items.Add(new SparePartViewModel { Id = cSP.Key, SparePartName = cSP.Value });
            }
        }

        private void LoadData()
        {
            try
            {
                CarViewModel view = logicC.Read(new CarBindingModel
                {
                    Id = (int)comboBoxCars.SelectedValue
                })?[0];
                if (view != null)
                {
                    carView = view;
                    currentCarSpareParts = view.CarSpareParts;
                }
                else
                {
                    currentCarSpareParts = new Dictionary<int, string>();
                }
                ReloadList();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка загрузки данных машины : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxCars.SelectedValue != null)
            {
                if (!currentCarSpareParts.ContainsKey((int)listBoxAllSpareParts.SelectedValue))
                {
                    currentCarSpareParts.Add((int)listBoxAllSpareParts.SelectedValue, (listBoxAllSpareParts.SelectedItem as SparePartViewModel).SparePartName);
                    ReloadList();
                }
            }
            else
            {
                MessageBox.Show("Выберите машину", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxCars.SelectedValue != null)
            {
                if (listBoxCurrentSpareParts.SelectedItems.Count == 1)
                {
                    MessageBoxResult result = (MessageBoxResult)MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            currentCarSpareParts.Remove((int)listBoxCurrentSpareParts.SelectedValue);
                            ReloadList();
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Ошибка удаления запчасти из списка : " + ex.Message);
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите машину", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxCars.SelectedValue == null)
            {
                MessageBox.Show("Выберите машину", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logicC.CreateOrUpdate(new CarBindingModel
                {
                    Id = carView.Id,
                    CarName = carView.CarName,
                    UserId = carView.UserId,
                    CarSpareParts = currentCarSpareParts,
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка сохранения данных : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void comboBoxCars_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadData();
        }
    }
}

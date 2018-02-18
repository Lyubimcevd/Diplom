using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using ARMExperta.Classes;

namespace ARMExperta.Windows
{
    public partial class EducationGroups : Window
    {
        Dictionary<int, string> students;

        public EducationGroups()
        {
            InitializeComponent();
            UpdateComboBox();
        }

        private void combobox_group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox_group.SelectedItem != null)
            {
                students = Server.GetServer.GetStudentsFromEducationGroup(Server.GetServer.GetGroups().First(x => x.Value == combobox_group.SelectedItem.ToString()).Key);
                datagrid.ItemsSource = CurrentSystemStatus.GetSS.ConvertDictionaryToDataTable(students).DefaultView;
            }
        }

        private void AddNewEducationGroup(object sender, RoutedEventArgs e)
        {
            WindowForEdit WFE = new WindowForEdit();
            WFE.ShowDialog();
            if (WFE.Result.Length != 0)
            {
                if (Server.GetServer.GetGroups().FirstOrDefault(x=>x.Value == WFE.Result).Value != null)
                {
                    MessageBox.Show("Группа с таким названием уже есть", "АРМ Эксперта");
                    combobox_group.SelectedValue = WFE.Result;
                }
                else
                {
                    Server.GetServer.SetNewGroup(WFE.Result);
                    UpdateComboBox();
                }
            }
        }

        private void DeleteEducationGroup(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены? Будут удалены все студенты группы, рабочие группы, модели рабочих групп","АРМ Эксперта",MessageBoxButton.YesNo) 
                == MessageBoxResult.Yes)
            {
                Server.GetServer.DeleteGroup(Server.GetServer.GetGroups().First(x=>x.Value == combobox_group.SelectedItem.ToString()).Key);
                MessageBox.Show("Группа удалена", "АРМ Эксперта");
                UpdateComboBox();
            }
        }

        void UpdateComboBox()
        {
            combobox_group.ItemsSource = Server.GetServer.GetGroups().Values;
            if (combobox_group.Items.Count != 0) combobox_group.SelectedIndex = 0;
            else combobox_group.SelectedIndex = -1;
        }

        private void SaveStudents(object sender, RoutedEventArgs e)
        {
            Server.GetServer.UpdateGroup((datagrid.ItemsSource as DataView).Table, Server.GetServer.GetGroups().First(x => x.Value == combobox_group.SelectedItem.ToString()).Key);
            combobox_group_SelectionChanged(null,null);
            MessageBox.Show("Сохранено", "АРМ Эксперта");
            save_button.IsEnabled = false;
        }

        private void datagrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            save_button.IsEnabled = true;
        }
    }
}

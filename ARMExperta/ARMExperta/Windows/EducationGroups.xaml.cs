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
        public EducationGroups()
        {
            InitializeComponent();
            UpdateComboBox();
            combobox_group.SelectedIndex = 0;
        }

        private void combobox_group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string group;
            if (combobox_group.Items.Count != 0) group = combobox_group.SelectedValue.ToString();
            else group = "";
            datagrid_group.ItemsSource = Server.GetServer.GetStudentsFromGroupDataTable(group).DefaultView;
        }

        private void AddNewEducationGroup(object sender, RoutedEventArgs e)
        {
            WindowForEdit WFE = new WindowForEdit();
            WFE.ShowDialog();
            if (WFE.Result.Trim().Length != 0)
            {
                bool error = false;
                foreach (string group in Server.GetServer.GetGroups())
                    if (WFE.Result == group)
                    {
                        error = true;
                        break;
                    }
                if (error)
                {
                    MessageBox.Show("Группа с таким названием уже есть", "АРМ Эксперта");
                    combobox_group.SelectedValue = WFE.Result;
                    return;
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
                Server.GetServer.DeleteGroup(combobox_group.SelectedValue.ToString());
                UpdateComboBox();
                MessageBox.Show("Группа удалена", "АРМ Эксперта");
            }
        }

        void UpdateComboBox()
        {
            combobox_group.ItemsSource = Server.GetServer.GetGroups();
        }

        private void SaveStudents(object sender, RoutedEventArgs e)
        {
            Server.GetServer.SaveStudentsListOnServer((datagrid_group.ItemsSource as DataView).Table, combobox_group.SelectedValue.ToString());
            combobox_group_SelectionChanged(null,null);
            MessageBox.Show("Сохранено", "АРМ Эксперта");
            save_button.IsEnabled = false;
        }

        private void datagrid_group_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            save_button.IsEnabled = true;
        }
    }
}

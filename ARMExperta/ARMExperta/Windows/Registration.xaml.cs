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
using ARMExperta.Classes;

namespace ARMExperta.Windows
{
    public partial class Registration : Window
    {
        Dictionary<int, string> stud_in_ed_group = new Dictionary<int, string>();
        Dictionary<int, string> stud_in_work_group = new Dictionary<int, string>();

        public Registration()
        {
            InitializeComponent();
            combobox.ItemsSource = Server.GetServer.GetGroups().Values;
            combobox.SelectedIndex = 0;
        }

        private void combobox_group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            stud_in_ed_group = Server.GetServer.GetStudentsFromEducationGroup(Server.GetServer.GetGroups().First(x => x.Value == combobox.SelectedItem.ToString()).Key);
            listbox_list_of_group.ItemsSource = stud_in_ed_group.Values;
        }

        private void listbox_list_of_group_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listbox_list_of_group.SelectedItem != null)
                if (stud_in_work_group.FirstOrDefault(x => x.Value == listbox_list_of_group.SelectedItem.ToString()).Value == null)
                {
                    stud_in_work_group.Add(stud_in_ed_group.First(x => x.Value == listbox_list_of_group
                    .SelectedItem.ToString()).Key, listbox_list_of_group.SelectedItem.ToString());
                    listbox_list_of_new_group.ItemsSource = stud_in_work_group.Values.ToList();
                }
                else MessageBox.Show("Студент уже добавлен в группу","АРМ Эксперта");
        }

        private void listbox_list_of_new_group_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listbox_list_of_new_group.SelectedItem != null) stud_in_work_group.Remove(stud_in_work_group.First(x=>x.Value == listbox_list_of_new_group.SelectedItem.ToString()).Key);
            listbox_list_of_new_group.ItemsSource = stud_in_work_group.Values.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (listbox_list_of_new_group.Items.Count == 0)
            {
                MessageBox.Show("Нет участников группы");
                return;
            }
            if (textbox_password.Text.Length == 0)
            {
                MessageBox.Show("Не задан пароль группы");
                return;
            }
            if (listbox_list_of_new_group.Items.Count!=0&&textbox_password.Text.Length!=0)
            {
                int id = Server.GetServer.AddNewWorkGroup(textbox_password.Text);
                Server.GetServer.UpdateWorkGroup(stud_in_work_group, id);
                MessageBox.Show("Группа зарегестрирована");
            }
        }

        private void textbox_password_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Button_Click(null, null);
        }
    }
}

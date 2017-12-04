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
        public Registration()
        {
            InitializeComponent();
            combobox_group.ItemsSource = Server.GetServer.GetGroups();
        }

        private void combobox_group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listbox_list_of_group.ItemsSource = Server.GetServer.GetStudentsFromGroup(combobox_group.SelectedValue.ToString());
        }

        private void listbox_list_of_group_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listbox_list_of_group.SelectedValue != null)
                listbox_list_of_new_group.Items.Add(listbox_list_of_group.SelectedValue);
        }

        private void listbox_list_of_new_group_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listbox_list_of_new_group.SelectedValue != null)
                listbox_list_of_new_group.Items.Remove(listbox_list_of_new_group.SelectedItem);
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
                List<string> students = new List<string>();
                foreach (string fio in listbox_list_of_new_group.Items) students.Add(fio);
                Server.GetServer.SetStudentsGroup(students, textbox_password.Text);
                MessageBox.Show("Группа зарегестрирована");
            }
        }

        private void textbox_password_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Button_Click(null, null);
        }
    }
}

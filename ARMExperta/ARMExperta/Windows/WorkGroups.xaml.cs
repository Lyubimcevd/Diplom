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
    public partial class WorkGroups : Window
    {
        List<User> groups;
        Dictionary<int, string> students;
        MainWindow main_window;
        string preview_mark;

        public WorkGroups(MainWindow p_main_window)
        {
            InitializeComponent();
            main_window = p_main_window;
            UpdateComboBox();
        }

        private void Give_Gost(object sender, RoutedEventArgs e)
        {
            Choice Ch = new Choice(Server.GetServer.GetGOSTs());
            Ch.ShowDialog();
            if (Ch.Result != 0)
            {
                Server.GetServer.CopyGOSTToAllWorkGroups(Ch.Result);
                MessageBox.Show("Готово", "АРМ Эксперта");
            }
        }

        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox.SelectedItem != null)
            {
                students = Server.GetServer.GetStudentsFromWorkGroup(groups.First(x => x == combobox.SelectedItem).Id);
                listbox.ItemsSource = students.Values.ToList();
                passwordbox.Password = groups.First(x => x == combobox.SelectedItem).Password;
                mark.Text = groups.First(x => x == combobox.SelectedItem).Mark;
                preview_mark = mark.Text;
                if (groups.First(x => x == combobox.SelectedItem).IsReady) ready.Text = "ГОТОВА";
            }
        }

        private void Add_Student(object sender, RoutedEventArgs e)
        {
            Dictionary<int, string> tmp = Server.GetServer.GetStudentsFromEducationGroup(groups.First(x => x == combobox.SelectedItem).IdEucationGroup);
            Choice Ch = new Choice(tmp);
            Ch.ShowDialog();
            if (Ch.Result != 0)
            {
                students.Add(Ch.Result, tmp[Ch.Result]);
                listbox.ItemsSource = students.Values.ToList();
            }
        }

        private void Delete_Student(object sender, RoutedEventArgs e)
        {
            if (listbox.SelectedItem != null)
            {
                if (students.Count != 1)
                {
                    students.Remove(students.First(x => x.Value == listbox.SelectedItem.ToString()).Key);
                    listbox.ItemsSource = students.Values.ToList();
                }
                else
                if (MessageBox.Show("Рабочая группа будет удалена. Продолжить?", "АРМ Эксперта", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes) Delete_Group(null, null);
            }
            else MessageBox.Show("Студент не выбран", "АРМ Эксперта");
        }

        void UpdateComboBox()
        {
            groups = Server.GetServer.GetWorkGroups();
            combobox.ItemsSource = groups;
            combobox.SelectedIndex = 0;
        }

        private void Delete_Group(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены? Будет удалена модель рабочей группы", "АРМ Эксперта", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                Server.GetServer.DeleteWorkGroup(groups.First(x => x == combobox.SelectedItem).Id);
                MessageBox.Show("Группа удалена", "АРМ Эксперта");
                UpdateComboBox();
            }
        }

        private void model_Click(object sender, RoutedEventArgs e)
        {
            main_window.OpenUserModel(groups.First(x => x == combobox.SelectedItem));
            main_window.Activate();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            Server.GetServer.UpdateWorkGroup(students, groups.First(x => x == combobox.SelectedItem).Id);
            Server.GetServer.UpdatePasswordForWorkGroup(passwordbox.Password, groups.First(x => x == combobox.SelectedItem).Id);
            if (preview_mark!=mark.Text)
            {
                Server.GetServer.UpdateMarkForWorkGroup(mark.Text, groups.First(x => x == combobox.SelectedItem).Id);
                Server.GetServer.SendMessage("Оценка " + mark.SelectedValue + ".", CurrentSystemStatus.GetSS.CurrentUser.Id, 
                    groups.First(x => x == combobox.SelectedItem).Id, true);
            }
            MessageBox.Show("Сохранено", "АРМ Эксперта");
        }

        private void chat_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.OpenChats.ContainsKey(groups.First(x=>x == combobox.SelectedItem).Id))
            {
                CurrentSystemStatus.GetSS.OpenChats[groups.First(x => x == combobox.SelectedItem).Id].Update();
                CurrentSystemStatus.GetSS.OpenChats[groups.First(x => x == combobox.SelectedItem).Id].Activate();
            }
            else
            {
                Chat Ch = new Chat(groups.First(x => x == combobox.SelectedItem).Id);
                Ch.Show();
            }
        }
    }
}

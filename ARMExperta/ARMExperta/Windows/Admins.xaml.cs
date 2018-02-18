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
    
    public partial class Admins : Window
    {
        List<User> admins;

        public Admins()
        {
            InitializeComponent();
            Update();
        }   

        private void AddNewAdmin(object sender, RoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.StringIsCorrect(textbox_fio.Text) && CurrentSystemStatus.GetSS.StringIsCorrect(passwordbox.Password))
            {
                if (Server.GetServer.GetAdmins().FirstOrDefault(x => x.Naim == textbox_fio.Text.Trim()) == null)
                {
                    Server.GetServer.AddNewAdmin(textbox_fio.Text, passwordbox.Password);
                    MessageBox.Show("Пользователь добавлен в систему", "АРМ Эксперта");
                    Update();
                }
                else MessageBox.Show("Такой пользователь уже есть в системе", "АРМ Эксперта");             
            }
            else MessageBox.Show("Введите логин и пароль", "АРМ Эксперта");
        }

        private void ChangeChoiceAdmin(object sender, RoutedEventArgs e)
        {
            User current_user = null;
            if (admins.First(x => x.Naim == listbox.SelectedItem.ToString()) != CurrentSystemStatus.GetSS.CurrentUser)
            {
                Login LG = new Login();
                LG.ShowDialog();
                if (LG.EnterUser?.Id != -1) current_user = LG.EnterUser;
            }
            else current_user = CurrentSystemStatus.GetSS.CurrentUser;
            if (current_user != null)
            {
                Server.GetServer.UpdateAdmin(listbox.SelectedItem.ToString(), passwordbox.Password, admins.First(x => x.Naim == listbox.SelectedItem.ToString()).Id);
                MessageBox.Show("Данные о пользователе обновлены", "АРМ Эксперта");
                Update();
            }
        }

        private void DeleteChoiceAdmin(object sender, RoutedEventArgs e)
        {
            User current_user = null;
            if (admins.First(x => x.Naim == listbox.SelectedItem.ToString()) != CurrentSystemStatus.GetSS.CurrentUser)
            {
                Login LG = new Login();
                LG.ShowDialog();
                if (LG.EnterUser?.Id != -1) current_user = LG.EnterUser;
            }
            else current_user = CurrentSystemStatus.GetSS.CurrentUser;
            if (current_user != null)
            {
                Server.GetServer.DeleteAdmin(current_user.Id);
                MessageBox.Show("Пользователь удалён из системы", "АРМ Эксперта");
                Update();
            }
        }

        void Update()
        {
            admins = Server.GetServer.GetAdmins();
            List<string> tmp = new List<string>();
            foreach (User us in admins) tmp.Add(us.Naim);
            listbox.ItemsSource = tmp;
            textbox_fio.Text = "";
            passwordbox.Password = "";
            change.IsEnabled = false;
            delete.IsEnabled = false;
        }

        private void listbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textbox_fio.Text = listbox.SelectedItem.ToString();
            passwordbox.Password = admins.First(x => x.Naim == textbox_fio.Text).Password;
            change.IsEnabled = true;
            delete.IsEnabled = true;
        }
    }
}

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

        private void datagrid_admins_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            textbox_fio.Text = (datagrid_admins.SelectedItem as User).Naim;
            passwordbox.Password = (datagrid_admins.SelectedItem as User).Password;
        }

        private void AddNewAdmin(object sender, RoutedEventArgs e)
        {
            if (textbox_fio.Text.Trim().Length != 0 && passwordbox.Password.Trim().Length != 0)
            {
                if (!Server.GetServer.AddNewAdmin(textbox_fio.Text, passwordbox.Password)) MessageBox.Show("Такой пользователь уже есть в системе", "АРМ Эксперта");
                else
                {
                    MessageBox.Show("Пользователь добавлен в систему", "АРМ Эксперта");
                    Update();
                }
            }
            else MessageBox.Show("Введите логин и пароль", "АРМ Эксперта");
        }

        private void ChangeChoiceAdmin(object sender, RoutedEventArgs e)
        {
            bool result = false;
            if (datagrid_admins.SelectedItem as User != CurrentSystemStatus.GetSS.CurrentUser)
            {
                Login Log = new Login(true);
                Log.ShowDialog();
                if (Log.GetRightRoots()) result = true;
            }
            else result = true;
            if (result)
            {
                Server.GetServer.UpdateAdmin(textbox_fio.Text, passwordbox.Password,(datagrid_admins.SelectedItem as User).Id);
                MessageBox.Show("Данные о пользователе обновлены", "АРМ Эксперта");
                Update();
            }
        }

        private void DeleteChoiceAdmin(object sender, RoutedEventArgs e)
        {
            bool result = false;
            if (datagrid_admins.SelectedItem as User != CurrentSystemStatus.GetSS.CurrentUser)
            {
                Login Log = new Login(true);
                Log.ShowDialog();
                if (Log.GetRightRoots()) result = true;
            }
            else result = true;
            if (result)
            {
                Server.GetServer.DeleteAdmin(datagrid_admins.SelectedItem as User);
                MessageBox.Show("Пользователь удалён из системы", "АРМ Эксперта");
                Update();
            }
        }

        void Update()
        {
            admins = Server.GetServer.GetAdminsAndPassword();
            datagrid_admins.ItemsSource = admins;
            textbox_fio.Text = "";
            passwordbox.Password = "";
        }
    }
}

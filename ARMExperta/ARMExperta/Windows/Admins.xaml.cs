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
            UpdateDataGrid();
        }

        private void datagrid_admins_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            textbox_fio.Text = (datagrid_admins.SelectedItem as User).Naim;
            string stars = "";
            for (int i = 0; i < (datagrid_admins.SelectedItem as User).Password.Length; i++) stars += "*";
            textbox_pwd.Text = stars;
        }

        private void AddNewAdmin(object sender, RoutedEventArgs e)
        {
            if (!Server.GetServer.AddNewAdmin(textbox_fio.Text, textbox_pwd.Text)) MessageBox.Show("Такой пользователь уже есть в системе", "АРМ Эксперта");
            else
            {
                MessageBox.Show("Пользователь добавлен в систему", "АРМ Эксперта");
                UpdateDataGrid();
            }
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
                Server.GetServer.UpdateAdmin(textbox_fio.Text,textbox_pwd.Text,(datagrid_admins.SelectedItem as User).Id);
                MessageBox.Show("Данные о пользователе обновлены", "АРМ Эксперта");
                UpdateDataGrid();
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
                UpdateDataGrid();
            }
        }

        void UpdateDataGrid()
        {
            admins = Server.GetServer.GetAdminsAndPassword();
            datagrid_admins.ItemsSource = admins;
        }
    }
}

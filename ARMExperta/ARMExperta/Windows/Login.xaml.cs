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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ARMExperta.Classes;

namespace ARMExperta.Windows
{
    public partial class Login : Window
    {
        MainWindow MW;
        List<User> users;
        List<string> logins;
        bool flag_podtv_admin_roots = false,
             return_flag = false;

        public Login()
        {
            InitializeComponent();
            UpdateComboBox();          
        }

        public Login(bool flag)
        {
            InitializeComponent();
            UpdateComboBox();
            flag_podtv_admin_roots = true;
        }

        private void EnterInSystem(object sender, RoutedEventArgs e)
        {
            User current_user = users.FirstOrDefault(x => x.Naim == combobox_login.SelectedValue.ToString());
            if (passwordbox.Password == current_user.Password)
            {
                if (!flag_podtv_admin_roots)
                {
                    CurrentSystemStatus.GetSS.CurrentUser = current_user;
                    MW = new MainWindow();
                    MW.Show();
                }
                this.Close();
                return_flag = true;
            }
            else MessageBox.Show("Неверный пароль");
        }   

        private void RegistrationInSystem(object sender, RoutedEventArgs e)
        {
            Registration REG = new Registration();
            REG.ShowDialog();
            UpdateComboBox();
        }

        void UpdateComboBox()
        {
            users = Server.GetServer.GetWorkGroupsAndPassword();
            users.AddRange(Server.GetServer.GetAdminsAndPassword());
            logins = new List<string>();
            foreach (User user in users) logins.Add(user.Naim);
            combobox_login.ItemsSource = logins;
            combobox_login.SelectedIndex = 0;
        }

        private void passwordbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                EnterInSystem(null, null);
        }

        public bool GetRightRoots()
        {
            return return_flag;
        }       
    }
}

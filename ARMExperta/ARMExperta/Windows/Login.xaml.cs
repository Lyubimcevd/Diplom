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
        List<User> users;
        User current_user;
        bool is_attempt_enter;

        public Login()
        {
            InitializeComponent();
            UpdateComboBox();          
        }

        private void EnterInSystem(object sender, RoutedEventArgs e)
        {
            User tmp = users.First(x => x == combobox.SelectedItem);
            if (passwordbox.Password == tmp.Password) current_user = tmp;
            is_attempt_enter = true;
            this.Close();
        }   

        private void RegistrationInSystem(object sender, RoutedEventArgs e)
        {
            Registration REG = new Registration();
            REG.ShowDialog();
            UpdateComboBox();
        }

        void UpdateComboBox()
        {
            users = Server.GetServer.GetWorkGroups();
            users.AddRange(Server.GetServer.GetAdmins());           
            combobox.ItemsSource = users;
            combobox.SelectedIndex = 0;
        }

        private void passwordbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                EnterInSystem(null, null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!is_attempt_enter) current_user = new User(-1, false);
        }

        public User EnterUser
        {
            get
            {
                return current_user;
            }
        }
    }
}

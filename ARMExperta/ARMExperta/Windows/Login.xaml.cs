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

        public Login()
        {
            InitializeComponent();
            
            users = Server.GetServer.GetUsersAndPassword();
            List<string> logins = new List<string>();
            foreach (User user in users) logins.Add(user.Naim);
            combobox_login.ItemsSource = logins;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User current_user = users.FirstOrDefault(x => x.Naim == combobox_login.SelectedValue.ToString());
            if (textbox_password.Text == current_user.Password)
            {
                CurrentSystemStatus.GetSS.CurrentUser = current_user;
                MW = new MainWindow();
                MW.Show();
                this.Close();
            }
            else MessageBox.Show("Неверный пароль");
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click(null, null);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Registration REG = new Registration();
            REG.ShowDialog();
        }
    }
}

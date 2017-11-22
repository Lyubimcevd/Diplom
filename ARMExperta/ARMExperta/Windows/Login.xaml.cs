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
        Dictionary<string, string> login_password;

        public Login()
        {
            InitializeComponent();
            
            login_password = Server.GetServer.GetUsersAndPassword();
            List<string> logins = login_password.Keys.ToList();
            combobox_login.ItemsSource = logins;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (textbox_password.Text == login_password[combobox_login.Text]) ;
            //всё хорошо
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

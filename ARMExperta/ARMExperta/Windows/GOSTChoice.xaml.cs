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
   
    public partial class GOSTChoice : Window
    {
        string choice_gost;

        public GOSTChoice()
        {
            InitializeComponent();
            listbox.ItemsSource = Server.GetServer.GetGOSTs();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            choice_gost = listbox.SelectedItem.ToString();
            this.Close();
        }

        private void listbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (listbox.SelectedItem!= null) Button_Click(null, null);
        }

        public string Result
        {
            get
            {
                return choice_gost;
            }
        }
    }
}

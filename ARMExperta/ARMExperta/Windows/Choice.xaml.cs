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
   
    public partial class Choice : Window
    {
        int id;
        Dictionary<int, string> dict;

        public Choice(Dictionary<int,string> p_dict)
        {
            InitializeComponent();
            dict = p_dict;
            listbox.ItemsSource = dict.Values;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            id = dict.First(x=>x.Value == listbox.SelectedItem.ToString()).Key;
            this.Close();
        }

        private void listbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (listbox.SelectedItem!= null) Button_Click(null, null);
        }

        public int Result
        {
            get
            {
                return id;
            }
        }
    }
}

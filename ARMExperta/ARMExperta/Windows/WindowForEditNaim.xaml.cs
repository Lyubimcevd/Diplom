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
    public partial class WindowForEditNaim : Window
    {
        string last_naim;

        public WindowForEditNaim(TreeViewModal list)
        {
            InitializeComponent();
            textbox.DataContext = list;
            last_naim = list.Naim;
            Loaded += delegate { textbox.Focus(); };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (textbox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Введите наименование поля");
                textbox.Text = last_naim;
            }
            else
            {
                CurrentSystemStatus.GetSS.AddInHistory();
                this.Close();
            }
        }

        private void textbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Button_Click(null, null);
        }

        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            textbox.SelectAll();
        }
    }
}

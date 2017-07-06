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
using Editor.Classes;

namespace Editor.Windows
{
    public partial class WindowForEditNaim : Window
    {
        bool is_button = false;

        public WindowForEditNaim(TreeViewModal list)
        {
            InitializeComponent();
            textbox.DataContext = list;
            Loaded += delegate { textbox.Focus(); };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            is_button = true;
            this.Close();
        }

        private void textbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Button_Click(null, null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!is_button) textbox.Text = null;
        }
    }
}

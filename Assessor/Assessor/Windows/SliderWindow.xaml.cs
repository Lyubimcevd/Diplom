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
using Assessor.Classes;

namespace Assessor.Windows
{
    public partial class SliderWindow : Window
    {
        TreeViewExpertModal current;
        public SliderWindow(TreeViewExpertModal pcurrent)
        {
            InitializeComponent();
            current = pcurrent;
            this.DataContext = current;
            Loaded += delegate { text_box.Focus(); };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (current.Children.Count == 0) current.Is_Ready = true;
            current.Parent.ChangeRightBorder();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) this.Close();
        }

        private void text_box_GotFocus(object sender, RoutedEventArgs e)
        {
            text_box.SelectAll();
        }
    }
}

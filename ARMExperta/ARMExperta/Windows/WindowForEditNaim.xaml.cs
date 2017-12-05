using System.Windows;
using System.Windows.Input;
using ARMExperta.Classes;

namespace ARMExperta.Windows
{
    public partial class WindowForEditNaim : Window
    {
        string last_naim;

        public WindowForEditNaim(TreeViewModal tvm)
        {
            InitializeComponent();
            last_naim = tvm.Naim;
            textbox.DataContext = tvm;
            Loaded += delegate { textbox.Focus(); };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (textbox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Введите наименование поля");
                textbox.Text = last_naim;
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

        private void Window_Closed(object sender, System.EventArgs e)
        {
            textbox.Text = null;
        }
    }
}

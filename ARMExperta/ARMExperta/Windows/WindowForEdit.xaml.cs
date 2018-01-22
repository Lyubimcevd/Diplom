using System.Windows;
using System.Windows.Input;
using ARMExperta.Classes;

namespace ARMExperta.Windows
{
    public partial class WindowForEdit : Window
    {
        string last_naim;

        public WindowForEdit()
        {
            InitializeComponent();
            Loaded += delegate { textbox.Focus(); };
        }

        public WindowForEdit(string tmp)
        {
            InitializeComponent();
            last_naim = tmp;
            textbox.Text = tmp;
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
                this.Close();
            }
        }

        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            textbox.SelectAll();
        }

        public string GetResult
        {
            get
            {
                return textbox.Text;
            }
        }

        private void textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Button_Click(null, null);
        }
    }
}

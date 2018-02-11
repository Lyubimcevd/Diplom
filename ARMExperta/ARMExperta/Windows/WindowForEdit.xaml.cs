using System.Windows;
using System.Windows.Input;
using ARMExperta.Classes;

namespace ARMExperta.Windows
{
    public partial class WindowForEdit : Window
    {
    
        public WindowForEdit()
        {
            InitializeComponent();
            Loaded += delegate { textbox.Focus(); };
        }

        public WindowForEdit(string tmp)
        {
            InitializeComponent();
            textbox.Text = tmp;
            Loaded += delegate { textbox.Focus(); };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            textbox.SelectAll();
        }

        public string Result
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

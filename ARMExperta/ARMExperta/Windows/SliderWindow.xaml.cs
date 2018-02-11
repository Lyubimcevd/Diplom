using System.Windows;
using System.Windows.Input;
using ARMExperta.Classes;

namespace ARMExperta.Windows
{
    public partial class SliderWindow : Window
    {
        public SliderWindow()
        {
            InitializeComponent();
            main.DataContext = CurrentSystemStatus.GetSS.CurrentElement;
            Loaded += delegate { text_box.Focus(); };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CurrentSystemStatus.GetSS.CurrentElement.Parent.UpdateReady();
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

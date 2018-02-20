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
    public partial class Chat : Window
    {
        List<Message> mes;
        int who = -1;

        public Chat()
        {
            InitializeComponent();
            combobox.ItemsSource = Server.GetServer.GetAdmins();
            combobox.SelectedIndex = 0;
            Update();
        }

        public Chat(int p_who)
        {
            InitializeComponent();
            who = p_who;
            CurrentSystemStatus.GetSS.OpenChats.Add(who, this);
            grid.Visibility = Visibility.Collapsed;
            Update();            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.StringIsCorrect(sms.Text))
            {
                Server.GetServer.SendMessage(sms.Text.Trim(), CurrentSystemStatus.GetSS.CurrentUser.Id, who, !CurrentSystemStatus.GetSS.CurrentUser.IsGroup);
                Update();
                sms.Text = "";
            }
        }

        public void Update()
        {
            mes = Server.GetServer.GetLastMessages(who,CurrentSystemStatus.GetSS.CurrentUser.Id,CurrentSystemStatus.GetSS.CurrentUser.IsGroup);
            mes.AddRange(Server.GetServer.GetLastMessages(CurrentSystemStatus.GetSS.CurrentUser.Id,who,!CurrentSystemStatus.GetSS.CurrentUser.IsGroup));
            mes.Sort((x, y) => DateTime.Compare(x.Time, y.Time));
            listbox.ItemsSource = mes;
            listbox.SelectedIndex = listbox.Items.Count - 1;
            listbox.ScrollIntoView(listbox.SelectedItem);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CurrentSystemStatus.GetSS.OpenChats.Remove(who);
        }

        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (who != -1) CurrentSystemStatus.GetSS.OpenChats.Remove(who);
            who = (combobox.SelectedItem as User).Id;
            CurrentSystemStatus.GetSS.OpenChats.Add(who, this);
        }

        private void sms_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Button_Click(null, null);
        }
    }
}

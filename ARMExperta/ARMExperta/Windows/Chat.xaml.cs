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

        public Chat()
        {
            InitializeComponent();
            mes = Server.GetServer.GetLastMessages(CurrentSystemStatus.GetSS.CurrentUser.Id);
            listbox.ItemsSource = mes;
            combobox.ItemsSource = Server.GetServer.GetAdmins();
        }

        public void NewSms(Message sms)
        {
            mes.Add(sms);
        }
    }
}

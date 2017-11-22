using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ARMExperta.Windows;

namespace ARMExperta.Classes
{
    public class Commands
    {
        static Commands()
        {
            Forward = new RoutedCommand("Forward", typeof(MainWindow));
            Rename = new RoutedCommand("Rename", typeof(MainWindow));
        }
        public static RoutedCommand Forward { get; set; }
        public static RoutedCommand Rename { get; set; }
    }
}

using System.Windows.Input;
using ARMExperta.Windows;

namespace ARMExperta.Classes
{
    public class Commands
    {
        public static RoutedCommand Forward { get; set; }
        public static RoutedCommand Rename { get; set; }
        public static RoutedCommand Estimate { get; set; }
        public static RoutedCommand About { get; set; }
        public static RoutedCommand ChangeWorkMode { get; set; }
        public static RoutedCommand AddElement { get; set; }

        static Commands()
        {
            Forward = new RoutedCommand("Forward", typeof(MainWindow));
            Rename = new RoutedCommand("Rename", typeof(MainWindow));
            Estimate = new RoutedCommand("Estimate", typeof(MainWindow));
            About = new RoutedCommand("About", typeof(MainWindow));
            ChangeWorkMode = new RoutedCommand("ChangeWorkMode", typeof(MainWindow));
            AddElement = new RoutedCommand("AddElement", typeof(MainWindow));
        }
    }
}

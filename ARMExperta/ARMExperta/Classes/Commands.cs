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
        public static RoutedCommand EducationGroup { get; set; }
        public static RoutedCommand WorkGroup { get; set; }
        public static RoutedCommand Admins { get; set; }
        public static RoutedCommand Check { get; set; }
        public static RoutedCommand Ready { get; set; }

        static Commands()
        {
            Forward = new RoutedCommand("Forward", typeof(MainWindow));
            Rename = new RoutedCommand("Rename", typeof(MainWindow));
            Estimate = new RoutedCommand("Estimate", typeof(MainWindow));
            About = new RoutedCommand("About", typeof(MainWindow));
            ChangeWorkMode = new RoutedCommand("ChangeWorkMode", typeof(MainWindow));
            AddElement = new RoutedCommand("AddElement", typeof(MainWindow));
            EducationGroup = new RoutedCommand("EducationGroup", typeof(MainWindow));
            WorkGroup = new RoutedCommand("WorkGroup", typeof(MainWindow));
            Admins = new RoutedCommand("Admins", typeof(MainWindow));
            Check = new RoutedCommand("Check", typeof(MainWindow));
            Ready = new RoutedCommand("Ready", typeof(MainWindow));
        }
    }
}

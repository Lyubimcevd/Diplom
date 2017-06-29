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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Editor.Classes;

namespace Editor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CommandBinding();
        }

        void CommandBinding()
        {
            CommandBinding commandBinding = new CommandBinding(ApplicationCommands.New);
            commandBinding.Executed += CommandBinding_New;
            this.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(ApplicationCommands.Open);
            commandBinding.Executed += CommandBinding_Open;
            this.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(ApplicationCommands.Save);
            commandBinding.Executed += CommandBinding_Save;
            this.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(ApplicationCommands.SaveAs);
            commandBinding.Executed += CommandBinding_SaveAs;
            this.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(ApplicationCommands.Undo);
            commandBinding.Executed += CommandBinding_Undo;
            this.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(ApplicationCommands.Cut);
            commandBinding.Executed += CommandBinding_Cut;
            this.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(ApplicationCommands.Copy);
            commandBinding.Executed += CommandBinding_Copy;
            this.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(ApplicationCommands.Paste);
            commandBinding.Executed += CommandBinding_Paste;
            this.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(ApplicationCommands.Help);
            commandBinding.Executed += CommandBinding_Help;
            this.CommandBindings.Add(commandBinding);
        }

        void CommandBinding_New(object sender, ExecutedRoutedEventArgs e)
        {
            ObservableCollection<TreeViewModal> Root = new ObservableCollection<TreeViewModal>();
            Root.Add(new TreeViewModal("Бла бла бла"));
            tree.ItemsSource = Root;
        }

        void CommandBinding_Open(object sender, ExecutedRoutedEventArgs e)
        {

        }

        void CommandBinding_Save(object sender, ExecutedRoutedEventArgs e)
        {

        }

        void CommandBinding_SaveAs(object sender, ExecutedRoutedEventArgs e)
        {

        }

        void CommandBinding_Undo(object sender, ExecutedRoutedEventArgs e)
        {

        }

        void CommandBinding_Cut(object sender, ExecutedRoutedEventArgs e)
        {

        }

        void CommandBinding_Copy(object sender, ExecutedRoutedEventArgs e)
        {

        }

        void CommandBinding_Paste(object sender, ExecutedRoutedEventArgs e)
        {

        }

        void CommandBinding_Help(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((sender as MenuItem).DataContext as TreeViewModal).Children.Add(new TreeViewModal("Бла бла бла 2"));
        }
    }
}

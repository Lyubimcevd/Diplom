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
using Editor.Windows;

namespace Editor
{
    public partial class MainWindow : Window
    {
        TreeViewModal buffer,current,root;
        List<ObservableCollection<TreeViewModal>> history;
        int current_index;

        public MainWindow()
        {
            InitializeComponent();
        }

        void CommandBinding_New(object sender, ExecutedRoutedEventArgs e)
        {
            history = new List<ObservableCollection<TreeViewModal>>();
            ObservableCollection<TreeViewModal> Root = new ObservableCollection<TreeViewModal>();
            Root.Add(TreeViewModal.NewItem(null));
            tree.ItemsSource = Root;
            root = Root[0];
            Historian();
            root.IsHistoryBegin = true;
            root.IsOpen = true;
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
            current_index--;
            tree.ItemsSource = history[current_index];
            if (current_index == 0) root.IsHistoryBegin = true;
            root.IsHistoryEnd = false;
        }

        void CommandBinding_Forward(object sender, ExecutedRoutedEventArgs e)
        {
            current_index++;
            tree.ItemsSource = history[current_index];
            if (current_index == history.Count-1) root.IsHistoryEnd = true;
            root.IsHistoryBegin = false;
        }

        void CommandBinding_Cut(object sender, ExecutedRoutedEventArgs e)
        {
            current = (e.Source as TreeView).SelectedItem as TreeViewModal;
            buffer = current.Clone;
            current.Parent.Children.Remove(current);
            Historian();
            root.IsBuffer = true;
        }

        void CommandBinding_Copy(object sender, ExecutedRoutedEventArgs e)
        {
            current = (e.Source as TreeView).SelectedItem as TreeViewModal;
            buffer = current.Clone;
            root.IsBuffer = true;
        }

        void CommandBinding_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            current = (e.Source as TreeView).SelectedItem as TreeViewModal;
            current.Children.Add(buffer.Clone);
            Historian();
            root.IsBuffer = true;
        }

        void CommandBinding_Help(object sender, ExecutedRoutedEventArgs e)
        {

        }

        void CommandBinding_Close(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            root.IsSelect = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            current = (sender as MenuItem).DataContext as TreeViewModal;
            current.Children.Add(TreeViewModal.NewItem(current));
            Historian();
        }

        void Historian()
        {
            history.Add(tree.ItemsSource as ObservableCollection<TreeViewModal>);
            root.IsHistoryBegin = false;
            current_index = history.Count - 1;
        }
    }
}

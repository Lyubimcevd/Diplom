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
        TreeViewModal buffer,current;
        List<TreeViewModal> history;
        ObservableCollection<TreeViewModal> Root;
        int current_index;
        bool is_buffer = false,
            is_history_end = true,
            is_history_begin = true,
            is_save = true,
            is_select = false,
            is_open = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region CommandBinding

        void CommandBinding_New(object sender, ExecutedRoutedEventArgs e)
        {
            history = new List<TreeViewModal>();
            Root = new ObservableCollection<TreeViewModal>();
            Root.Add(TreeViewModal.NewItem(null));
            tree.ItemsSource = Root;
            Historian();
            is_history_begin = true;
            is_open = true;
            is_save = false;
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
            Root = new ObservableCollection<TreeViewModal>();
            Root.Add(history[current_index]);
            tree.ItemsSource = Root;
            if (current_index == 0) is_history_begin = true;
            is_history_end = false;
        }

        void CommandBinding_Forward(object sender, ExecutedRoutedEventArgs e)
        {
            current_index++;
            Root = new ObservableCollection<TreeViewModal>();
            Root.Add(history[current_index]);
            tree.ItemsSource = Root;
            if (current_index == history.Count-1) is_history_end = true;
            is_history_begin = false;
        }

        void CommandBinding_Cut(object sender, ExecutedRoutedEventArgs e)
        {
            buffer = current.Clone;
            current.Parent.Children.Remove(current);
            Historian();
            is_buffer = true;
        }

        void CommandBinding_Copy(object sender, ExecutedRoutedEventArgs e)
        {
            buffer = current.Clone;
            is_buffer = true;
        }

        void CommandBinding_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            current.Children.Add(buffer.Clone);
            Historian();
            is_save = false;
        }

        void CommandBinding_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            current.Parent.Children.Remove(current);
            is_save = false;
        }

        void CommandBinding_Help(object sender, ExecutedRoutedEventArgs e)
        {

        }

        void CommandBinding_Close(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Can_Execute

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !is_save;
        }

        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = is_open;
        }

        private void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = is_select;
        }

        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = is_select;
        }

        private void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = is_buffer&&is_select;
        }

        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !is_history_begin;
        }

        private void Forward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !is_history_end;
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = is_select;
        }

        #endregion

        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            current = tree.SelectedItem as TreeViewModal;
            is_select = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            current = (sender as MenuItem).DataContext as TreeViewModal;
            current.Children.Add(TreeViewModal.NewItem(current));
            Historian();
            is_save = false;
        }
        void Historian()
        {
            history.Add((tree.ItemsSource as ObservableCollection<TreeViewModal>)[0].Clone);
            is_history_begin = false;
            current_index = history.Count - 1;
        }
    }
}

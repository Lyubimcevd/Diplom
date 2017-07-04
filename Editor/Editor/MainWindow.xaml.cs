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
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Editor
{
    public partial class MainWindow : Window
    {
        TreeViewModal buffer,current;
        List<TreeViewModal> history;
        ObservableCollection<TreeViewModal> Root;
        OpenFileDialog OFD;
        SaveFileDialog SFD;
        BinaryFormatter BF;
        string save_path;
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
            history = new List<TreeViewModal>();
        }

        #region CommandBinding

        void CommandBinding_New(object sender, ExecutedRoutedEventArgs e)
        {
            Root = new ObservableCollection<TreeViewModal>();
            Root.Add(TreeViewModal.NewItem(null));
            if (Root[0].Naim != null)
            {
                tree.ItemsSource = Root;
                Historian();
                is_history_begin = true;
                is_open = true;
                is_save = false;
            }
        }

        void CommandBinding_Open(object sender, ExecutedRoutedEventArgs e)
        {
            OFD = new OpenFileDialog();
            OFD.Filter = "ГОСТ (*.gstx)|*.gstx|Все файлы (*.*)|*.*";
            OFD.RestoreDirectory = true;

            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {               
                if (!is_save)
                    if (System.Windows.MessageBox.Show("Сохранить изменения?", "АРМ Эксперта Редактор",
                        MessageBoxButton.YesNoCancel) == MessageBoxResult.OK) CommandBinding_Save(null, null);
                save_path = OFD.FileName;
                BF = new BinaryFormatter();
                using (FileStream fs = new FileStream(save_path, FileMode.OpenOrCreate))
                {
                    Root = new ObservableCollection<TreeViewModal>();
                    Root.Add(BF.Deserialize(fs) as TreeViewModal);
                }
                tree.ItemsSource = Root;

                is_open = true;
                is_save = true;
                history = new List<TreeViewModal>();
                is_history_begin = true;
                is_history_end = true;
            }
        }

        void CommandBinding_Save(object sender, ExecutedRoutedEventArgs e)
        {
            BF = new BinaryFormatter();
            if (save_path == null) CommandBinding_SaveAs(null, null);
            else
            {
                using (FileStream fs = new FileStream(save_path, FileMode.OpenOrCreate))
                {
                    BF.Serialize(fs, Root[0]);
                }
                is_save = true;
            }
        }

        void CommandBinding_SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            BF = new BinaryFormatter();           
            SFD = new SaveFileDialog();
            SFD.Filter = "ГОСТ (*.gstx)|*.gstx|Все файлы (*.*)|*.*";
            SFD.RestoreDirectory = true;
            if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                save_path = SFD.FileName;
                using (FileStream fs = new FileStream(save_path, FileMode.OpenOrCreate))
                {
                    BF.Serialize(fs, Root[0]);
                }
                is_save = true;
            }
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

        private void Add_New_Element(object sender, RoutedEventArgs e)
        {
            current = (sender as System.Windows.Controls.MenuItem).DataContext as TreeViewModal;
            current.Children.Add(TreeViewModal.NewItem(current));
            Historian();
            is_save = false;
        }
        void Historian()
        {
            history.Add(Root[0].Clone);
            is_history_begin = false;
            current_index = history.Count - 1;
        }

        private void Rename(object sender, RoutedEventArgs e)
        {
            WindowForEditNaim WFEN = new WindowForEditNaim(tree.SelectedItem as TreeViewModal);
            WFEN.ShowDialog();
            is_save = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!is_save)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Сохранить изменения?", "АРМ Эксперта Редактор",
                    MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.OK) CommandBinding_Save(null, null);
                if (result == MessageBoxResult.Cancel) e.Cancel = true;
            }
        }

        private void Rename_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                current.IsExpanded = !current.IsExpanded;
                Rename(null, null);
            }
        }
    }
}

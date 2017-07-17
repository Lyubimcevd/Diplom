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
        TreeViewItem current_item;
        TitleViewModal windows_title;
        List<TreeViewModal> history;
        ObservableCollection<TreeViewModal> Root;
        OpenFileDialog OFD;
        SaveFileDialog SFD;
        BinaryFormatter BF;
        SHDocVw.InternetExplorer IE;
        AboutBox AB;
        StreamWriter SW;
        StreamReader SR;
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
            windows_title = new TitleViewModal("АРМ Эксперта Редактор");
            this.DataContext = windows_title;
        }

        #region CommandBinding

        void CommandBinding_New(object sender, ExecutedRoutedEventArgs e)
        {
            if (!is_save)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Сохранить изменения?", "АРМ Эксперта Редактор",
                    MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes) CommandBinding_Save(null, null);
                if (result == MessageBoxResult.Cancel) return;
            }          
            NewTree();
            windows_title.Title = "АРМ Эксперта Редактор*";
            tree.ItemsSource = Root;
            Historian();
            is_history_begin = true;
            is_open = true;
            is_save = false;  
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
                    Root.Add(new TreeViewModal(BF.Deserialize(fs) as SaveClass,null));
                }
                tree.ItemsSource = Root;
                windows_title.Title = "АРМ Эксперта Редактор: "+OFD.FileName;

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
                    BF.Serialize(fs, new SaveClass(Root[0]));
                }

                windows_title.Title = "АРМ Эксперта Редактор: " + SFD.FileName;

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
            NotSave();
        }

        void CommandBinding_Forward(object sender, ExecutedRoutedEventArgs e)
        {
            current_index++;
            Root = new ObservableCollection<TreeViewModal>();
            Root.Add(history[current_index]);
            tree.ItemsSource = Root;
            if (current_index == history.Count-1) is_history_end = true;
            is_history_begin = false;
            NotSave();
        }

        void CommandBinding_Cut(object sender, ExecutedRoutedEventArgs e)
        {
            buffer = current.Clone;
            current.Parent.Children.Remove(current);
            Historian();
            is_buffer = true;
            NotSave();
        }

        void CommandBinding_Copy(object sender, ExecutedRoutedEventArgs e)
        {
            buffer = current.Clone;
            is_buffer = true;
        }

        void CommandBinding_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            TreeViewModal tmp = buffer.Clone;
            tmp.Parent = current;
            current.Children.Add(tmp);
            current_item.IsExpanded = true;
            Historian();
            NotSave();
        }

        void CommandBinding_Rename(object sender, ExecutedRoutedEventArgs e)
        {
            WindowForEditNaim WFEN = new WindowForEditNaim(current);
            WFEN.ShowDialog();
            NotSave();
        }

        void CommandBinding_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            if (is_select)
            {
                current.Parent.Children.Remove(current);
                NotSave();
            }
        }

        void CommandBinding_Help(object sender, ExecutedRoutedEventArgs e)
        {
            IE = new SHDocVw.InternetExplorer();    
            IE.Navigate(System.Windows.Forms.Application.StartupPath + @"\Help\Help.html");
            IE.Visible = true;
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

        private void Rename_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = is_select;
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

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AB = new AboutBox();
            AB.ShowDialog();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OFD = new OpenFileDialog();
            OFD.Filter = "Текстовый файл (*.txt)|*.txt|Все файлы (*.*)|*.*";
            OFD.RestoreDirectory = true;

            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!is_save)
                    if (System.Windows.MessageBox.Show("Сохранить изменения?", "АРМ Эксперта Редактор",
                        MessageBoxButton.YesNoCancel) == MessageBoxResult.OK) CommandBinding_Save(null, null);
                save_path = null;
                NewTree();
                SR = new System.IO.StreamReader(OFD.FileName);
                string line;
                int deep;
                TreeViewModal tmp;
                while ((line = SR.ReadLine()) != null)
                {
                    tmp = Root[0];
                    deep = Convert.ToInt32(line[0].ToString());
                    for (int i = 0; i < deep; i++) tmp = tmp.Children.Last();
                    tmp.Children.Add(new TreeViewModal(line.Remove(0, 2), tmp));
                }
                SR.Close();
                tree.ItemsSource = Root;

                is_open = true;
                is_save = true;
                history = new List<TreeViewModal>();
                is_history_begin = true;
                is_history_end = true;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            SFD = new SaveFileDialog();
            SFD.Filter = "Текстовый файл (*.txt)|*.txt|Все файлы (*.*)|*.*";
            SFD.RestoreDirectory = true;
            if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SW = new StreamWriter(SFD.FileName);
                RecursyExport(Root[0],0);
                SW.Close();
            }
        }

        private void RecursyExport(TreeViewModal root,int level)
        {
            foreach (TreeViewModal el in root.Children)
            {
                SW.WriteLine(level.ToString() + " " + el.Naim);
                RecursyExport(el,level+1);
            }
        }

        private void TreeViewItemExpanded(object sender, RoutedEventArgs e)
        {
            current = (sender as TreeViewItem).DataContext as TreeViewModal;
            if (current.IsRenamed)
            {
                current.IsRenamed = false;
                (sender as TreeViewItem).IsExpanded = false;
            }
            e.Handled = true;
        }

        private void TreeViewItemCollapsed(object sender, RoutedEventArgs e)
        {
            current = (sender as TreeViewItem).DataContext as TreeViewModal;
            if (current.IsRenamed)
            {
                current.IsRenamed = false;
                (sender as TreeViewItem).IsExpanded = true;
            }
            e.Handled = true;
        }

        private void TreeViewItemPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            current_item = sender as TreeViewItem;
        }

        private void TreeViewItemPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            current_item = sender as TreeViewItem;
            current = (sender as TreeViewItem).DataContext as TreeViewModal;
            if (current != Root[0]) is_select = true;
        }

        private void Add_New_Element(object sender, RoutedEventArgs e)
        {
            current = (sender as System.Windows.Controls.MenuItem).DataContext as TreeViewModal;
            TreeViewModal tmp = TreeViewModal.NewItem(current);
            if (tmp.Naim != null)
            {
                current.Children.Add(tmp);
                current_item.IsExpanded = true;
                Historian();
                NotSave();
            }
        }

        void Historian()
        {
            history.Add(Root[0].Clone);
            is_history_begin = false;
            current_index = history.Count - 1;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!is_save)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Сохранить изменения?", "АРМ Эксперта Редактор",
                    MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes) CommandBinding_Save(null, null);
                if (result == MessageBoxResult.Cancel) e.Cancel = true;
            }
        }

        private void Rename_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                if (is_select)
                {
                    CommandBinding_Rename(null, null);
                    current.IsRenamed = true;
                }
        }

        void NotSave()
        {
            is_save = false;
            if (windows_title.Title.Last()!='*') windows_title.Title += "*";
        }

        void NewTree()
        {
            Root = new ObservableCollection<TreeViewModal>();
            Root.Add(new TreeViewModal("Показатели качества", null));
        }
    }
}

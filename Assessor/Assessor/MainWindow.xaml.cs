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
using Assessor.Classes;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Json;
using Assessor.Windows;

namespace Assessor
{
    public partial class MainWindow : Window
    {
        TitleViewModal windows_title;
        TreeViewExpertModal current;
        ObservableCollection<TreeViewExpertModal> Root;
        OpenFileDialog OFD;
        SaveFileDialog SFD;
        DataContractJsonSerializer BF;
        string save_path;
        bool is_save = true,
            is_open = false;

        public MainWindow()
        {
            InitializeComponent();
            windows_title = new TitleViewModal("АРМ Эксперта Оценщик");
            this.DataContext = windows_title;
        }

        void CommandBinding_New(object sender, ExecutedRoutedEventArgs e)
        {
            OFD = new OpenFileDialog();
            OFD.Filter = "ГОСТ (*.gstx)|*.gstx|Все файлы (*.*)|*.*";
            OFD.RestoreDirectory = true;

            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!is_save)
                    if (System.Windows.MessageBox.Show("Сохранить изменения?", "АРМ Эксперта Оценщик",
                        MessageBoxButton.YesNoCancel) == MessageBoxResult.OK) CommandBinding_Save(null, null);
                save_path = OFD.FileName;
                BF = new DataContractJsonSerializer(typeof(SaveClass));
                using (FileStream fs = new FileStream(save_path, FileMode.Open))
                {
                    Root = new ObservableCollection<TreeViewExpertModal>();
                    Root.Add(new TreeViewExpertModal(BF.ReadObject(fs) as SaveClass, null));
                }
                tree.ItemsSource = Root;
                windows_title.Title = "АРМ Эксперта Оценщик: " + OFD.FileName;

                is_open = true;
                is_save = true;
            }
        }

        void CommandBinding_Open(object sender, ExecutedRoutedEventArgs e)
        {
            OFD = new OpenFileDialog();
            OFD.Filter = "ГОСТ (*.gstm)|*.gstm|Все файлы (*.*)|*.*";
            OFD.RestoreDirectory = true;

            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!is_save)
                    if (System.Windows.MessageBox.Show("Сохранить изменения?", "АРМ Эксперта Оценщик",
                        MessageBoxButton.YesNoCancel) == MessageBoxResult.OK) CommandBinding_Save(null, null);
                save_path = OFD.FileName;
                BF = new DataContractJsonSerializer(typeof(SaveClass));
                using (FileStream fs = new FileStream(save_path, FileMode.Open))
                {
                    Root = new ObservableCollection<TreeViewExpertModal>();
                    Root.Add(new TreeViewExpertModal(BF.ReadObject(fs) as SaveClassExpert, null));
                }
                tree.ItemsSource = Root;
                windows_title.Title = "АРМ Эксперта Оценщик: " + OFD.FileName;

                is_open = true;
                is_save = true;
            }
        }

        void CommandBinding_Save(object sender, ExecutedRoutedEventArgs e)
        {
            BF = new DataContractJsonSerializer(typeof(SaveClass));
            if (save_path == null) CommandBinding_SaveAs(null, null);
            else
            {
                using (FileStream fs = new FileStream(save_path, FileMode.OpenOrCreate))
                {
                    BF.WriteObject(fs, Root[0].Save);
                }
                is_save = true;
            }
        }

        void CommandBinding_SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            BF = new DataContractJsonSerializer(typeof(SaveClass));
            SFD = new SaveFileDialog();
            SFD.Filter = "ГОСТ (*.gstm)|*.gstm|Все файлы (*.*)|*.*";
            SFD.RestoreDirectory = true;
            if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                save_path = SFD.FileName;
                using (FileStream fs = new FileStream(save_path, FileMode.OpenOrCreate))
                {
                    BF.WriteObject(fs, Root[0].Save);
                }
                windows_title.Title = "АРМ Эксперта Оценщик: " + SFD.FileName;

                is_save = true;
            }
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !is_save;
        }

        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = is_open;
        }

        private void TreeViewItemExpanded(object sender, RoutedEventArgs e)
        {
            current = (sender as TreeViewItem).DataContext as TreeViewExpertModal;
            if (current.IsDoubleClick)
            {
                current.IsDoubleClick = false;
                (sender as TreeViewItem).IsExpanded = false;
            }
            e.Handled = true;
        }

        private void TreeViewItemCollapsed(object sender, RoutedEventArgs e)
        {
            current = (sender as TreeViewItem).DataContext as TreeViewExpertModal;
            if (current.IsDoubleClick)
            {
                current.IsDoubleClick = false;
                (sender as TreeViewItem).IsExpanded = true;
            }
            e.Handled = true;
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            current = (sender as TextBlock).DataContext as TreeViewExpertModal;
            if (e.ClickCount == 2)
            {
                SliderWindow SW = new SliderWindow();
                SW.main.DataContext = current;
                SW.ShowDialog();
                current.IsDoubleClick = true;
            }
        }
    }
}

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
        SHDocVw.InternetExplorer IE;
        AboutBox AB;
        MessageBoxResult result;
        string save_path;
        bool is_save = true,
             is_open = false,
             cancel_combo_box = true,
             open_file = false;

        public MainWindow()
        {
            InitializeComponent();
            windows_title = new TitleViewModal("АРМ Эксперта Оценщик");
            this.DataContext = windows_title;
            WorkMode.IsExpert = true;
        }

        void CommandBinding_New(object sender, ExecutedRoutedEventArgs e)
        {
            OFD = new OpenFileDialog();
            if (WorkMode.IsExpert) OFD.Filter = "ГОСТ (*.gstm)|*.gstm|Все файлы (*.*)|*.*";
            else OFD.Filter = "ГОСТ (*.gstx)|*.gstx|Все файлы (*.*)|*.*";
            open_file = false;
            OFD.RestoreDirectory = true;

            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!is_save)
                {
                    result = System.Windows.MessageBox.Show("Сохранить изменения?", "АРМ Эксперта Оценка",
                        MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.OK) CommandBinding_Save(null, null);
                    if (result == MessageBoxResult.Cancel) return;
                }
                save_path = OFD.FileName;
                BF = new DataContractJsonSerializer(typeof(SaveClass));
                using (FileStream fs = new FileStream(save_path, FileMode.Open))
                {
                    Root = new ObservableCollection<TreeViewExpertModal>();
                    Root.Add(new TreeViewExpertModal(BF.ReadObject(fs) as SaveClass, null));
                }
                tree.ItemsSource = Root;
                windows_title.Title = "АРМ Эксперта Оценка: " + OFD.FileName;

                is_open = true;
                is_save = true;
            }
        }

        void CommandBinding_Open(object sender, ExecutedRoutedEventArgs e)
        {
            OFD = new OpenFileDialog();
            if (WorkMode.IsExpert) OFD.Filter = "ГОСТ (*.gstme)|*.gstme|Все файлы (*.*)|*.*";
            else OFD.Filter = "ГОСТ (*.gstm)|*.gstm|Все файлы (*.*)|*.*";
            open_file = true;
            OFD.RestoreDirectory = true;
            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!is_save)
                {
                    result = System.Windows.MessageBox.Show("Сохранить изменения?", "АРМ Эксперта Оценка", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.OK) CommandBinding_Save(null, null);
                    if (result == MessageBoxResult.Cancel) return;
                }
                save_path = OFD.FileName;
                BF = new DataContractJsonSerializer(typeof(SaveClassExpert));
                using (FileStream fs = new FileStream(save_path, FileMode.Open))
                {
                    Root = new ObservableCollection<TreeViewExpertModal>();
                    Root.Add(new TreeViewExpertModal(BF.ReadObject(fs) as SaveClassExpert, null));
                }
                tree.ItemsSource = Root;
                windows_title.Title = "АРМ Эксперта Оценка: " + OFD.FileName;

                is_open = true;
                is_save = true;
            }
        }

        void CommandBinding_Save(object sender, ExecutedRoutedEventArgs e)
        {
            BF = new DataContractJsonSerializer(typeof(SaveClassExpert));
            if (WorkMode.IsExpert)
                if (!open_file) CommandBinding_SaveAs(null, null);
                else SaveInFile();
            else
                if (!open_file) CommandBinding_SaveAs(null, null);
                else SaveInFile();
        }

        void CommandBinding_SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            BF = new DataContractJsonSerializer(typeof(SaveClassExpert));
            SFD = new SaveFileDialog();
            if (WorkMode.IsExpert) SFD.Filter = "ГОСТ (*.gstme)|*.gstme|Все файлы (*.*)|*.*";
            else SFD.Filter = "ГОСТ (*.gstm)|*.gstm|Все файлы (*.*)|*.*";
            SFD.RestoreDirectory = true;
            if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                save_path = SFD.FileName;
                SaveInFile();
            }
        }

        void CommandBinding_Help(object sender, ExecutedRoutedEventArgs e)
        {
            IE = new SHDocVw.InternetExplorer();
            IE.Navigate(System.Windows.Forms.Application.StartupPath + @"\Help\Help.html");
            IE.Visible = true;
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

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AB = new AboutBox();
            AB.ShowDialog();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cancel_combo_box)
                if (is_open)
                {
                    result = System.Windows.MessageBox.Show("При смене режима работы будет закрыт текущий файл без сохранения. Продолжить?", "АРМ Эксперта Оценка", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes) CloseCurrentFile();
                    else
                    {
                        cancel_combo_box = false;
                        (sender as System.Windows.Controls.ComboBox).SelectedItem = e.RemovedItems[0];
                        return;
                    }
                }
                if ((sender as System.Windows.Controls.ComboBox).Text == "Администратор") WorkMode.IsExpert = true;
                else WorkMode.IsExpert = false;
            cancel_combo_box = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!is_save)
            {
                result = System.Windows.MessageBox.Show("Сохранить изменения?", "АРМ Эксперта Оценка", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.OK) CommandBinding_Save(null, null);
                if (result == MessageBoxResult.Cancel) e.Cancel = true;
            }
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            current = (sender as TextBlock).DataContext as TreeViewExpertModal;
            if (e.ClickCount == 2)
            {
                SliderWindow SW = new SliderWindow(current);
                SW.ShowDialog();
                current.IsDoubleClick = true;
                is_save = false;
            }
        }

        void CloseCurrentFile()
        {
            windows_title.Title = "АРМ Эксперта Оценка";
            Root.Clear();
            save_path = null;
            is_open = false;
            is_save = true;
        }

        void SaveInFile()
        {
            using (FileStream fs = new FileStream(save_path, FileMode.Create))
            {
                BF.WriteObject(fs, Root[0].Save);
            }
            windows_title.Title = "АРМ Эксперта Оценка: " + save_path;
            is_save = true;
        }
    }
}

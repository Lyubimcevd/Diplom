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
using System.Windows.Shapes;
using ARMExperta.Classes;
using System.Collections.ObjectModel;

namespace ARMExperta.Windows
{
    public partial class MainWindow : Window
    {
        MessageBoxResult result;
        ObservableCollection<TreeViewModal> Root;
        

        public MainWindow()
        {
            InitializeComponent();
            DataContext = TitleModal.GetTitle;
        }

        #region CommandBinding

        void CommandBinding_New(object sender, ExecutedRoutedEventArgs e)
        {
            if (!CurrentSystemStatus.GetSS.IsSave)
            {
                result = System.Windows.MessageBox.Show("Сохранить изменения?", "АРМ Эксперта Редактор", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes) CommandBinding_Save(null, null);
                if (result == MessageBoxResult.Cancel) return;
            }
            Root = new ObservableCollection<TreeViewModal>();
            Root.Add(new TreeViewModal("Показатели качества"));
            tree.ItemsSource = Root;
            Historian();
            is_history_begin = true;
            is_history_end = true;
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
            
        }

        void CommandBinding_Forward(object sender, ExecutedRoutedEventArgs e)
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

        void CommandBinding_Rename(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        void CommandBinding_Delete(object sender, ExecutedRoutedEventArgs e)
        {
           
        }

        void CommandBinding_Help(object sender, ExecutedRoutedEventArgs e)
        {

        }

        void CommandBinding_Close(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region CanExecute

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
            e.CanExecute = is_buffer && is_select;
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
           
        }

        private void Add_New_Element(object sender, RoutedEventArgs e)
        {
           
        }
    }
}

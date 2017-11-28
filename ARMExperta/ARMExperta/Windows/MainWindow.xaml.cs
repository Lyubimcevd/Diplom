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
            DataContext = CurrentSystemStatus.GetSS;
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
            CurrentSystemStatus.GetSS.Tree.Clear();
            CurrentSystemStatus.GetSS.Tree.Add(new TreeViewModal("Показатели качества"));
            Root = new ObservableCollection<TreeViewModal>();
            Root.Add(CurrentSystemStatus.GetSS.Tree.First());
            tree.ItemsSource = Root;
            CurrentSystemStatus.GetSS.AddInHistory();        
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
            WindowForEditNaim WFEN = new WindowForEditNaim(CurrentSystemStatus.GetSS.CurrentElement);
            WFEN.ShowDialog();
            Root[0].Update();
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
            e.CanExecute = !CurrentSystemStatus.GetSS.IsSave;
        }

        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CurrentSystemStatus.GetSS.IsOpen;
        }

        private void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentElement != null) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentElement != null) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentElement != null&& CurrentSystemStatus.GetSS.IsBuffer) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Rename_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentElement != null) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentPosInHistory > 0) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Forward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentPosInHistory < CurrentSystemStatus.GetSS.History.Count-1)
            e.CanExecute = true;
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentElement != null) e.CanExecute = true;
            else e.CanExecute = false;
        }

        #endregion

        private void About_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Add_New_Element(object sender, RoutedEventArgs e)
        {
            TreeViewModal tmp = new TreeViewModal();
            WindowForEditNaim WFEN = new WindowForEditNaim(tmp);
            WFEN.ShowDialog();
            if (tmp.Naim != null)
            {
                tmp.Id = CurrentSystemStatus.GetSS.GetNewId();
                tmp.ParentId = CurrentSystemStatus.GetSS.CurrentElement.Id;
                CurrentSystemStatus.GetSS.Tree.Add(tmp);
                Root[0].Update();
                CurrentSystemStatus.GetSS.AddInHistory();
                CurrentSystemStatus.GetSS.IsSave = false;
            }
        }

        private void TreeViewItemCollapsed(object sender, RoutedEventArgs e)
        {
            ((sender as TreeViewItem).DataContext as TreeViewModal).IsExpanded = false;
        }

        private void TreeViewItemExpanded(object sender, RoutedEventArgs e)
        {
            ((sender as TreeViewItem).DataContext as TreeViewModal).IsExpanded = true;
        }

        private void TextBlock_PreviewMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            CurrentSystemStatus.GetSS.CurrentElement = (sender as TextBlock).DataContext as TreeViewModal;
            if (e.ClickCount == 2) CommandBinding_Rename(null, null);
        }      
    }
}

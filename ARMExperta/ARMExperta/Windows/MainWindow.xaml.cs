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
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.IO;

namespace ARMExperta.Windows
{
    public partial class MainWindow : Window
    {
        WorkGroups WG;

        public MainWindow()
        {
            Login LG = new Login();
            while (LG.EnterUser == null)
            {
                LG.ShowDialog();
                if (LG.EnterUser == null)
                {
                    System.Windows.MessageBox.Show("Неверный логин или пароль", "АРМ Эксперта");
                    LG = new Login();
                }
                else
                    if (LG.EnterUser.Id == -1) Environment.Exit(0);              
            }
            CurrentSystemStatus.GetSS.CurrentUser = LG.EnterUser;
            InitializeComponent();
            DataContext = CurrentSystemStatus.GetSS;
            if (!CurrentSystemStatus.GetSS.CurrentUser.IsGroup)
            {
                Admininstr.Visibility = Visibility.Visible;
                Open_menu_work_group.Visibility = Visibility.Collapsed;
                Open_menu_admin.Visibility = Visibility.Visible;
                Save_menu_work_group.Visibility = Visibility.Collapsed;
                Save_menu_admin.Visibility = Visibility.Visible;
                chat.Visibility = Visibility.Collapsed;
            }
            else Ready.Visibility = Visibility.Visible;
        }

        #region CommandBinding

        void CommandBinding_New(object sender, ExecutedRoutedEventArgs e)
        {
            if (CheckSave())
            {
                CurrentSystemStatus.GetSS.UpdateTitle();
                CurrentSystemStatus.GetSS.DictionaryTree.Clear();
                StartWork();
            }
        }

        void CommandBinding_Open(object sender, ExecutedRoutedEventArgs e)
        {
            if (CheckSave()) OpenUserModel(CurrentSystemStatus.GetSS.CurrentUser);
        }

        void CommandBinding_Save(object sender, ExecutedRoutedEventArgs e)
        {
            if (!CurrentSystemStatus.GetSS.CurrentUser.IsGroup)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Сохранить как ГОСТ?", "АРМ Эксперта", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    Save_gost_Click(null, null);
                    return;
                }
                if (result == MessageBoxResult.Cancel) return;
            }
            Server.GetServer.SaveOnServer(CurrentSystemStatus.GetSS.CurrentUser);
            System.Windows.MessageBox.Show("Сохранено", "АРМ Эксперта");
            CurrentSystemStatus.GetSS.IsSave = true;
        }

        void CommandBinding_Undo(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentSystemStatus.GetSS.UndoHistory();
            CurrentSystemStatus.GetSS.IsSave = false;
        }

        void CommandBinding_Forward(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentSystemStatus.GetSS.ForwardHistory();
            CurrentSystemStatus.GetSS.IsSave = false;
        }

        void CommandBinding_Cut(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentSystemStatus.GetSS.ClearBuffer();
            CurrentSystemStatus.GetSS.CutFromTree(CurrentSystemStatus.GetSS.CurrentElement);
            Update();
        }

        void CommandBinding_Copy(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentSystemStatus.GetSS.ClearBuffer();
            CurrentSystemStatus.GetSS.CopyFromTree(CurrentSystemStatus.GetSS.CurrentElement);
        }

        void CommandBinding_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentSystemStatus.GetSS.PasteFromBuffer();
            Update();
        }

        void CommandBinding_Rename(object sender, ExecutedRoutedEventArgs e)
        {
            WindowForEdit WFE = new WindowForEdit(CurrentSystemStatus.GetSS.CurrentElement.Naim);
            WFE.ShowDialog();
            if (WFE.Result.Trim().Length != 0) CurrentSystemStatus.GetSS.CurrentElement.Naim = WFE.Result;
            Update();
        }

        void CommandBinding_Estimate(object sender, ExecutedRoutedEventArgs e)
        {
            SliderWindow SW = new SliderWindow();
            SW.ShowDialog();
            CurrentSystemStatus.GetSS.IsSave = false;
        }

        void CommandBinding_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentSystemStatus.GetSS.DeleteSubTree(CurrentSystemStatus.GetSS.CurrentElement);
            Update();
        }

        void CommandBinding_Help(object sender, ExecutedRoutedEventArgs e)
        {
            Help.ShowHelp(sender as System.Windows.Forms.Control, "help.chm");
        }

        void CommandBinding_Close(object sender, ExecutedRoutedEventArgs e)
        {
            if (CheckSave()) this.Close();
        }

        void CommandBinding_About(object sender, ExecutedRoutedEventArgs e)
        {
           
        }

        void CommandBinding_AddElement(object sender, ExecutedRoutedEventArgs e)
        {
            WindowForEdit WFE = new WindowForEdit();
            WFE.ShowDialog();            
            if (WFE.Result.Trim().Length != 0)
            {
                TreeViewModal tmp = new TreeViewModal();
                tmp.Naim = WFE.Result;
                tmp.Id = CurrentSystemStatus.GetSS.DictionaryTree.Keys.Max() + 1;
                tmp.ParentId = CurrentSystemStatus.GetSS.CurrentElement.Id;
                CurrentSystemStatus.GetSS.DictionaryTree.Add(tmp.Id, tmp);
                Update();
            }
        }

        void CommandBinding_ChangeWorkMode(object sender, ExecutedRoutedEventArgs e)
        {
            if (CheckSave())
                CurrentSystemStatus.GetSS.IsExpert = !CurrentSystemStatus.GetSS.IsExpert;
            foreach (TreeViewModal tvm in CurrentSystemStatus.GetSS.DictionaryTree.Values) tvm.UpdateReady();
                
        }

        void CommandBinding_Print(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Отработало нажатие");
            Print.GetPrint.PrintDocument(CurrentSystemStatus.GetSS.Tree[0]);
        }

        void CommandBinding_Ready(object sender, ExecutedRoutedEventArgs e)
        {
            string mes = "Группа: " + CurrentSystemStatus.GetSS.CurrentUser.Naim;
            if (!CurrentSystemStatus.GetSS.CurrentUser.IsReady)
            {
                mes += " готова!";
                Server.GetServer.SetReadyOfWorkGroup(true, CurrentSystemStatus.GetSS.CurrentUser.Id);
            }
            else
            {
                mes += " не готова!";
                Server.GetServer.SetReadyOfWorkGroup(false, CurrentSystemStatus.GetSS.CurrentUser.Id);
            }
            List<User> admins = Server.GetServer.GetAdmins();
            foreach (User us in admins) Server.GetServer.SendMessage(mes, CurrentSystemStatus.GetSS.CurrentUser.Id, us.Id ,false);
            CurrentSystemStatus.GetSS.UpdateColorReady();
        }

        void CommandBinding_EducationGroup(object sender, ExecutedRoutedEventArgs e)
        {
            EducationGroups EG = new EducationGroups();
            EG.ShowDialog();
        }

        void CommandBinding_WorkGroup(object sender, ExecutedRoutedEventArgs e)
        {
            WG = new WorkGroups(this);
            WG.Show();
        }

        void CommandBinding_Admins(object sender, ExecutedRoutedEventArgs e)
        {
            Admins Adm = new Admins();
            Adm.ShowDialog();
        }

        #endregion

        #region CanExecute

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !CurrentSystemStatus.GetSS.IsSave;
            Save_gost.IsEnabled = e.CanExecute;
        }

        private void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentElement != null
                &&!CurrentSystemStatus.GetSS.IsExpert
                &&CurrentSystemStatus.GetSS.CurrentElement.Parent != null) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentElement != null
                &&!CurrentSystemStatus.GetSS.IsExpert
                &&CurrentSystemStatus.GetSS.CurrentElement.Parent != null) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentElement != null
                &&CurrentSystemStatus.GetSS.IsBuffer&&!CurrentSystemStatus.GetSS.IsExpert) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Rename_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentElement != null
                &&!CurrentSystemStatus.GetSS.IsExpert
                &&CurrentSystemStatus.GetSS.CurrentElement.Parent != null) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Estimate_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        { 
            if (CurrentSystemStatus.GetSS.CurrentElement != null&&CurrentSystemStatus.GetSS.CurrentUser.GOST == 0)
            {
                if (!CurrentSystemStatus.GetSS.IsExpert)
                {
                    if (CurrentSystemStatus.GetSS.CurrentElement.Parent != null) e.CanExecute = true;
                    else e.CanExecute = false;
                }
                else
                {
                    if (CurrentSystemStatus.GetSS.CurrentElement.Children.Count == 0) e.CanExecute = true;
                    else e.CanExecute = false;
                }
            }
            else e.CanExecute = false;
        }

        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CanUndo) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Forward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CanForward) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentElement != null
                &&!CurrentSystemStatus.GetSS.IsExpert
                &&CurrentSystemStatus.GetSS.CurrentElement.Parent != null) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void AddElement_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentElement != null
                &&!CurrentSystemStatus.GetSS.IsExpert) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Print_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.IsExpert) e.CanExecute = true;
            else e.CanExecute = false;
        }

        #endregion

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
        }      

        bool CheckSave()
        {
            if (!CurrentSystemStatus.GetSS.IsSave)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Сохранить изменения?", "АРМ Эксперта", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes) CommandBinding_Save(null, null);
                if (result == MessageBoxResult.Cancel) return false;
                else
                {
                    CurrentSystemStatus.GetSS.IsSave = true;
                    return true;
                }
            }
            else return true;
        }

        void StartWork()
        {
            CurrentSystemStatus.GetSS.ClearHistory();         
            CurrentSystemStatus.GetSS.DictionaryTree.Add(0,new TreeViewModal("Показатели качества"));
            tree.ItemsSource = CurrentSystemStatus.GetSS.Tree;
            CurrentSystemStatus.GetSS.AddInHistory();            
        }

        void Update()
        {
            CurrentSystemStatus.GetSS.AddInHistory();
            tree.ItemsSource = CurrentSystemStatus.GetSS.Tree;
            CurrentSystemStatus.GetSS.IsSave = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CheckSave()) e.Cancel = true;
            if (WG != null) WG.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "ГОСТ (*.gstx)|*.gstx|Все файлы (*.*)|*.*";
            OFD.RestoreDirectory = true;

            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataContractJsonSerializer BF = new DataContractJsonSerializer(typeof(SaveClass));
                using (FileStream fs = new FileStream(OFD.FileName, FileMode.Open)) ReadSaveClass(BF.ReadObject(fs) as SaveClass,0);
                CurrentSystemStatus.GetSS.DictionaryTree.Add(0, new TreeViewModal("Показатели качества"));
                Update();
            }
        }

        void ReadSaveClass(SaveClass sc,int par_id)
        {
            TreeViewModal tmp = new TreeViewModal(sc.Naim);
            if (CurrentSystemStatus.GetSS.DictionaryTree.Count != 0) tmp.Id = CurrentSystemStatus.GetSS.DictionaryTree.Keys.Max() + 1;
            else tmp.Id = 1;
            tmp.ParentId = par_id;
            CurrentSystemStatus.GetSS.DictionaryTree.Add(tmp.Id, tmp);
            foreach (SaveClass sc_tmp in sc.Children) ReadSaveClass(sc_tmp, tmp.Id);
        }

        private void Open_gost_Click(object sender, RoutedEventArgs e)
        {
            if (CheckSave())
            {
                Choice Ch = new Choice(Server.GetServer.GetGOSTs());
                Ch.ShowDialog();
                if (Ch.Result != 0)
                {
                    CurrentSystemStatus.GetSS.CurrentUser.GOST = Ch.Result;
                    CurrentSystemStatus.GetSS.UpdateTitle();
                    CurrentSystemStatus.GetSS.DictionaryTree.Clear();
                    Server.GetServer.GetModalByUser(CurrentSystemStatus.GetSS.CurrentUser);
                    StartWork();
                }
            }
        }

        private void Save_gost_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Введите название ГОСТа", "АРМ Эксперта");
            WindowForEdit WFE = new WindowForEdit();
            WFE.ShowDialog();
            if (CurrentSystemStatus.GetSS.StringIsCorrect(WFE.Result))
            {               
                CurrentSystemStatus.GetSS.CurrentUser.GOST = Server.GetServer.AddNewGOST(WFE.Result.Trim());
                Server.GetServer.SaveOnServer(CurrentSystemStatus.GetSS.CurrentUser);
                System.Windows.MessageBox.Show("Сохранено", "АРМ Эксперта");
                CurrentSystemStatus.GetSS.CurrentUser.GOST = Server.GetServer.GetGOSTs().First(x=>x.Value == WFE.Result.Trim()).Key;
                CurrentSystemStatus.GetSS.IsSave = true;
            }
        }

        public void OpenUserModel(User user)
        {
            CurrentSystemStatus.GetSS.DictionaryTree.Clear();
            Server.GetServer.GetModalByUser(user);
            StartWork();
            foreach (TreeViewModal tvm in CurrentSystemStatus.GetSS.DictionaryTree.Values) tvm.UpdateReady();
        }

        private void chat_Click(object sender, RoutedEventArgs e)
        {
            Chat Ch = new Chat();
            Ch.ShowDialog();
        }
    }
}

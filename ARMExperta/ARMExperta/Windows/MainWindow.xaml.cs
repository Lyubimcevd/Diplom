﻿using System;
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
        ObservableCollection<TreeViewModal> Root;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = CurrentSystemStatus.GetSS;
        }

        #region CommandBinding

        void CommandBinding_New(object sender, ExecutedRoutedEventArgs e)
        {
            if (CheckSave()) StartWork();
        }

        void CommandBinding_Open(object sender, ExecutedRoutedEventArgs e)
        {
            if (CheckSave())
            {
                Server.GetServer.GetModalByUser(CurrentSystemStatus.GetSS.CurrentUser);
                StartWork();
            }    
        }

        void CommandBinding_Save(object sender, ExecutedRoutedEventArgs e)
        {
            Server.GetServer.SaveOnServer();
            MessageBox.Show("Сохранено","АРМ Эксперта");
            CurrentSystemStatus.GetSS.IsSave = true;
        }

        void CommandBinding_Undo(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentSystemStatus.GetSS.CurrentPosInHistory--;
            CurrentSystemStatus.GetSS.Tree = CurrentSystemStatus.GetSS.History[CurrentSystemStatus.GetSS.CurrentPosInHistory];
            Root[0].Update();
            CurrentSystemStatus.GetSS.IsSave = false;
        }

        void CommandBinding_Forward(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentSystemStatus.GetSS.CurrentPosInHistory++;
            CurrentSystemStatus.GetSS.Tree = CurrentSystemStatus.GetSS.History[CurrentSystemStatus.GetSS.CurrentPosInHistory];
            Root[0].Update();
            CurrentSystemStatus.GetSS.IsSave = false;
        }

        void CommandBinding_Cut(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentSystemStatus.GetSS.DeleteOldBuffer();
            CurrentSystemStatus.GetSS.CurrentElement.ParentId = -1;
            CurrentSystemStatus.GetSS.SetLikeBuffer(CurrentSystemStatus.GetSS.CurrentElement);
            Update();
        }

        void CommandBinding_Copy(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentSystemStatus.GetSS.DeleteOldBuffer();
            CurrentSystemStatus.GetSS.SetLikeBuffer(CurrentSystemStatus.GetSS.CopySubTree(CurrentSystemStatus.GetSS.CurrentElement, -1));
        }

        void CommandBinding_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            TreeViewModal tmp = null;
            foreach (TreeViewModal tvm in CurrentSystemStatus.GetSS.Tree)
                if (tvm.Parent == null && tvm.IsBuffer)
                {
                    tvm.ParentId = CurrentSystemStatus.GetSS.CurrentElement.Id;
                    CurrentSystemStatus.GetSS.SetNoBuffer(tvm);
                    tmp = tvm;
                }
            CurrentSystemStatus.GetSS.SetLikeBuffer(CurrentSystemStatus.GetSS.CopySubTree(tmp, -1));
            Update();
        }

        void CommandBinding_Rename(object sender, ExecutedRoutedEventArgs e)
        {
            WindowForEditNaim WFEN = new WindowForEditNaim(CurrentSystemStatus.GetSS.CurrentElement);
            WFEN.ShowDialog();
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
            TreeViewModal tmp = new TreeViewModal();
            WindowForEditNaim WFEN = new WindowForEditNaim(tmp);
            WFEN.ShowDialog();
            if (tmp.Naim != null)
            {
                tmp.Id = CurrentSystemStatus.GetSS.GetNewId();
                tmp.ParentId = CurrentSystemStatus.GetSS.CurrentElement.Id;
                CurrentSystemStatus.GetSS.Tree.Add(tmp);
                Update();
            }
        }

        void CommandBinding_ChangeWorkMode(object sender, ExecutedRoutedEventArgs e)
        {
            if (CheckSave())
            CurrentSystemStatus.GetSS.IsExpert = !CurrentSystemStatus.GetSS.IsExpert;
        }

        #endregion

        #region CanExecute

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !CurrentSystemStatus.GetSS.IsSave;
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
                &&CurrentSystemStatus.GetSS.IsBuffer
                &&!CurrentSystemStatus.GetSS.IsExpert) e.CanExecute = true;
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
            if (CurrentSystemStatus.GetSS.CurrentElement != null
                &&CurrentSystemStatus.GetSS.CurrentElement.Parent != null) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CurrentSystemStatus.GetSS.CurrentPosInHistory > 0
                &&!CurrentSystemStatus.GetSS.IsExpert) e.CanExecute = true;
            else e.CanExecute = false;
        }

        private void Forward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((CurrentSystemStatus.GetSS.CurrentPosInHistory < CurrentSystemStatus.GetSS.History.Count - 1)&&!CurrentSystemStatus.GetSS.IsExpert) e.CanExecute = true;
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
            if (e.ChangedButton == MouseButton.Left
                &&CurrentSystemStatus.GetSS.CurrentElement.Parent != null)
                if (e.ClickCount == 2) CommandBinding_Estimate(null, null);
        }      

        bool CheckSave()
        {
            if (!CurrentSystemStatus.GetSS.IsSave)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить изменения?", "АРМ Эксперта", MessageBoxButton.YesNoCancel);
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
            CurrentSystemStatus.GetSS.History.Clear();
            CurrentSystemStatus.GetSS.Tree = new ObservableCollection<TreeViewModal>(CurrentSystemStatus.GetSS.OldTree);
            CurrentSystemStatus.GetSS.Tree.Insert(0,new TreeViewModal("Показатели качества"));
            Root = new ObservableCollection<TreeViewModal>();
            Root.Add(CurrentSystemStatus.GetSS.Tree.First());
            tree.ItemsSource = Root;
            CurrentSystemStatus.GetSS.AddInHistory();
        }

        void Update()
        {
            Root[0].Update();
            CurrentSystemStatus.GetSS.AddInHistory();
            CurrentSystemStatus.GetSS.IsSave = false;
        }
    }
}

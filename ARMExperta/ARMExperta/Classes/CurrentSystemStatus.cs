using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ARMExperta.Classes
{
    class CurrentSystemStatus : INotifyPropertyChanged
    {
        Dictionary<int, TreeViewModal> dictionary_tree = new Dictionary<int, TreeViewModal>();
        Dictionary<int, TreeViewModal> buffer = new Dictionary<int, TreeViewModal>();
        bool is_save = true,
             is_expert = false;
        int current_pos = -1;
        List<Dictionary<int, TreeViewModal>> history = new List<Dictionary<int, TreeViewModal>>();
        User current_user;
        TreeViewModal current_element;

        static CurrentSystemStatus current_sys_stat;
        public event PropertyChangedEventHandler PropertyChanged;

        CurrentSystemStatus() { }

        public static CurrentSystemStatus GetSS
        {
            get
            {
                if (current_sys_stat == null) current_sys_stat = new CurrentSystemStatus();
                return current_sys_stat;
            }
        }

        public Dictionary<int,TreeViewModal> DictionaryTree
        {
            get
            {
                return dictionary_tree;
            }
            set
            {
                dictionary_tree = value;
            }
        }
        public ObservableCollection<TreeViewModal> Tree
        {
            get
            {
                ObservableCollection<TreeViewModal> result = new ObservableCollection<TreeViewModal>();
                result.Add(DictionaryTree[0]);
                return result;
            }
        }
        public bool IsSave
        {
            get
            {
                return is_save;
            }
            set
            {
                is_save = value;
                OnPropertyChanged("Title");
            }

        }
        public bool IsExpert
        {
            get
            {
                return is_expert;
            }
            set
            {
                is_expert = value;
                OnPropertyChanged("Title");
            }
        }
        public bool IsBuffer
        {
            get
            {
                if (buffer.Count != 0) return true;
                else return false;
            }
        }
        public TreeViewModal CurrentElement
        {
            get
            {
                return current_element;
            }
            set
            {
                current_element = value;
            }
        }
        public User CurrentUser
        {
            get
            {
                return current_user;
            }
            set
            {
                current_user = value;
            }
        }
        public string Title
        {
            get
            {
                string title = "АРМ Эксперта ";
                if (CurrentUser.IsGroup) title += " Группа : ";
                title += "(" + CurrentUser.Naim + ")";
                if (IsExpert) title += "(Эксперт)";
                else title += "(Администратор)";
                if (CurrentUser.IsGOST)
                    if (CurrentUser.GOST != null) title += " " + CurrentUser.GOST;
                    else title += " Новый ГОСТ";
                if (!IsSave) title += "*";
                return title;
            }
        }
        public bool CanUndo
        {
            get
            {
                if (current_pos != -1&&!IsExpert) return true;
                else return false;
            }
        }
        public bool CanForward
        {
            get
            {
                if (current_pos != history.Count - 1 && !IsExpert) return true;
                else return false;
            }
        }
       
        public void AddInHistory()
        {
            history.Add(DictionaryTree);
            current_pos++;
        }
        public void UndoHistory()
        {
            DictionaryTree = history[current_pos];
            current_pos--;
        }
        public void ForwardHistory()
        {
            DictionaryTree = history[current_pos];
            current_pos++;
        }
        public void CutFromTree(TreeViewModal tvm)
        {
            CopyFromTree(tvm);
            DeleteSubTree(tvm);
        }
        public void CopyFromTree(TreeViewModal tvm)
        {
            buffer.Add(tvm.Id, DictionaryTree[tvm.Id].Clone);
            foreach (TreeViewModal child in tvm.Children) CopyFromTree(child);
        }
        public void PasteFromBuffer()
        {
            DictionaryTree.Add(DictionaryTree.Keys.Max() + 1, buffer.First().Value.Clone);
            DictionaryTree[DictionaryTree.Keys.Max()].ParentId = CurrentElement.Id;        
            foreach (TreeViewModal tvm in buffer.Values)
            {
                if (tvm != buffer.First().Value)
                {
                    DictionaryTree.Add(DictionaryTree.Keys.Max() + 1, tvm.Clone);
                    DictionaryTree[DictionaryTree.Keys.Max()].ParentId = DictionaryTree.Last(x => x.Value.Id == tvm.ParentId).Key;                  
                }
            }
            foreach (int i in DictionaryTree.Keys)
                if (DictionaryTree[i].Id != i) DictionaryTree[i].Id = i;
        }
        public void ClearHistory()
        {
            history.Clear();
        }
        public void ClearBuffer()
        {
            buffer.Clear();
        }
        public void DeleteSubTree(TreeViewModal tvm)
        {
            foreach (TreeViewModal tmp in tvm.Children) DeleteSubTree(tmp);
            DictionaryTree.Remove(tvm.Id);
        }
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void UpdateTitle()
        {
            OnPropertyChanged("Title");
        }
    }
}

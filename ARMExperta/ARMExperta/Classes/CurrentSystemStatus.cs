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
        ObservableCollection<TreeViewModal> tree = new ObservableCollection<TreeViewModal>();
        ObservableCollection<TreeViewModal> old_tree = new ObservableCollection<TreeViewModal>();
        bool is_buffer = false,
             is_save = true,
             is_open = false,
             is_expert = false;
        int current_pos;
        string current_file_path;
        List<TreeViewModal> history = new List<TreeViewModal>();
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
    
        public ObservableCollection<TreeViewModal> Tree
        {
            get
            {
                return tree;
            }
        }
        public ObservableCollection<TreeViewModal> OldTree
        {
            get
            {
                return old_tree;
            }
        }
        public bool IsOpen
        {
            get
            {
                return is_open;
            }
            set
            {
                is_open = value;
                OnPropertyChanged("Title");
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
        public bool IsBuffer
        {
            get
            {
                return is_buffer;
            }
            set
            {
                is_buffer = value;
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
        public string CurrentFilePath
        {
            get
            {
                return current_file_path;
            }
            set
            {
                current_file_path = value;
                OnPropertyChanged("Title");
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
                if (CurrentFilePath != null) title += CurrentFilePath;
                if (!IsSave) title += "*";
                return title;
            }
        }
        public int CurrentPosInHistory
        {
            get
            {
                return current_pos;
            }
        }
        public List<TreeViewModal> History
        {
            get
            {
                return history;
            }
        }

        public void AddInHistory()
        {
            history.Add(tree.First().Clone);
            current_pos = history.Count - 1;
        }
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public int GetNewId()
        {
            int result = 0;
            foreach (TreeViewModal tvm in Tree)
                if (tvm.Id > result) result = tvm.Id;
            return result+1;
        }
    }
}

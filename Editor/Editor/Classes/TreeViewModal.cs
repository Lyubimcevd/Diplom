using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Editor.Windows;
using System.ComponentModel;

namespace Editor.Classes
{
    public class TreeViewModal:INotifyPropertyChanged
    {
        ObservableCollection<TreeViewModal> children = new ObservableCollection<TreeViewModal>();
        string naim;
        TreeViewModal parent;
        bool is_buffer = false, 
             is_history_end = true,
             is_history_begin = true,
             is_save = true,
             is_select = false,
             is_open = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public TreeViewModal() { }
        public TreeViewModal(TreeViewModal pparent)
        {
            parent = pparent;
        } 
        public ObservableCollection<TreeViewModal> Children
        {
            get
            {
                return children;
            }
        }
        public string Naim
        {
            get
            {
                return naim;
            }
            set
            {
                naim = value;
            }
        }
        public TreeViewModal Clone
        {
            get
            {
                TreeViewModal clone = new TreeViewModal();
                clone.Naim = Naim;
                foreach (TreeViewModal list in Children)
                {
                    clone.Children.Add(list.Clone);
                    clone.Children.Last().Parent = this;
                }
                return clone;
            }
        }
        static public TreeViewModal NewItem(TreeViewModal pparent)
        {
            TreeViewModal tmp = new TreeViewModal(pparent);
            WindowForEditNaim WFEN = new WindowForEditNaim(tmp);
            WFEN.ShowDialog();
            return tmp;
        }
        public TreeViewModal Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
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
                foreach (TreeViewModal list in Children) list.IsBuffer = value;
                OnPropertyChanged("IsBuffer");
            }
        }
        public bool IsHistoryBegin
        {
            get
            {
                return is_history_begin;
            }
            set
            {
                is_history_begin = value;
                OnPropertyChanged("IsHistoryBegin");
            }
        }
        public bool IsHistoryEnd
        {
            get
            {
                return is_history_end;
            }
            set
            {
                is_history_end = value;
                OnPropertyChanged("IsHistoryEnd");
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
                OnPropertyChanged("IsSave");
            }
        }
        public bool IsSelect
        {
            get
            {
                return is_select;
            }
            set
            {
                is_select = value;
                OnPropertyChanged("IsSelect");
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
                OnPropertyChanged("IsOpen");
            }
        }
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

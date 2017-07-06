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
    [Serializable]
    public class TreeViewModal:INotifyPropertyChanged
    {
        ObservableCollection<TreeViewModal> children = new ObservableCollection<TreeViewModal>();
        string naim;
        TreeViewModal parent;
        bool is_expanded;

        public event PropertyChangedEventHandler PropertyChanged;

        public TreeViewModal() { }
        public TreeViewModal(TreeViewModal pparent)
        {
            parent = pparent;
        }
        public TreeViewModal(string pnaim, TreeViewModal pparent)
        {
            naim = pnaim;
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
                OnPropertyChanged("Naim");
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
        public bool IsExpanded
        {
            get
            {
                return is_expanded;
            }
            set
            {
                is_expanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Editor.Windows;
using System.ComponentModel;
using System.Windows;

namespace Editor.Classes
{
    public class TreeViewModal:INotifyPropertyChanged
    {
        ObservableCollection<TreeViewModal> children = new ObservableCollection<TreeViewModal>();
        string naim;
        TreeViewModal parent;
        bool is_renamed;

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
        public TreeViewModal(SaveClass sc,TreeViewModal ppar)
        {
            naim = sc.Naim;
            parent = ppar;
            foreach (SaveClass cur in sc.Children) children.Add(new TreeViewModal(cur,this));
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
                if (value != null)
                {
                    if (value.Length == 0) MessageBox.Show("Поле не заполнено", "АРМ Эксперта Редактор");
                    else
                    {
                        naim = value;
                        OnPropertyChanged("Naim");
                    }
                }
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
                    clone.Children.Last().Parent = clone;
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
        public bool IsRenamed
        {
            get
            {
                return is_renamed;
            }
            set
            {
                is_renamed = value;
            }
        }
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public SaveClass Save
        {
            get
            {
                return new SaveClass(this);
            }
        }
    }
}

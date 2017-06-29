using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Editor.Classes
{
    class TreeViewModal
    {
        ObservableCollection<TreeViewModal> children;
        string naim;
        public TreeViewModal(string pnaim)
        {
            children = new ObservableCollection<TreeViewModal>();
            naim = pnaim;
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
                TreeViewModal clone = new TreeViewModal(Naim);
                clone.Naim = Naim;
                foreach (TreeViewModal list in Children) clone.Children.Add(list.Clone);
                return clone;
            }
        }
    }
}

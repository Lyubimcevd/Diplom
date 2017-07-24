using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Assessor.Classes
{
    public class TreeViewExpertModal:INotifyPropertyChanged
    {
        ObservableCollection<TreeViewExpertModal> children = new ObservableCollection<TreeViewExpertModal>();
        string naim;
        TreeViewExpertModal parent;
        int expert_opinion,coeff_import;
        bool is_expert,is_doubleclick;

        public event PropertyChangedEventHandler PropertyChanged;

        public TreeViewExpertModal(SaveClassExpert sc,TreeViewExpertModal ppar)
        {
            naim = sc.Naim;
            expert_opinion = sc.ExpertOpinion;
            coeff_import = sc.CoeffImport;
            parent = ppar;
            foreach (SaveClassExpert cur in sc.Children) children.Add(new TreeViewExpertModal(cur,this));
        }
        public TreeViewExpertModal(SaveClass sc, TreeViewExpertModal ppar)
        {
            naim = sc.Naim;
            parent = ppar;
            foreach (SaveClass cur in sc.Children) children.Add(new TreeViewExpertModal(cur, this));
        }
        public ObservableCollection<TreeViewExpertModal> Children
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
        public int ExpertOpinion
        {
            get
            {
                return expert_opinion;
            }
            set
            {
                expert_opinion = value;
            }
        }
        public int CoeffImport
        {
            get
            {
                return coeff_import;
            }
            set
            {
                coeff_import = value;
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
            }
        }
        public int ValueForSlider
        {
            get
            {
                if (IsExpert) return expert_opinion;
                else return coeff_import;
            }
            set
            {
                if (IsExpert) expert_opinion = value;
                else coeff_import = value;
                OnPropertyChanged("ValueForSlider");
            }
        }
        public bool IsDoubleClick
        {
            get
            {
                return is_doubleclick;
            }
            set
            {
                is_doubleclick = value;
            }
        }
        public TreeViewExpertModal Parent
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
        public SaveClassExpert Save
        {
            get
            {
                return new SaveClassExpert(this);
            }
        }
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

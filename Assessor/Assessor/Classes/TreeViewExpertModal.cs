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
        int expert_opinion,coeff_import,max = 100;
        bool is_doubleclick,is_ready = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public TreeViewExpertModal(SaveClassExpert sc,TreeViewExpertModal ppar)
        {
            naim = sc.Naim;
            expert_opinion = sc.ExpertOpinion;
            coeff_import = sc.CoeffImport;
            parent = ppar;
            foreach (SaveClassExpert cur in sc.Children) children.Add(new TreeViewExpertModal(cur,this));
            if (Children.Count != 0) ChangeRightBorder();
            else Is_Ready = true;
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
        public int ValueForSlider
        {
            get
            {
                if (WorkMode.IsExpert) return expert_opinion;
                else return coeff_import;
            }
            set
            {
                if (value > max)
                {
                    MessageBox.Show("Максимально возможное значение: " + max, "АРМ Эксперта Оценка");
                    value = max;
                }
                if (WorkMode.IsExpert) expert_opinion = value;
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
        public int Max
        {
            get
            {
                return max;
            }
            set
            {
                max = value;
                OnPropertyChanged("Max");
            }
        }
        public bool Is_Ready
        {
            get
            {
                return is_ready;
            }
            set
            {
                is_ready = value;
                OnPropertyChanged("Color");
                OnPropertyChanged("NaimForScreen");
            }
        }
        public string Color
        {
            get
            {
                if (Is_Ready) return "green";
                else return "black";
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
        public void ChangeRightBorder()
        {
            int sum = 100;
            bool is_all_ready = true;
            foreach (TreeViewExpertModal modal in Children)
            {
                sum -= modal.ValueForSlider;
                if (!modal.Is_Ready) is_all_ready = false;
            }
            foreach (TreeViewExpertModal modal in Children) modal.Max = sum + modal.ValueForSlider;
            if ((sum == 0) && is_all_ready) Is_Ready = true;
            else Is_Ready = false;
        }
    }
}

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
        int expert_opinion = -1,
            admin_coeff = 0,
            max = 100;
        bool is_doubleclick,
             is_ready = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public TreeViewExpertModal(SaveClassExpert sc,TreeViewExpertModal ppar)
        {
            naim = sc.Naim;
            expert_opinion = sc.ExpertOpinion;
            admin_coeff = sc.AdminCoeff;
            parent = ppar;
            foreach (SaveClassExpert cur in sc.Children) children.Add(new TreeViewExpertModal(cur,this));
            if (WorkMode.IsExpert)
            {
                if (expert_opinion != -1) Is_Ready = true;
            }
            else
            {
                UpdateValues();
            }
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
        public int AdminCoeff
        {
            get
            {
                return admin_coeff;
            }
            set
            {
                admin_coeff = value;
            }
        }
        public int ValueForSlider
        {
            get
            {
                if (WorkMode.IsExpert)
                    if (ExpertOpinion == -1) return 0;
                    else return ExpertOpinion;
                else return AdminCoeff;
            }
            set
            {
                if (value > max)
                {
                    MessageBox.Show("Максимально возможное значение: " + max, "АРМ Эксперта Оценка");
                    value = max;
                }
                if (WorkMode.IsExpert) ExpertOpinion = value;
                else AdminCoeff = value;
                if (Children.Count == 0) Is_Ready = true;
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
                if (is_ready&&WorkMode.IsExpert)
                    if (ExpertOpinion==-1) MakeExpertOpinion();
                OnPropertyChanged("Color");
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
        public void UpdateValues()
        {
            if (!WorkMode.IsExpert)
            {
                int sum = 100;
                bool is_all_ready = true;
                if (Children.Count == 0) sum = 0;
                else
                {
                    foreach (TreeViewExpertModal modal in Children) sum -= modal.ValueForSlider;
                    foreach (TreeViewExpertModal modal in Children) modal.Max = sum + modal.ValueForSlider;
                    foreach (TreeViewExpertModal modal in Children)
                    {
                        if (!modal.Is_Ready) is_all_ready = false;
                        break;
                    }
                }
                if (sum == 0&&is_all_ready) Is_Ready = true;
                else Is_Ready = false;
            }
            else
            {
                bool is_all_ready = true;
                foreach (TreeViewExpertModal child in Children)
                    if (child.ExpertOpinion == -1)
                    {
                        is_all_ready = false;
                        break;
                    }
                if (is_all_ready) Is_Ready = true;                
            }
            if (Parent != null) Parent.UpdateValues();
        }
        void MakeExpertOpinion()
        {
            foreach(TreeViewExpertModal child in Children)
            {
                ExpertOpinion += child.ExpertOpinion * child.AdminCoeff/100;
            }
        }
    }
}

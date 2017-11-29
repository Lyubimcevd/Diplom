using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Linq;

namespace ARMExperta.Classes
{
    public class TreeViewModal:INotifyPropertyChanged
    {
        string naim;
        int expert_opinion,
            admin_coeff,
            max = 100,
            id,
            par_id;
        bool is_ready = false,
             is_expanded = true,
             is_buffer = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public TreeViewModal(string p_naim)
        {
            naim = p_naim;
            par_id = -1;
        }
        public TreeViewModal(TreeViewModal main)
        {
            naim = main.Naim;
            id = main.Id;
            par_id = main.Parent.Id;
        }
        public TreeViewModal() { }
                
        public ObservableCollection<TreeViewModal> Children
        {
            get
            {
                ObservableCollection<TreeViewModal> result = new ObservableCollection<TreeViewModal>();
                foreach (TreeViewModal tvm in CurrentSystemStatus.GetSS.Tree)
                    if (tvm.Parent?.Id == Id) result.Add(tvm);
                return result;
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
                if (CurrentSystemStatus.GetSS.IsExpert)
                    if (ExpertOpinion == -1) return 0;
                    else return ExpertOpinion;
                else
                    if (AdminCoeff == -1) return 0;
                    else return AdminCoeff;
            }
            set
            {
                if (value > max)
                {
                    MessageBox.Show("Максимально возможное значение: " + max, "АРМ Эксперта Оценка");
                    value = max;
                }
                if (CurrentSystemStatus.GetSS.IsExpert) ExpertOpinion = value;
                else AdminCoeff = value;
                OnPropertyChanged("ValueForSlider");
                OnPropertyChanged("Color");
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
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
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
                if (is_ready&&CurrentSystemStatus.GetSS.IsExpert)
                    if (ExpertOpinion==-1) MakeExpertOpinion();
                OnPropertyChanged("Color");
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
        public string Color
        {
            get
            {
                if (Is_Ready) return "green";
                else
                    if (ValueForSlider > 0) return "brown";
                    else return "black";
            }
        }
        public TreeViewModal Parent
        {
            get
            {
                return CurrentSystemStatus.GetSS.Tree.FirstOrDefault(x => x.Id == par_id);
            }
        }
        public int ParentId
        {
            set
            {
                par_id = value;
            }
        }

        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void UpdateValues()
        {
            if (!CurrentSystemStatus.GetSS.IsExpert)
            {
                int sum = 100;          
                foreach (TreeViewModal modal in Children) sum -= modal.ValueForSlider;
                foreach (TreeViewModal modal in Children) modal.Max = sum + modal.ValueForSlider;
                if (sum == 0)
                    foreach (TreeViewModal modal in Children) modal.Is_Ready = true;
                if (Parent?.Parent == null)
                    {
                        bool is_all_ready = true;
                        foreach (TreeViewModal modal in Children)
                            if (!modal.Is_Ready)
                            {
                                is_all_ready = false;
                                break;
                            }
                        if (is_all_ready) Is_Ready = true;
                    }

            }
            else
            {
                bool is_all_ready = true;
                foreach (TreeViewModal child in Children)
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
            foreach(TreeViewModal child in Children)
            {
                ExpertOpinion += child.ExpertOpinion * child.AdminCoeff/100;
            }
        }
        public void Update()
        {
            OnPropertyChanged("Children");
        }
    }
}

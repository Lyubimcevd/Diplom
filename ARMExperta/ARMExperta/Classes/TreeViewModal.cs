using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Data;
using System;

namespace ARMExperta.Classes
{
    public class TreeViewModal:INotifyPropertyChanged
    {
        string naim;
        int expert_opinion = -1,
            admin_coeff = -1,
            max = 100,
            id,
            par_id;
        bool is_ready = false,
             is_expanded = true;
             
        public event PropertyChangedEventHandler PropertyChanged;

        public TreeViewModal(DataRow dr)
        {
            if (CurrentSystemStatus.GetSS.CurrentUser.GOST == 0)
            {
                id = Convert.ToInt32(dr["id"]);
                if (dr["par_id"] != DBNull.Value) par_id = Convert.ToInt32(dr["par_id"]);
                naim = dr["naim"].ToString();
                if (dr["admin_coef"] != DBNull.Value) admin_coeff = Convert.ToInt32(dr["admin_coef"]);
                if (dr["expert_opin"] != DBNull.Value) expert_opinion = Convert.ToInt32(dr["expert_opin"]);
            }
            else
            {
                id = Convert.ToInt32(dr["id"]);
                if (dr["par_id"] != DBNull.Value) par_id = Convert.ToInt32(dr["par_id"]);
                naim = dr["naim"].ToString();
            }
        }
        public TreeViewModal(string p_naim)
        {
            naim = p_naim;
            par_id = -1;
        }
        public TreeViewModal() { }
                
        public ObservableCollection<TreeViewModal> Children
        {
            get
            {
                ObservableCollection<TreeViewModal> result = new ObservableCollection<TreeViewModal>();
                foreach (TreeViewModal tvm in CurrentSystemStatus.GetSS.DictionaryTree.Values)
                    if (tvm.par_id == Id) result.Add(tvm);
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
                Is_Ready = true;
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
                {
                    if (ExpertOpinion < 0) return 0;
                    else return ExpertOpinion;
                }
                else
                {
                    if (AdminCoeff < 0) return 0;
                    else return AdminCoeff;
                }
            }
            set
            {
                if (value > Max)
                {
                    MessageBox.Show("Максимально возможное значение: " + Max, "АРМ Эксперта Оценка");
                    value = Max;
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
                if (CurrentSystemStatus.GetSS.IsExpert) return 100;
                else return max;
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
                OnPropertyChanged("Color");
            }
        }
        public string Color
        {
            get
            {
                if (Is_Ready) return "green";
                else
                {
                    if (CurrentSystemStatus.GetSS.IsExpert)
                    {
                        if (ExpertOpinion < 0) return "black";
                        else return "brown";
                    }
                    else
                    {
                        if (AdminCoeff < 0) return "black";
                        else return "brown";
                    }
                }
            }
        }
        public TreeViewModal Parent
        {
            get
            {
                if (par_id != -1) return CurrentSystemStatus.GetSS.DictionaryTree[par_id];
                else return null;
            }
        }
        public int ParentId
        {
            get
            {
                return par_id;
            }
            set
            {
                par_id = value;
            }
        }
        public TreeViewModal Clone
        {
            get
            {
                TreeViewModal result = new TreeViewModal();
                result.Naim = Naim;
                result.ExpertOpinion = ExpertOpinion;
                result.AdminCoeff = AdminCoeff;
                result.Id = Id;
                result.ParentId = ParentId;
                return result;
            }
        }

        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void UpdateReady()
        {
            if (CurrentSystemStatus.GetSS.IsExpert)
            {
                int tmp = 0;
                foreach (TreeViewModal child in Children)
                    if (child.Is_Ready) tmp += child.ExpertOpinion * child.AdminCoeff / 100;
                    else
                    {
                        Is_Ready = false;
                        return;
                    }
                if (Children.Count != 0) ExpertOpinion = tmp;
                if (ExpertOpinion > -1) Is_Ready = true;
                else Is_Ready = false;
                Parent?.UpdateReady();
            }
            else
            {
                if (Parent == null) Is_Ready = false;
                int sum = 100;
                foreach (TreeViewModal modal in Children) sum -= modal.ValueForSlider;
                foreach (TreeViewModal modal in Children) modal.Max = sum + modal.ValueForSlider;
                if (sum == 0)
                    foreach (TreeViewModal modal in Children) modal.Is_Ready = true;
                else
                    foreach (TreeViewModal modal in Children) modal.Is_Ready = false;
            }
        }
    }
}

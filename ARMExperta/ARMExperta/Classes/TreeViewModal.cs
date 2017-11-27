using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace ARMExperta.Classes
{
    public class TreeViewModal:INotifyPropertyChanged
    {
        ObservableCollection<TreeViewModal> children = new ObservableCollection<TreeViewModal>();
        string naim;
        TreeViewModal parent;
        int expert_opinion,admin_coeff,max = 100;
        int id;
        bool is_doubleclick,is_ready = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public TreeViewModal(ServerModal sm)
        {
            id = sm.Id;
            naim = sm.Naim;
            expert_opinion = sm.ExpertOpinion;
            admin_coeff = sm.AdminCoef;
            if (!Dictionaries.GetDictionaries.CurrentTreeOnClient.ContainsKey(sm.ParentId))
                Dictionaries.GetDictionaries.CurrentTreeOnClient.Add(sm.ParentId, 
                    new TreeViewModal(Dictionaries.GetDictionaries.CurrentTableOnServer[sm.ParentId]));
            parent = Dictionaries.GetDictionaries.CurrentTreeOnClient[sm.ParentId];
            foreach (ServerModal child in Dictionaries.GetDictionaries.CurrentTableOnServer.Values)
            {
                if (child.ParentId == sm.Id)
                {
                    if (!Dictionaries.GetDictionaries.CurrentTreeOnClient.ContainsKey(child.Id))
                        Dictionaries.GetDictionaries.CurrentTreeOnClient.Add(child.Id, 
                            new TreeViewModal(Dictionaries.GetDictionaries.CurrentTableOnServer[child.Id]));
                    children.Add(Dictionaries.GetDictionaries.CurrentTreeOnClient[child.Id]);
                }
            }
        }
        public TreeViewModal(string p_naim)
        {
            naim = p_naim;
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
                if (WorkMode.IsExpert) ExpertOpinion = value;
                else AdminCoeff = value;
                OnPropertyChanged("ValueForSlider");
                OnPropertyChanged("Color");
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
        public int GroupId
        {
            get
            {
                return group_id;
            }
            set
            {
                group_id = value;
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
                else
                    if (ValueForSlider > 0) return "brown";
                    else return "black";
            }
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
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void UpdateValues()
        {
            if (!WorkMode.IsExpert)
            {
                int sum = 100;          
                foreach (TreeViewModal modal in Children) sum -= modal.ValueForSlider;
                foreach (TreeViewModal modal in Children) modal.Max = sum + modal.ValueForSlider;
                if (sum == 0)
                    foreach (TreeViewModal modal in Children) modal.Is_Ready = true;
                if (parent != null)
                    if (parent.parent == null)
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
    }
}

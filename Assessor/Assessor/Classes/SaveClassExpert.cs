using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Assessor.Classes
{
    [DataContract]
    public class SaveClassExpert
    {
        [DataMember]
        ObservableCollection<SaveClassExpert> children = new ObservableCollection<SaveClassExpert>();
        [DataMember]
        string naim;
        [DataMember]
        int expert_opinion, addmin_coeff;

        public SaveClassExpert(TreeViewExpertModal psave)
        {
            naim = psave.Naim;
            expert_opinion = psave.ExpertOpinion;
            addmin_coeff = psave.AdminCoeff;
            foreach (TreeViewExpertModal mod in psave.Children) children.Add(mod.Save);
        }
        public string Naim
        {
            get
            {
                return naim;
            }
        }    
        public ObservableCollection<SaveClassExpert> Children
        {
            get
            {
                return children;
            }
        }
        public int ExpertOpinion
        {
            get
            {
                return expert_opinion;
            }
        }
        public int AdminCoeff
        {
            get
            {
                return addmin_coeff;
            }
        }
    }
}

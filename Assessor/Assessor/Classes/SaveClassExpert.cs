using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Assessor.Classes
{
    [Serializable]
    public class SaveClassExpert
    {
        ObservableCollection<SaveClassExpert> children = new ObservableCollection<SaveClassExpert>();
        string naim;
        int expert_opinion, coeff_import;

        public SaveClassExpert(TreeViewExpertModal psave)
        {
            naim = psave.Naim;
            expert_opinion = psave.ExpertOpinion;
            coeff_import = psave.CoeffImport;
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
        public int CoeffImport
        {
            get
            {
                return coeff_import;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace ARMExperta.Classes
{
    [DataContract]
    public class SaveClass
    {
        [DataMember]
        ObservableCollection<SaveClass> children = new ObservableCollection<SaveClass>();
        [DataMember]
        string naim;

        public SaveClass(TreeViewModal psave)
        {
            naim = psave.Naim;
            foreach (TreeViewModal mod in psave.Children) children.Add(mod.Save);
        }
        public string Naim
        {
            get
            {
                return naim;
            }
        }    
        public ObservableCollection<SaveClass> Children
        {
            get
            {
                return children;
            }
        }
    }
}

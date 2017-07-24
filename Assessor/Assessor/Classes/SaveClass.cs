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
    public class SaveClass
    {
        [DataMember]
        ObservableCollection<SaveClass> children = new ObservableCollection<SaveClass>();
        [DataMember]
        string naim;

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

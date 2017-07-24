using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Assessor.Classes
{
    class TitleViewModal:INotifyPropertyChanged
    {
        string title;

        public event PropertyChangedEventHandler PropertyChanged;

        public TitleViewModal(string ptitle)
        {
            title = ptitle;
        }
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

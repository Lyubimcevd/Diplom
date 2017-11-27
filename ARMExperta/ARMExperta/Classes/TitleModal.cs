using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMExperta.Classes
{
    class TitleModal : INotifyPropertyChanged
    {
        static TitleModal title_modal;

        TitleModal() { }

        public static TitleModal GetTitle
        {
            get
            {
                if (title_modal == null) title_modal = new TitleModal();
                return title_modal;
            }
        }
        public string Title
        {
            get
            {
                string title = "АРМ Эксперта Группа:" + CurrentSystemStatus.GetSS.Group + " ; ";
                if (CurrentSystemStatus.GetSS.IsExpert) title += "Режим: Эксперт";
                else title += "Режим: Администратор";
                if (CurrentSystemStatus.GetSS.CurrentFilePath != null) title += CurrentSystemStatus.GetSS.CurrentFilePath;
                if (!CurrentSystemStatus.GetSS.IsSave) title += "*";
                return title;
            }
        }
        public void Update()
        {
            OnPropertyChanged("Title");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

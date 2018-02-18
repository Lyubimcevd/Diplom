using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMExperta.Classes
{
    public class Message
    {
        string time,
            sms,
            sms_dir;
        public Message(string p_time,string p_sms,int id_admin,int id_group,bool dir)
        {
            time = p_time;
            sms = p_sms;
            if (dir) sms_dir = Server.GetServer.GetAdminFIO(id_admin) + " -> " + Server.GetServer.GetWorkGroups().First(x => x.Id == id_group).Naim;
            else sms_dir = Server.GetServer.GetWorkGroups().First(x => x.Id == id_group).Naim + " -> " + Server.GetServer.GetAdminFIO(id_admin);
        }

        public string Time
        {
            get
            {
                return time;
            }
        }
        public string Sms
        {
            get
            {
                return sms;
            }
        }
        public string SmsPath
        {
            get
            {
                return sms_dir;
            }
        }
    }
}

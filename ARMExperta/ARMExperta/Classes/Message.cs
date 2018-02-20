using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMExperta.Classes
{
    public class Message
    {
        string sms,
            sms_dir;
        DateTime time;
        public Message(DateTime p_time,string p_sms,int frwho,bool fradm)
        {
            time = p_time;
            sms = p_sms;
            if (fradm) sms_dir = "От: " + Server.GetServer.GetAdminFIO(frwho);
            else sms_dir = "От: "+Server.GetServer.GetWorkGroups().First(x => x.Id == frwho).Naim;
        }

        public string TimeShow
        {
            get
            {
                return time.ToShortTimeString();
            }
        }
        public DateTime Time
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

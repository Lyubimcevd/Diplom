using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMExperta.Classes
{
    public class User
    {
        int id,
            id_gost;
        bool is_group;
        public User(int p_id,bool p_is_group)
        {
            id = p_id;
            is_group = p_is_group;
        }
        public string Naim
        {
            get
            {

                if (is_group)
                {
                    Dictionary<int, string> tmp = Server.GetServer.GetStudentsFromWorkGroup(Id);
                    string result = "";
                    foreach (string stud in tmp.Values) result += stud + " ";
                    return result;
                }
                else return Server.GetServer.GetAdminFIO(id);
            }
        }
        public int Id
        {
            get
            {
                return id;
            }
        }
        public bool IsGroup
        {
            get
            {
                return is_group;
            }
        }
        public string Password
        {
            get
            {
               return Server.GetServer.GetPassword(id, is_group);
            }
        }
        public int GOST
        {
            get
            {
                return id_gost;
            }
            set
            {
                id_gost = value;
            }
        }
        public int IdEucationGroup
        {
            get
            {
                if (is_group) return Server.GetServer.GetEducationGroupOfStudent(Server.GetServer.GetStudentsFromWorkGroup(id).First().Key);
                else return -1;
            }
        }
        public string Mark
        {
            get
            {
                if (is_group) return Server.GetServer.GetMarkOfWorkGroup(Id);
                else return "";
            }
        }
    }
}

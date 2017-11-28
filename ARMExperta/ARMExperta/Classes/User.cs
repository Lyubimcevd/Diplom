using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMExperta.Classes
{
    class User
    {
        string naim;
        int id;
        bool is_group;
        string password;

        public User(string p_naim,int p_id,string p_password,bool p_is_group)
        {
            naim = p_naim;
            id = p_id;
            is_group = p_is_group;
            password = p_password;
        }
        public string Naim
        {
            get
            {
                return naim;
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
                return password;
            }
        }

    }
}

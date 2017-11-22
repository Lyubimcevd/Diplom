using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessor.Classes
{
    class ServerModal
    {
        int id, p_id, group_id, admin_coef, expert_opin;
        string naim;

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public int ParentId
        {
            get
            {
                return p_id;
            }
            set
            {
                p_id = value;
            }
        }
        public int GroupId
        {
            get
            {
                return group_id;
            }
            set
            {
                group_id = value;
            }
        }
        public int AdminCoef
        {
            get
            {
                return admin_coef;
            }
            set
            {
                admin_coef = value;
            }
        }
        public int ExpertOpinion
        {
            get
            {
                return expert_opin;
            }
            set
            {
                expert_opin = value;
            }
        }
        public string Naim
        {
            get
            {
                return naim;
            }
            set
            {
                naim = value;
            }
        }
    }
}

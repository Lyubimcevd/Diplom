using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ARMExperta.Classes
{
    class Converters
    {
        static Converters converters;

        Converters() { }

        public static Converters GetConverters
        {
            get
            {
                if (converters == null) converters = new Converters();
                return converters;
            }
        }

        public void ConvertDataTableToTree(DataTable DT)
        {
            foreach (DataRow DR in DT.Rows)
            {
                TreeViewModal tmp = new TreeViewModal();
                tmp.Id = Convert.ToInt32(DR["id"]);
                if (DR["par_id"] != DBNull.Value) tmp.ParentId = Convert.ToInt32(DR["par_id"]);
                tmp.Naim = DR["naim"].ToString();
                if (DR["admin_coef"] != DBNull.Value) tmp.AdminCoeff = Convert.ToInt32(DR["admin_coef"]);
                if (DR["expert_opin"] != DBNull.Value) tmp.ExpertOpinion = Convert.ToInt32(DR["expert_opin"]);
                CurrentSystemStatus.GetSS.OldTree.Add(tmp);
            }
        }
    }
}

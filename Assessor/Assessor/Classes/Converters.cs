using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Assessor.Classes
{
    class Converters
    {
        static Converters converters;

        Converters() { }

        public static Converters GetConverters()
        {
            if (converters == null) converters = new Converters();
            return converters;
        }

        public List<ServerModal> ConvertDataTableToServerModal(DataTable DT)
        {
            List<ServerModal> Rows = new List<ServerModal>();
            foreach (DataRow DR in DT.Rows) Rows.Add(FillRowFromTable(DR, new ServerModal()));
            return Rows;
        }
        static ServerModal FillRowFromTable(DataRow DR,ServerModal row)
        {
            row.Id = Convert.ToInt32(DR["id"]);
            if (DR["par"] != DBNull.Value) row.ParentId = Convert.ToInt32(DR["p_id"]);
            row.GroupId = Convert.ToInt32(DR["group_id"]);
            row.Naim = DR["naim"].ToString();
            if (DR["admin_coef"] != DBNull.Value) row.AdminCoef = Convert.ToInt32(DR["admin_coef"]);
            if (DR["expert_opin"] != DBNull.Value) row.ExpertOpinion = Convert.ToInt32(DR["naim"]);
            return row;
        }
    }
}

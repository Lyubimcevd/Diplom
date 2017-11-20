using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Threading;
using System.Windows;

namespace Editor.Classes
{
    class Server
    {
        static Server server;
        SqlConnection conn;
        DataTable DT;
        SqlDataAdapter DA;
        SqlCommandBuilder CB;

        Server()
        {
            conn = new SqlConnection("user id=ldo;password=IfLyyz4sCJ;server=nitel-hp;database=uit;MultipleActiveResultSets=True");
            conn.Open();
        }
        public static Server GetServer
        {
            get
            {
                if (server == null) server = new Server();
                return server;
            }
        }
        public List<object> ExecuteCommand(string Command)
        {
            SqlCommand SC = new SqlCommand(Command, conn);
            SqlDataReader SDR = SC.ExecuteReader();           
            if (SDR.HasRows)
            {
                List<object> ResultDate = new List<object>();
                while (SDR.Read())
                {
                    for (int i = 0; i < SDR.GetSchemaTable().Rows.Count; i++)
                        if (!SDR.IsDBNull(i)) ResultDate.Add(SDR.GetValue(i));
                        else ResultDate.Add(null);
                }
                return ResultDate;
            }
            else return new List<object>();
        }

        public List<ServerModal> GetModalByGroupId(int group_id)
        {
            DA = new SqlDataAdapter("select from modals where group_id = "+group_id.ToString(), conn);
            CB = new SqlCommandBuilder(DA);
            DT = new DataTable();
            DA.Fill(DT);
            return Converters.GetConverters().ConvertDataTableToServerModal(DT);
        }
    }
}

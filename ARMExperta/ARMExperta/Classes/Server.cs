using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Threading;
using System.Windows;

namespace ARMExperta.Classes
{
    class Server
    {
        static Server server;
        SqlConnection conn;
        DataTable DT;
        SqlDataAdapter DA;
        SqlCommandBuilder CB;
        List<object> tmp;

        Server()
        {
            conn = new SqlConnection("user id=ldo;password=IfLyyz4sCJ;server=nitel-hp;database=test1;MultipleActiveResultSets=True");
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

        List<object> ExecuteCommand(string Command)
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
            DA = new SqlDataAdapter("select * from modals where group_id = "+group_id.ToString(), conn);
            CB = new SqlCommandBuilder(DA);
            DT = new DataTable();
            DA.Fill(DT);
            return Converters.GetConverters().ConvertDataTableToServerModal(DT);
        }
        public Dictionary<string, string> GetUsersAndPassword()
        {
            tmp = ExecuteCommand("select fio,pwd from prep");
            string[] mas;
            Dictionary<string, string> result = new Dictionary<string, string>();
            for (int i = 0; i < tmp.Count; i += 2) result.Add(tmp[i] as string, tmp[i + 1] as string);
            tmp = ExecuteCommand("select id_uch,pwd from work_group");
            string login;
            for (int i = 0; i < tmp.Count; i += 2)
            {
                login = "Группа №"+(i/2+1).ToString()+": ";
                mas = (tmp[i] as string).Split(',');
                foreach (string id in mas)
                    login += ExecuteCommand("select fio from group_id_fio where id = " + id)[0] as string + ",";
                login = login.Remove(login.Length - 1);
                result.Add(login, tmp[i + 1] as string);
            }
            return result;
        }
        public List<string> GetGroups()
        {
            List<string> result = new List<string>();
            tmp = ExecuteCommand("select distinct ed_group from group_id_fio");
            foreach (string group in tmp) result.Add(group);
            return result;
        }
        public List<string> GetStudentsFromGroup(string group)
        {
            List<string> result = new List<string>();
            tmp = ExecuteCommand("select fio from group_id_fio where ed_group = '" + group +"'");
            foreach (string student in tmp) result.Add(student);
            return result;
        }
        public string GetStudentId(string fio)
        {
            return ExecuteCommand("select id from group_id_fio where fio = " + fio)[0].ToString();
        }
        public string GetGroupByGroupId(int group_id)
        {
            tmp = ExecuteCommand("select id_uch from work_group where id = "+group_id.ToString());
            string result = "";
            string[] mas = (tmp[0] as string).Split(',');
            foreach (string id in mas)
                result += ExecuteCommand("select fio from group_id_fio where id = " + id)[0] as string + ",";
            return result.Remove(result.Length - 1);
        }

        public void SetStudentsGroup(string ids,string pwd)
        {
            ExecuteCommand("INSERT INTO work_group (id_uch,is_ready,pwd) VALUES ('" + ids + "'," + 0 + ",'" + pwd + "')");
        }
        public void SaveOnServer()
        {
            foreach (int key in Dictionaries.GetDictionaries.CurrentTreeOnClient.Keys)
                if (Dictionaries.GetDictionaries.CurrentTableOnServer.ContainsKey(key))
                    ExecuteCommand("update models set p_id = " + Dictionaries.GetDictionaries.CurrentTreeOnClient[key].Parent?.Id.ToString()
                        + ",naim = " + Dictionaries.GetDictionaries.CurrentTreeOnClient[key].Naim
                        + ",admin_coef = " + Dictionaries.GetDictionaries.CurrentTreeOnClient[key].AdminCoeff
                        + ",expert_opin = " + Dictionaries.GetDictionaries.CurrentTreeOnClient[key].ExpertOpinion
                        + "where id = " + key.ToString());
                else
                    ExecuteCommand("INSERT INTO models(id,p_id,group_id,naim,admin_coef,expert_opin) VALUES (" + key.ToString() + ","
                        + Dictionaries.GetDictionaries.CurrentTreeOnClient[key].Parent?.Id.ToString() + ","
                        + Dictionaries.GetDictionaries.CurrentTreeOnClient[key].GroupId.ToString() + ",'"
                        + Dictionaries.GetDictionaries.CurrentTreeOnClient[key].Naim + "',"
                        + Dictionaries.GetDictionaries.CurrentTreeOnClient[key].AdminCoeff.ToString() + ","
                        + Dictionaries.GetDictionaries.CurrentTreeOnClient[key].ExpertOpinion.ToString());
        }
    }
}

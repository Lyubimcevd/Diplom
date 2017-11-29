using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
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

        public void GetModalByUser(User user)
        {
            DA = new SqlDataAdapter("select * from modals where group_id = "+user.Id.ToString()+" and is_group = "+user.IsGroup.ToString(), conn);
            CB = new SqlCommandBuilder(DA);
            DT = new DataTable();
            DA.Fill(DT);
            CurrentSystemStatus.GetSS.OldTree.Clear();
            Converters.GetConverters.ConvertDataTableToTree(DT);
            CurrentSystemStatus.GetSS.Tree = new ObservableCollection<TreeViewModal>(CurrentSystemStatus.GetSS.OldTree);
        }
        public List<User> GetUsersAndPassword()
        {
            tmp = ExecuteCommand("select fio,id,pwd from prep");
            string[] mas;
            List<User> result = new List<User>();
            for (int i = 0; i < tmp.Count; i += 3) result.Add(new User(tmp[i].ToString(), Convert.ToInt32(tmp[i + 1]),tmp[i+2].ToString(),false));
            tmp = ExecuteCommand("select id_uch,id,pwd from work_group");
            string login;
            for (int i = 0; i < tmp.Count; i += 3)
            {
                login = "";
                mas = (tmp[i] as string).Split(',');
                foreach (string id in mas)
                    login += ExecuteCommand("select fio from group_id_fio where id = " + id)[0].ToString() + ",";
                login = login.Remove(login.Length - 1);
                result.Add(new User(login, Convert.ToInt32(tmp[i + 1]),tmp[i+2].ToString(),true));
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
            string result = "";          
            tmp = ExecuteCommand("select id_uch from work_group where id = " + group_id.ToString());              
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
            foreach (TreeViewModal tvm in CurrentSystemStatus.GetSS.Tree)
                if (CurrentSystemStatus.GetSS.OldTree.FirstOrDefault(x=>x.Id == tvm.Id)!=null)
                    ExecuteCommand("update models set p_id = " + tvm.Parent.Id+ ",naim = " + tvm.Naim + ",admin_coef = " + tvm.AdminCoeff
                        + ",expert_opin = " + tvm.ExpertOpinion + "where id = " + tvm.Id);
                else
                    ExecuteCommand("INSERT INTO models(id,p_id,group_id,naim,admin_coef,expert_opin,is_group) VALUES (" + tvm.Id + ","
                        + tvm.Parent.Id + "," + CurrentSystemStatus.GetSS.CurrentUser.Id + ",'" + tvm.Naim + "'," + tvm.AdminCoeff + ","
                        + tvm.ExpertOpinion + "," + CurrentSystemStatus.GetSS.CurrentUser.IsGroup);
        }
    }
}

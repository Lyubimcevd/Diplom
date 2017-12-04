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
            conn = new SqlConnection("Data Source=NITEL-HP;Initial Catalog=test1;uid=ldo;pwd=IfLyyz4sCJ;MultipleActiveResultSets=True");
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
            DA = new SqlDataAdapter("select * from models where group_id = "+user.Id.ToString()+" and is_group = '"+user.IsGroup.ToString()+"'", conn);
            CB = new SqlCommandBuilder(DA);
            DT = new DataTable();
            DA.Fill(DT);
            CurrentSystemStatus.GetSS.OldTree.Clear();
            Converters.GetConverters.ConvertDataTableToTree(DT);
        }
        public List<User> GetUsersAndPassword()
        {
            List<User> result = new List<User>();
            tmp = ExecuteCommand("select id,fio,pwd from prep");
            for (int i = 0; i < tmp.Count; i += 3) result.Add(new User(tmp[i + 1].ToString(), Convert.ToInt32(tmp[i]), tmp[i + 2].ToString(), false));
            tmp = ExecuteCommand("select id,pwd from work_group");
            List<object> tmp1 = new List<object>();
            string login;
            for (int i = 0; i < tmp.Count; i+=2)
            {
                login = "";
                tmp1 = ExecuteCommand("select fio from students where work_group = " + Convert.ToInt32(tmp[0]));
                foreach (string fio in tmp1) login += fio + "; ";
                login = login.Remove(login.Length - 2);
                result.Add(new User(login, Convert.ToInt32(tmp[0]), tmp[1].ToString(), true));
            }         
            return result;
        }
        public List<string> GetGroups()
        {
            List<string> result = new List<string>();
            tmp = ExecuteCommand("select naim from ed_group");
            foreach (string group in tmp) result.Add(group);
            return result;
        }
        public List<string> GetStudentsFromGroup(string group)
        {
            List<string> result = new List<string>();
            tmp = ExecuteCommand("select fio from students inner join ed_group on students.ed_group = ed_group.Id where ed_group.naim = '" + group + "'");
            foreach (string student in tmp) result.Add(student);
            return result;
        }

        public void SetStudentsGroup(List<string> students,string pwd)
        {
            tmp = ExecuteCommand("select max(id) from work_group");
            int id;
            if (tmp[0] == null) id = 1;
            else id = Convert.ToInt32(tmp[0]) + 1;
            ExecuteCommand("insert into work_group (id,pwd) values (" + id + ",'" + pwd + "')");
            foreach (string fio in students)
                ExecuteCommand("update students set work_group = " + id + " where fio = '" + fio + "'");
        }
        public void SaveOnServer()
        {
            foreach (TreeViewModal tvm in CurrentSystemStatus.GetSS.Tree)
                if (!tvm.IsBuffer&&tvm.Parent!=null)
                    if (CurrentSystemStatus.GetSS.OldTree.FirstOrDefault(x=>x.Id == tvm.Id)!=null)
                        ExecuteCommand("update models set par_id = " + tvm.Parent.Id+ ",naim = " + tvm.Naim + ",admin_coef = " + tvm.AdminCoeff
                            + ",expert_opin = " + tvm.ExpertOpinion + "where id = " + tvm.Id);
                    else
                        ExecuteCommand("INSERT INTO models(id,par_id,group_id,naim,admin_coef,expert_opin,is_group) VALUES (" + tvm.Id + ","
                            + tvm.Parent.Id + "," + CurrentSystemStatus.GetSS.CurrentUser.Id + ",'" + tvm.Naim + "'," + tvm.AdminCoeff + ","
                            + tvm.ExpertOpinion + ",'" + CurrentSystemStatus.GetSS.CurrentUser.IsGroup + "')");
        }
    }
}

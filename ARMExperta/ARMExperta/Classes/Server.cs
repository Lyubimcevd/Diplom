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
using ARMExperta.Windows;
using System.IO;

namespace ARMExperta.Classes
{
    class Server
    {
        static DBFFiles server;
        SqlConnection conn;
        DataTable DT;
        SqlDataAdapter DA;
        SqlCommandBuilder CB;
        string str_connect;
        System.Timers.Timer time;

        Server()
        {
            StreamReader SR = new StreamReader("connect_string.cfg");
            str_connect = SR.ReadLine();
            conn = new SqlConnection(str_connect);
            conn.Open();
            SqlDependency.Start(str_connect);
            time = new System.Timers.Timer(60000);
            time.Elapsed += UpdateConnection;
            time.AutoReset = true;
            time.Enabled = true;
            Cardinal();
        }
        public static DBFFiles GetServer
        {
            get
            {
                if (server == null) server = new DBFFiles();
                return server;
                //if (server == null) server = new Server();
                //return server;
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
        int GetNewID(string table_naim)
        {
            int result = 1;
            while (ExecuteCommand("select * from " + table_naim + " where id = " + result).Count != 0) result++;
            return result;
        }

        public void GetModalByUser(User user)
        {
            if (user.GOST == 0) DA = new SqlDataAdapter("select * from models where group_id = "+user.Id.ToString()+" and is_group = '"+user.IsGroup.ToString()+"'", conn);
            else DA = new SqlDataAdapter("select * from GOSTs_models where gost_id = "+user.GOST, conn);
            CB = new SqlCommandBuilder(DA);
            DT = new DataTable();
            DA.Fill(DT);
            foreach (DataRow dr in DT.Rows) CurrentSystemStatus.GetSS.DictionaryTree.Add(Convert.ToInt32(dr["id"]), new TreeViewModal(dr));
        }
        public List<User> GetWorkGroups()
        {
            List<User> result = new List<User>();
            List<object> id_work_group = ExecuteCommand("select id from work_group");
            for (int i = 0; i < id_work_group.Count; i++) result.Add(new User(Convert.ToInt32(id_work_group[i]), true));          
            return result;
        }
        public Dictionary<int,string> GetGroups()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            List<object> id_naim_ed_group = ExecuteCommand("select id,naim from ed_group");
            for (int i = 0; i < id_naim_ed_group.Count; i += 2) result.Add(Convert.ToInt32(id_naim_ed_group[i]), id_naim_ed_group[i + 1].ToString());
            return result;
        }
        public Dictionary<int,string> GetStudentsFromEducationGroup(int p_id)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            List<object> id_fio_students = ExecuteCommand("select id,fio from students where ed_group = " + p_id);
            for (int i = 0; i < id_fio_students.Count; i += 2) result.Add(Convert.ToInt32(id_fio_students[i]), id_fio_students[i + 1].ToString());
            return result;
        }
        public Dictionary<int,string> GetStudentsFromWorkGroup(int p_id)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            List<object> id_fio_students = ExecuteCommand("select id,fio from students where work_group = " + p_id);
            for (int i = 0; i < id_fio_students.Count; i += 2) result.Add(Convert.ToInt32(id_fio_students[i]), id_fio_students[i + 1].ToString());
            return result;
        }
        public List<User> GetAdmins()
        {
            List<User> result = new List<User>();
            List<object> id_admins = ExecuteCommand("select id from admins");
            for (int i = 0; i < id_admins.Count; i++) result.Add(new User(Convert.ToInt32(id_admins[i]),false));
            return result;
        }
        public string GetAdminFIO(int p_id)
        {
            return ExecuteCommand("select fio from admins where id = " + p_id)[0].ToString();
        }
        public Dictionary<int,string> GetGOSTs()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            List<object> id_naim_GOSTs_naim = ExecuteCommand("select id,naim from GOSTs_naim");
            for (int i = 0; i < id_naim_GOSTs_naim.Count; i += 2) result.Add(Convert.ToInt32(id_naim_GOSTs_naim[i]), id_naim_GOSTs_naim[i + 1].ToString());
            return result;
        }
        public string GetPassword(int p_id, bool is_group)
        {
            if (is_group) return ExecuteCommand("select pwd from work_group where id = " + p_id)[0].ToString();
            else return ExecuteCommand("select pwd from admins where id = " + p_id)[0].ToString();
        }
        public int GetEducationGroupOfStudent(int p_id)
        {
            return Convert.ToInt32(ExecuteCommand("select ed_group from students where id = " + p_id)[0]);
        }
        public string GetMarkOfWorkGroup(int p_id)
        {
            List<object> mark_work_group = ExecuteCommand("select mark from work_group where id = " + p_id);
            if (mark_work_group[0] == null) return null;
            else return mark_work_group[0].ToString();
        }
        public List<Message> GetLastMessages(int frwho,int towho,bool fradm)
        {
            List<Message> result = new List<Message>();
            List<object> all_messages_id = ExecuteCommand("select dt,sms,frwho from chat where towho = "+towho+" and frwho = "+frwho+" and fradm = '"+fradm+"'");
            for (int i = 0; i < all_messages_id.Count; i += 3) result.Add(new Message(Convert.ToDateTime(all_messages_id[i]),
                  all_messages_id[i+1].ToString(), Convert.ToInt32(all_messages_id[i+2]), fradm));
            return result;
        }
        public bool GetReadyOfWorkGroup(int p_id)
        {
            return Convert.ToBoolean(ExecuteCommand("select is_ready from work_group where id = " + p_id)[0]);
        }

        public int AddNewWorkGroup(string pwd)
        {
            List<object> id_work_group = ExecuteCommand("select max(id) from work_group");
            int id;
            if (id_work_group[0] == null) id = 1;
            else id = Convert.ToInt32(id_work_group[0]) + 1;
            ExecuteCommand("insert into work_group (id,pwd) values (" + id + ",'" + pwd + "')");
            return id;
        }
        public void DeleteWorkGroup(int p_id)
        {
            ExecuteCommand("delete from models where is_group = 1 and group_id = " + p_id);
            ExecuteCommand("delete from work_group where id = " + p_id);
        }
        public void UpdateWorkGroup(Dictionary<int,string> stud,int id_work_group)
        {
            ExecuteCommand("update students set work_group = null where work_group = " + id_work_group);
            foreach (int id in stud.Keys) ExecuteCommand("update students set work_group = " + id_work_group + " where id = " + id);
        }
        public void UpdatePasswordForWorkGroup(string pwd,int id_work_group)
        {
            ExecuteCommand("update work_group set pwd = " + pwd + " where id = " + id_work_group);
        }
        public void UpdateMarkForWorkGroup(string mark, int id_work_group)
        {
            ExecuteCommand("update work_group set mark = " + mark + " where id = " + id_work_group);
        }
   
        public void SaveOnServer(User user)
        {
            if (user.GOST == 0) ExecuteCommand("delete from models where group_id = " + user.Id + " and is_group = '"+ user.IsGroup + "'");
            else ExecuteCommand("delete from GOSTs_models where gost_id = "+user.GOST);
            SaveOnServerTreeViewModal(CurrentSystemStatus.GetSS.Tree[0],0,user);
        }
        void SaveOnServerTreeViewModal(TreeViewModal ptvm,int ppar_id,User user)
        {
            int par_id = 0;
            if (ptvm.Parent != null)
            {
                if (user.GOST == 0)
                {
                    ExecuteCommand("INSERT INTO models(par_id,group_id,naim,admin_coef,expert_opin,is_group) VALUES (" + ppar_id + "," + user.Id + ",'" + ptvm.Naim 
                        + "'," + ptvm.AdminCoeff + "," + ptvm.ExpertOpinion + ",'" + user.IsGroup + "')");
                    par_id = Convert.ToInt32(ExecuteCommand("select top 1 id from models order by id desc")[0]);
                }
                else
                {
                    ExecuteCommand("INSERT INTO GOSTs_models (par_id,naim,gost_id) VALUES(" + ppar_id + ",'" + ptvm.Naim + "',"+ user.GOST+ ")");
                    par_id = Convert.ToInt32(ExecuteCommand("select top 1 id from GOSTs_models order by id desc")[0]);
                }
            }
            foreach (TreeViewModal tvm in ptvm.Children) SaveOnServerTreeViewModal(tvm,par_id,user);
        }

        public void SetNewGroup(string group)
        {
            ExecuteCommand("INSERT INTO ed_group ([id],[naim]) VALUES (" + GetNewID("ed_group") + ",'" + group + "')");
        }
        public void DeleteGroup(int p_id)
        {
            List<object> work_group_students = ExecuteCommand("select distinct work_group from students where work_group is not null and ed_group = " + p_id);
            for (int i = 0; i < work_group_students.Count; i++) DeleteWorkGroup(Convert.ToInt32(work_group_students[i]));
            ExecuteCommand("delete from students where ed_group = " + p_id);
            ExecuteCommand("delete from ed_group where id = " + p_id);
        }
        public void UpdateGroup(DataTable dt, int p_id_ed_group)
        {
            ExecuteCommand("delete from students where ed_group = " + p_id_ed_group);
            foreach (DataRow stud in dt.Rows)
                ExecuteCommand("INSERT INTO students ([id],[fio],[ed_group]) VALUES (" + GetNewID("students") + ",'" + stud[0] + "'," + p_id_ed_group + ")");
        }

        public void AddNewAdmin(string fio,string pwd)
        {
            ExecuteCommand("INSERT INTO admins ([id],[fio],[pwd]) VALUES (" + GetNewID("admins") + ",'" + fio + "','" + pwd + "')");
        }
        public void UpdateAdmin(string fio, string pwd,int id)
        {
            ExecuteCommand("Update admins set fio = '" + fio + "',pwd = '" + pwd + "' where id = " + id);
        }
        public void DeleteAdmin(int id)
        {
            ExecuteCommand("delete from models where is_group = 0 and group_id = " + id);
            ExecuteCommand("Delete from admins where id = " + id);
        }

        public int AddNewGOST(string name)
        {
            ExecuteCommand("INSERT INTO GOSTs_naim (id,[naim]) VALUES (" + GetNewID("GOSTs_naim") + ",'" + name + "')");
            return Convert.ToInt32(ExecuteCommand("select top 1 id from GOSTs_naim order by id desc")[0]);
        }
        public void CopyGOSTToAllWorkGroups(int p_id)
        {
            List<User> users = GetWorkGroups();
            CurrentSystemStatus.GetSS.CurrentUser.GOST = p_id;
            foreach (User us in users)
            {
                us.GOST = p_id;
                GetModalByUser(us);
                CurrentSystemStatus.GetSS.DictionaryTree.Add(0, new TreeViewModal("Показатели качества"));
                us.GOST = 0;
                SaveOnServer(us);
                CurrentSystemStatus.GetSS.DictionaryTree.Clear();
            }
            CurrentSystemStatus.GetSS.CurrentUser.GOST = 0;
        }

        public void SetReadyOfWorkGroup(bool is_ready,int p_id)
        {
            ExecuteCommand("update work_group set is_ready = '" + is_ready + "' where id = " + p_id);
        }

        public void SendMessage(string text,int frwho,int towho,bool fradm)
        {
            ExecuteCommand("INSERT INTO chat(sms,frwho,towho,fradm) VALUES ('"+text+"',"+frwho+","+towho+ ",'"+fradm+ "')");
        }

        //Система слежения. Ядро Cardinal

        SqlDependency sqlDependencyStalker;
        Dispatcher Dis = Dispatcher.CurrentDispatcher;
        Message mes;
        int towho,
            frwho;

        void Cardinal()
        {
            if (sqlDependencyStalker != null)
            {
                sqlDependencyStalker.OnChange -= OnDatabaseChange_dt;
                sqlDependencyStalker = null;
            }
            using (var command = new SqlCommand("select dt from dbo.chat", conn))
            {
                sqlDependencyStalker = new SqlDependency(command);
                sqlDependencyStalker.OnChange += new OnChangeEventHandler(OnDatabaseChange_dt);
                command.ExecuteReader();
            }
        }

        void OnDatabaseChange_dt(object sender, SqlNotificationEventArgs e)
        {
            List<object> sms_chat = ExecuteCommand("select top 1 * from chat order by dt desc");
            frwho = Convert.ToInt32(sms_chat[2]);
            towho = Convert.ToInt32(sms_chat[3]);
            mes = new Message(Convert.ToDateTime(sms_chat[0]), sms_chat[1].ToString(), frwho, Convert.ToBoolean(sms_chat[4]));
            if (CurrentSystemStatus.GetSS.CurrentUser.Id == towho) Dis.Invoke(new Action(OpenChat));
            Cardinal();
        }

        void OpenChat()
        {
            if (CurrentSystemStatus.GetSS.OpenChats.ContainsKey(frwho))
            {
                CurrentSystemStatus.GetSS.OpenChats[frwho].Update();
                CurrentSystemStatus.GetSS.OpenChats[frwho].Activate();
            }
            else
            {
                Chat Ch = new Chat(frwho);
                Ch.ShowDialog();
            }
        }

        void UpdateConnection(Object source, System.Timers.ElapsedEventArgs e)
        {
            Cardinal();
        }
    }
}

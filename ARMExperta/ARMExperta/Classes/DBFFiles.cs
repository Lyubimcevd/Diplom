using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

namespace ARMExperta.Classes
{
    class DBFFiles
    {
        OleDbConnection conn;

        public DBFFiles()
        {
            conn = new OleDbConnection();
            StreamReader SR = new StreamReader("connect_string.cfg");
            conn = new OleDbConnection(SR.ReadLine());
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        DataTable GetTable(string Command)
        {
            DataTable dt = null;
            if (conn != null)
            {
                try
                {
                    dt = new DataTable();
                    OleDbCommand oCmd = conn.CreateCommand();
                    oCmd.CommandText = Command;                 
                    dt.Load(oCmd.ExecuteReader());
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            return dt;
        }
        List<object> ExecuteCommand(string Command)
        {
            DataTable dt = GetTable(Command);
            List<object> result = new List<object>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                    for (int i = 0; i < dt.Columns.Count; i++) result.Add(dr[i]);
            }
            return result;
        }
        void ExecuteCommandWithoutResult(string Command)
        {
            if (conn != null)
            {
                try
                {
                    OleDbCommand oCmd = conn.CreateCommand();
                    oCmd.CommandText = Command;
                    oCmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        int GetNewID(string table_naim)
        {
            List<object> tmp = ExecuteCommand("select top 1 id from " + table_naim + " order by id desc");
            if (tmp.Count == 0) return 1;
            else return Convert.ToInt32(tmp[0]) + 1;
        }

        public void GetModalByUser(User user)
        {
            DataTable dt;
            if (user.GOST == 0) dt = GetTable("select * from models where group_id = " + user.Id.ToString() + " and is_group = " + user.IsGroup.ToString());
            else dt = GetTable("select * from GOSTs_models where gost_id = " + user.GOST);
            foreach (DataRow dr in dt.Rows) CurrentSystemStatus.GetSS.DictionaryTree.Add(Convert.ToInt32(dr["id"]), new TreeViewModal(dr));
        }
        public List<User> GetWorkGroups()
        {
            List<User> result = new List<User>();
            List<object> id_work_group = ExecuteCommand("select id from work_group");
            for (int i = 0; i < id_work_group.Count; i++) result.Add(new User(Convert.ToInt32(id_work_group[i]), true));
            return result;
        }
        public Dictionary<int, string> GetGroups()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            List<object> id_naim_ed_group = ExecuteCommand("select id,naim from ed_group");
            for (int i = 0; i < id_naim_ed_group.Count; i += 2) result.Add(Convert.ToInt32(id_naim_ed_group[i]), id_naim_ed_group[i + 1].ToString());
            return result;
        }
        public Dictionary<int, string> GetStudentsFromEducationGroup(int p_id)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            List<object> id_fio_students = ExecuteCommand("select id,fio from students where ed_group = " + p_id);
            for (int i = 0; i < id_fio_students.Count; i += 2) result.Add(Convert.ToInt32(id_fio_students[i]), id_fio_students[i + 1].ToString());
            return result;
        }
        public Dictionary<int, string> GetStudentsFromWorkGroup(int p_id)
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
            for (int i = 0; i < id_admins.Count; i++) result.Add(new User(Convert.ToInt32(id_admins[i]), false));
            return result;
        }
        public string GetAdminFIO(int p_id)
        {
            return ExecuteCommand("select fio from admins where id = " + p_id)[0].ToString();
        }
        public Dictionary<int, string> GetGOSTs()
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
        public List<Message> GetLastMessages(int frwho, int towho, bool fradm)
        {
            List<Message> result = new List<Message>();
            List<object> all_messages_id = ExecuteCommand("select dt,sms,frwho from chat where towho = " + towho + " and frwho = " + frwho + " and fradm = '" + fradm + "'");
            for (int i = 0; i < all_messages_id.Count; i += 3) result.Add(new Message(Convert.ToDateTime(all_messages_id[i]),
                  all_messages_id[i + 1].ToString(), Convert.ToInt32(all_messages_id[i + 2]), fradm));
            return result;
        }
        public bool GetReadyOfWorkGroup(int p_id)
        {
            return Convert.ToBoolean(ExecuteCommand("select is_ready from work_group where id = " + p_id)[0]);
        }

        public int AddNewWorkGroup(string pwd)
        {
            int id = GetNewID("work_group");
            ExecuteCommandWithoutResult("insert into work_group (id,pwd) values (" + id + ",'" + pwd + "')");
            return id;
        }
        public void DeleteWorkGroup(int p_id)
        {
            ExecuteCommandWithoutResult("delete from models where is_group = 1 and group_id = " + p_id);
            ExecuteCommandWithoutResult("delete from work_group where id = " + p_id);
        }
        public void UpdateWorkGroup(Dictionary<int, string> stud, int id_work_group)
        {
            ExecuteCommandWithoutResult("update students set work_group = '' where work_group = " + id_work_group);
            foreach (int id in stud.Keys) ExecuteCommand("update students set work_group = " + id_work_group + " where id = " + id);
        }
        public void UpdatePasswordForWorkGroup(string pwd, int id_work_group)
        {
            ExecuteCommandWithoutResult("update work_group set pwd = '" + pwd + "' where id = " + id_work_group);
        }
        public void UpdateMarkForWorkGroup(string mark, int id_work_group)
        {
            ExecuteCommandWithoutResult("update work_group set mark = " + mark + " where id = " + id_work_group);
        }

        public void SaveOnServer(User user)
        {
            if (user.GOST == 0) ExecuteCommandWithoutResult("delete from models where group_id = " + user.Id + " and is_group = " + user.IsGroup);
            else ExecuteCommandWithoutResult("delete from GOSTs_models where gost_id = " + user.GOST);
            SaveOnServerTreeViewModal(CurrentSystemStatus.GetSS.Tree[0], 0, user);
        }
        void SaveOnServerTreeViewModal(TreeViewModal ptvm, int ppar_id, User user)
        {
            int par_id = 0;
            if (ptvm.Parent != null)
            {
                if (user.GOST == 0)
                {
                    ExecuteCommandWithoutResult("INSERT INTO models(id,par_id,group_id,naim,admin_coef,expert_opin,is_group) VALUES ("+GetNewID("models")+","+ ppar_id + "," + user.Id + ",'" + ptvm.Naim
                        + "'," + ptvm.AdminCoeff + "," + ptvm.ExpertOpinion + "," + user.IsGroup + ")");
                    par_id = Convert.ToInt32(ExecuteCommand("select top 1 id from models order by id desc")[0]);
                }
                else
                {
                    ExecuteCommandWithoutResult("INSERT INTO GOSTs_models (id,par_id,naim,gost_id) VALUES(" + GetNewID("GOSTs_models") + "," + ppar_id + ",'" + ptvm.Naim + "'," + user.GOST + ")");
                    par_id = Convert.ToInt32(ExecuteCommand("select top 1 id from GOSTs_models order by id desc")[0]);
                }
            }
            foreach (TreeViewModal tvm in ptvm.Children) SaveOnServerTreeViewModal(tvm, par_id, user);
        }

        public void SetNewGroup(string group)
        {
            ExecuteCommandWithoutResult("INSERT INTO ed_group ([id],[naim]) VALUES (" + GetNewID("ed_group") + ",'" + group + "')");
        }
        public void DeleteGroup(int p_id)
        {
            List<object> work_group_students = ExecuteCommand("select distinct work_group from students where len(work_group) = 0 and ed_group = " + p_id);
            for (int i = 0; i < work_group_students.Count; i++) DeleteWorkGroup(Convert.ToInt32(work_group_students[i]));
            ExecuteCommandWithoutResult("delete from students where ed_group = " + p_id);
            ExecuteCommandWithoutResult("delete from ed_group where id = " + p_id);
        }
        public void UpdateGroup(DataTable dt, int p_id_ed_group)
        {
            DataTable dt_old = GetTable("select * from students where ed_group = " + p_id_ed_group);
            ExecuteCommandWithoutResult("delete from students where ed_group = " + p_id_ed_group);
            foreach (DataRow stud in dt.Rows)
            {
                if (dt_old.Select("work_group <> 0 and fio = '" + stud[0]+"'").Length != 0) ExecuteCommandWithoutResult("INSERT INTO students([id], [fio], [ed_group]," +
                    " work_group) VALUES(" + GetNewID("students") + ",'" + stud[0] + "'," + p_id_ed_group + ","+ dt_old.Select("fio = '" + stud[0]+"'")[0]["work_group"].ToString() + ")");
                else ExecuteCommandWithoutResult("INSERT INTO students ([id],[fio],[ed_group]) VALUES (" + GetNewID("students") + ",'" + stud[0] + "'," + p_id_ed_group + ")");

            }
        }

        public void AddNewAdmin(string fio, string pwd)
        {
            ExecuteCommandWithoutResult("INSERT INTO admins ([id],[fio],[pwd]) VALUES (" + GetNewID("admins") + ",'" + fio + "','" + pwd + "')");
        }
        public void UpdateAdmin(string fio, string pwd, int id)
        {
            ExecuteCommandWithoutResult("Update admins set fio = '" + fio + "',pwd = '" + pwd + "' where id = " + id);
        }
        public void DeleteAdmin(int id)
        {
            ExecuteCommandWithoutResult("delete from models where is_group = false and group_id = " + id);
            ExecuteCommandWithoutResult("Delete from admins where id = " + id);
        }

        public int AddNewGOST(string name)
        {
            ExecuteCommandWithoutResult("INSERT INTO GOSTs_naim (id,[naim]) VALUES (" + GetNewID("GOSTs_naim") + ",'" + name + "')");
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

        public void SetReadyOfWorkGroup(bool is_ready, int p_id)
        {
            ExecuteCommandWithoutResult("update work_group set is_ready = " + is_ready + " where id = " + p_id);
        }

        public void SendMessage(string text, int frwho, int towho, bool fradm)
        {
            ExecuteCommandWithoutResult("INSERT INTO chat(sms,frwho,towho,fradm) VALUES ('" + text + "'," + frwho + "," + towho + ",'" + fradm + "')");
        }

    }
}

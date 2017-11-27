using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMExperta.Classes
{
    class CurrentSystemStatus
    {
        Dictionary<int, ServerModal> current_table_on_server;
        Dictionary<int, TreeViewModal> current_tree_on_client;
        bool is_buffer = false,
             is_history_end = true,
             is_history_begin = true,
             is_save = true,
             is_select = false,
             is_open = false,
             is_expert = false;
        int group_id;
        string current_file_path;
        static CurrentSystemStatus current_sys_stat;

        CurrentSystemStatus() { }

        public static CurrentSystemStatus GetSS
        {
            get
            {
                if (current_sys_stat == null) current_sys_stat = new CurrentSystemStatus();
                return current_sys_stat;
            }
        }
        public Dictionary<int, ServerModal> CurrentTableOnServer
        {
            get
            {
                return current_table_on_server;
            }
            set
            {
                current_table_on_server = value;
            }
        }
        public Dictionary<int, TreeViewModal> CurrentTreeOnClient
        {
            get
            {
                return current_tree_on_client;
            }
            set
            {
                current_tree_on_client = value;
            }
        }
        public bool IsOpen
        {
            get
            {
                return is_open;
            }
            set
            {
                is_open = value;
                TitleModal.GetTitle.Update();
            }
        }
        public bool IsSave
        {
            get
            {
                return is_save;
            }
            set
            {
                is_save = value;
                TitleModal.GetTitle.Update();
            }
          
        }
        public bool IsExpert
        {
            get
            {
                return is_expert;
            }
            set
            {
                is_expert = value;
                TitleModal.GetTitle.Update();
            }
        }
        public string Group
        {
            get
            {
                return Server.GetServer.GetGroupByGroupId(group_id);
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
        public string CurrentFilePath
        {
            get
            {
                return current_file_path;
            }
            set
            {
                current_file_path = value;
                TitleModal.GetTitle.Update();
            }
        }
    }
}

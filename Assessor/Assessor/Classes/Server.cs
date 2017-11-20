using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Threading;
using Cards_of_defectation.Windows;
using Cards_of_defectation.ОУП.Windows;
using System.Windows;

namespace Cards_of_defectation.Classes
{
    class Server
    {
        static Server server;
        Dictionary<string, Connection> connections = new Dictionary<string, Connection>();

        private Server() { }
        public static Server GetServer
        {
            get
            {
                if (server == null)
                {
                    server = new Server();
                    SqlDependency.Start("user id=ldo;password=IfLyyz4sCJ;server=nitel-hp;database=uit;MultipleActiveResultSets=True");
                }
                return server;
            }          
        }
        public Connection DataBase(string DataBaseName)
        {
            if (!connections.ContainsKey(DataBaseName)) 
                connections.Add(DataBaseName, new Connection(DataBaseName));
            return connections[DataBaseName];
        }
        public void CloseConnections()
        {
            SqlDependency.Stop("user id=ldo;password=IfLyyz4sCJ;server=nitel-hp;database=uit;MultipleActiveResultSets=True");
            foreach (KeyValuePair<string, Cards_of_defectation.Classes.Connection> item in connections) item.Value.Close();
        }
    }

    class Connection
    {
        SqlConnection conn;
        Dispatcher dis;
        ShopAlert SA;
        Tree_defect TD;
        MainOUP MOUP;
        SqlDependency sqlDependencyForTree, sqlDependencyForShop, sqlDependencyForPlan;

        public Connection(string DataBaseName)
        {
            conn = new SqlConnection("user id=ldo;password=IfLyyz4sCJ;server=nitel-hp;database=" + DataBaseName + ";MultipleActiveResultSets=True");            
            Log.Init.Info("Открытие соединения");
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Log.Init.Fatal("Не подключился к серверу. Ошибка: " + e.Message);
                MessageBox.Show("Сервер не доступен");
                System.Environment.Exit(0);
            }
            Log.Init.Info("Открыто");
        }
        public List<object> ExecuteCommand(string Command)
        {
            SqlCommand SC = new SqlCommand(Command, conn);
            Log.Init.Info("ExecuteCommand чтение по запросу "+Command);
            SqlDataReader SDR = null;
            try
            {
                SDR = SC.ExecuteReader();
            }
            catch (Exception e)
            {
                Log.Init.Fatal("Ошибка чтения. Ошибка: " + e.Message);
                MessageBox.Show("Ошибка чтения по запросу " + Command);
                System.Environment.Exit(0);
            }
            Log.Init.Info("Чтение успешно");
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
        public DataTableBuffer Table(string query)
        {
            return new DataTableBuffer(query, conn);
        }
        public void InitStalker(Dispatcher pdis, ShopAlert pSA)
        {
            dis = pdis;
            SA = pSA;
            StalkerForShop();
        }
        public void InitStalker(Dispatcher pdis, Tree_defect pTD)
        {
            dis = pdis;
            TD = pTD;
            StalkerForTree();
        }
        public void InitStalker(Dispatcher pdis, MainOUP pMOUP)
        {
            dis = pdis;
            MOUP = pMOUP;
            StalkerForPlan();
        }
        void StalkerForTree()
        {
            if (sqlDependencyForTree != null)
            {
                sqlDependencyForTree.OnChange -= OnDatabaseChange_Id_and_nom_ceh;
                sqlDependencyForTree = null;
            }
            using (var command = new SqlCommand("select id,nom_ceh from dbo.rz_kart_defect", conn))
            {
                sqlDependencyForTree = new SqlDependency(command);
                sqlDependencyForTree.OnChange += new OnChangeEventHandler(OnDatabaseChange_Id_and_nom_ceh);
                command.ExecuteReader();
            }
        }
        void StalkerForShop()
        {
            if (sqlDependencyForShop != null)
            {
                sqlDependencyForShop.OnChange -= OnDatabaseChange_is_faster;
                sqlDependencyForShop = null;
            }           
            using (var command = new SqlCommand("select is_faster from dbo.rz_kart_defect", conn))
            {
                sqlDependencyForShop = new SqlDependency(command);
                sqlDependencyForShop.OnChange += new OnChangeEventHandler(OnDatabaseChange_is_faster);
                command.ExecuteReader();
            }
        }
        void StalkerForPlan()
        {
            if (sqlDependencyForPlan != null)
            {
                sqlDependencyForPlan.OnChange -= OnDatabaseChange_nom_sz;
                sqlDependencyForPlan = null;
            }
            using (var command = new SqlCommand("select nom_sz from dbo.rz_plan_rabot", conn))
            {
                sqlDependencyForPlan = new SqlDependency(command);
                sqlDependencyForPlan.OnChange += new OnChangeEventHandler(OnDatabaseChange_nom_sz);
                command.ExecuteReader();
            }
        }
        void OnDatabaseChange_Id_and_nom_ceh(object sender, SqlNotificationEventArgs e)
        {
            if (TD != null) dis.Invoke(new Action(TD.UpdateTree));
            StalkerForTree();
        }
        void OnDatabaseChange_is_faster(object sender, SqlNotificationEventArgs e)
        {
            if (SA != null) dis.Invoke(new Action(SA.UpdateRow));
            StalkerForShop();
        }
        void OnDatabaseChange_nom_sz(object sender, SqlNotificationEventArgs e)
        {
            if (MOUP != null) dis.Invoke(new Action(MOUP.UpdateTable));
            StalkerForPlan();
        }
        public void Close()
        {
            conn.Close();
        }
    }

    class DataTableBuffer
    {
        DataTable DT;
        SqlDataAdapter DA;
        SqlCommandBuilder CB;

        public DataTableBuffer(string SelectCommand,SqlConnection conn)
        {
            DA = new SqlDataAdapter(SelectCommand, conn);
            CB = new SqlCommandBuilder(DA);
            int start = SelectCommand.IndexOf("from") + 5;
            int end = SelectCommand.IndexOf("where")-1;
            if (end == -2) end = SelectCommand.Length;
            DT = new DataTable(SelectCommand.Substring(start, end - start));
            Log.Init.Info("Заполнение таблицы из DataAdapter. Команда: " + SelectCommand);
            try
            {
                DA.Fill(DT);
            }
            catch (Exception e)
            {
                Log.Init.Fatal("Ошибка в блоке DataTableBuffer. Ошибка: " + e.Message);
                MessageBox.Show("Ошибка заполнения таблицы");
                Environment.Exit(0);
            }
            Log.Init.Info("Заполнено");
        }
        public void UpdateServerData(List<Row_in_plan_rabot> Rows)
        {
            Converter.ListToTable(Rows, DT);
            LoadTableOnServer();
        }
        public void UpdateServerData(List<Row_in_kart_defect> Rows)
        {
            Converter.ListToTable(Rows, DT);
            LoadTableOnServer();
        }
        public void UpdateServerData(DataTable pDT)
        {
            DT = pDT;
            LoadTableOnServer();
        }
        void LoadTableOnServer()
        {
            Log.Init.Info("Update таблицы " + DT.TableName);
            try
            {
                DA.Update(DT);
            }
            catch (Exception e)
            {
                Log.Init.Fatal("Ошибка блока LoadTableOnServer. Ошибка: " + e.Message);
                MessageBox.Show("Не удалось сохранить данные на сервер. Ошибка: "+e.Message);
                Environment.Exit(0);
            }
            Log.Init.Info("Update успешно");
        }
        public object LoadFromServer()
        {
            if (DT.TableName == "rz_plan_rabot")
            {
                return Converter.TableToList(DT) as List<Row_in_plan_rabot>;
            }
            else
            {              
                return Converter.TableToList(DT) as List<Row_in_kart_defect>;              
            }        
        }
        public List<Row_in_kart_defect> LoadFromServerForShopAlert()
        {
            List<Row_in_kart_defect> result = Converter.TableToList(DT) as List<Row_in_kart_defect>;
            result.Reverse();
            for (int i = 0; i < result.Count; i++)
                if (result[i].IsFaster)
                {
                    result.Insert(0, result[i]);
                    result.RemoveAt(i + 1);
                }
            return result;
        }
        public DataTable LoadTableFromServer()
        {
            return DT;
        }
    }
}

using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Data.OleDb;
using System.IO;
using TooFuns.Framework.Data;



namespace UCADB
{
   
    public class ConnectionConfig
    {
        public string dbType { get; set; }
        public string connName { get; set; }
        public string connectionString { get; set; }

    }
  


    public sealed class ConnectionInstance
    {
        public Dictionary<string, object> UserWorkSession = new Dictionary<string, object>();

        private Dictionary<string, XmlNode> ConnectionStrings = new Dictionary<string, XmlNode>();

        private Dictionary<string, ConnectionConfig> _connectionNodes = null;

        
        public  Dictionary<string, ConnectionConfig> ConnectionNodes
        {
            get
            {
                if(_connectionNodes==null)
                {

                    initDb();
                }

                return _connectionNodes;

            }

        }



        public void initDb()
        {
            if (_connectionNodes == null)
            {
                _connectionNodes = new Dictionary<string, ConnectionConfig>();

                List<Database> kpDB = (List<Database>)ConfigurationManager.GetSection("csyweb/database");

                foreach (var db in kpDB)
                {
                    var cc = new ConnectionConfig()
                    {
                        connectionString = db.connectString,
                        connName = db.Name,
                        dbType = "SqlServer"
                    };

                    _connectionNodes[db.Name] = cc;

                }


                

            }
        }
           
    

        public void ResetDataAccess()
        {
            ConnectionStrings.Clear();

            string path = "";

            if (HttpContext.Current != null)
            {
                path = HttpContext.Current.Server.MapPath("~");
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }

            path = Path.Combine(path, "common\\configs");


            string connfn = Path.Combine(path, "connectionStrings.xml");

            XmlDocument xconn = new XmlDocument();
            try
            {

                xconn.Load(connfn);
            }
            catch
            {

                return;
            }

            XmlNodeList inslist = xconn.SelectNodes("//ConnectionString");

                

        

            string exi;

            foreach (XmlNode xn in inslist)
            {
                if (xn.Attributes.GetNamedItem("ConnectionName") != null)
                {
                    if (ConnectionStrings.ContainsKey(xn.Attributes["ConnectionName"].Value))
                    {
                        ConnectionStrings[xn.Attributes["ConnectionName"].Value] = xn;
                    }
                    else
                    {

                        ConnectionStrings[xn.Attributes["ConnectionName"].Value] = xn;
                    }
                }
               
            }

        }



        public void AddConnectionString(string connName, XmlNode connNode)
        {
            ConnectionStrings.Add(connName, connNode);
        }

        public DbConnection ReturnDBConnection(string connName)
        {
            DbConnection res;

           



            if (ConnectionStrings.ContainsKey(connName))
            {

                string DBType = "";

                if (ConnectionStrings[connName].Attributes.GetNamedItem("Type") != null)
                {
                    DBType = ConnectionStrings[connName].Attributes["Type"].Value;
                }

             

                string connStr = ConnectionStrings[connName].InnerText;

                switch (DBType)
                {
                    case "SqlServer":
                       
                            res = new SqlConnection(connStr);
                            res.Open();

                            return res;
                       
                    case "Access":
                       
                            res = new OleDbConnection(connStr);
                            res.Open();


                            return res;
                    default:
                       
                            res = new SqlConnection(connStr);
                            res.Open();


                            return res;
                }


            }
            throw new Exception("missing conn config:" + connName);
        }

     
        private ConnectionInstance()
        {
            //SessionCache = new SesCache(this);
        }

        public static ConnectionInstance Instance
        {
            get
            {

                return _Instance;

            }
        }

        private static readonly ConnectionInstance _Instance = new ConnectionInstance();


       
    }
}

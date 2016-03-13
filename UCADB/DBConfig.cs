using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;
using System.Data;
using System.Web;

namespace UCADB
{
    public class DBConfig
    {
        public string defaultConfigDirectory = "common\\configs\\";
        public string defaultUIDirectory = "common\\ui\\";
        public string defaultSessionUserID = "UserID";

        private Guid _appID = Guid.Empty;
        private string _appName = "DefaultApp";

        XmlDocument _dbStructure = new XmlDocument();
        public bool IsNeedLoadStructure = true;

        private DataTable _exTrace = null;


        private bool mIsSessionBased = false;

        public bool IsSessionBased
        {
            get { return mIsSessionBased; }
            set { mIsSessionBased = value; }
        }
	

        
        private static volatile DBConfig _self = null;

        private static object syncRoot = new Object();
        private DBConfig() { }
        public static DBConfig Instance
        {
            get
            {
                if (_self == null)
                {
                    lock (syncRoot)
                    {
                        if (_self == null)
                            _self = new DBConfig();
                    }
                }
                return _self;
            }
        }


        public void TraceLog(string logStr)
        {
            lock (syncRoot)
            {
                if (_exTrace == null)
                {
                    _exTrace = new DataTable();
                    _exTrace.Columns.Add("ID", typeof(Guid));
                    _exTrace.Columns.Add("AppID", typeof(Guid));
                    _exTrace.Columns.Add("AppName", typeof(string));
                    _exTrace.Columns.Add("AccessIP", typeof(string));
                    _exTrace.Columns.Add("UserID", typeof(string));
                    _exTrace.Columns.Add("ErrorText", typeof(string));
                    _exTrace.Columns.Add("ErrorTime", typeof(DateTime));

                }

             

                
                


                

               
                
                


            }



        }




        public bool SetDBStructureFileName(string fileName)
        {
            try
            {
                _dbStructure.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultUIDirectory + fileName));
            }
            catch (Exception ex)
            {
                TraceLog(ex.Message);
            }
            return true;

        }

        




    }
}

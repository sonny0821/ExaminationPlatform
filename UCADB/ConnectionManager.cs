using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Web;

namespace UCADB
{
    public class ConnectionManager
    {
        public string openex = "";

        public DateTime CreateTime = DateTime.Now;

        public DateTime lastOpTime = DateTime.Now;
        public string UserIP = "Local";
        public string UserSessionID = "Local";
        public string customUserID = Guid.Empty.ToString();



        private string _UserID = "";

        private void InitUserInfo()
        {
            if (HttpContext.Current != null)
            {
                UserIP = HttpContext.Current.Request.UserHostAddress;
                if (HttpContext.Current.Session != null)
                {
                    UserSessionID = HttpContext.Current.Session.SessionID;
                    string sname = DBConfig.Instance.defaultSessionUserID;

                    if (HttpContext.Current.Session[sname] != null)
                    {
                        customUserID = HttpContext.Current.Session[sname].ToString();
                    }

                }
            }


        }


        public ConnectionManager()
        {
            InitUserInfo();
        }

        public ConnectionManager(ref List<string> UserLog, string UserID)
        {
            InitUserInfo();

            LogUser(ref UserLog, true);


        }

        private void LogUser(ref List<string> UserLog, bool IsLog)
        {
            if (IsLog)
            {
                UserLog.Add("User:" + _UserID.ToString() + "; IP:" + UserIP + "; SID:" + UserSessionID + " ; Log in at " + DateTime.Now.ToString());
            }
            else
            {
                UserLog.Add("User:" + _UserID.ToString() + "; IP:" + UserIP + "; SID:" + UserSessionID + " ; Log off at " + DateTime.Now.ToString());
            }
        }


        public int GetConnectionCount()
        {
            return ConnectionsList.Count;

        }

        public ICollection<string> GetConnectionKeys()
        {
            return ConnectionsList.Keys;
        }

        public ICollection<DbConnection> GetConnectionList()
        {
            return ConnectionsList.Values;
        }




        Dictionary<string, DbConnection> ConnectionsList = new Dictionary<string, DbConnection>();

        Dictionary<string, DbTransaction> TransactionList = new Dictionary<string, DbTransaction>();

        Dictionary<string, bool> TransactionStateList = new Dictionary<string, bool>();



        protected string defaultCommonStr = "";

        private void SetTransactionState(string connName, bool alive)
        {
            if (!TransactionStateList.ContainsKey(connName))
            {
                TransactionStateList.Add(connName, alive);
            }
            else
            {
                TransactionStateList[connName] = alive;
            }
        }

        private bool GetTransactionState(string connName)
        {
            if (!TransactionStateList.ContainsKey(connName))
            {
                return false;
            }
            else
            {
                return TransactionStateList[connName];
            }
        }


        public bool BeginTran(string connName)
        {
            if (ConnectionsList.ContainsKey(connName))
            {
                if (GetTransactionState(connName))
                {
                    return false;
                }

                if (TransactionList.ContainsKey(connName) && TransactionList[connName] != null)
                {
                    return false;
                }
                else
                {
                    DbTransaction dTran = ConnectionsList[connName].BeginTransaction();
                    TransactionList.Add(connName, dTran);
                    SetTransactionState(connName, true);

                    return true;
                }
            }
            return false;
        }

        public bool RollbakcTran(string connName)
        {
            if (!GetTransactionState(connName))
            {
                return false;
            }
            if (TransactionList.ContainsKey(connName) && TransactionList[connName] != null)
            {


                try
                {
                    TransactionList[connName].Rollback();
                    SetTransactionState(connName, false);
                    TransactionList.Remove(connName);
                }
                catch (Exception e)
                {
                    throw e;
                }

                return true;
            }

            return false;
        }

        public bool CommitTran(string connName)
        {
            if (!GetTransactionState(connName))
            {
                return false;
            }
            if (TransactionList.ContainsKey(connName) && TransactionList[connName] != null)
            {
                try
                {
                    TransactionList[connName].Commit();
                    SetTransactionState(connName, false);
                    TransactionList.Remove(connName);
                }
                catch (Exception e)
                {


                    RollbakcTran(connName);
                    throw e;



                }

                return true;
            }

            return false;

        }


        public void SetdefaultCommonStr(string cStr)
        {
            defaultCommonStr = cStr;
        }

        public void LoadConnection(string connName, DbConnection conn)
        {


            ConnectionsList.Add(connName, conn);
            if (ConnectionsList.Count == 1)
            {
                defaultCommonStr = connName;
            }

        }

        public List<string[]> ReturnConnectionInfo()
        {
            List<string[]> res = new List<string[]>();
            foreach (string connid in ConnectionsList.Keys)
            {
                string[] cinfo = new string[3];
                cinfo[0] = connid;
                cinfo[1] = ConnectionsList[connid].State.ToString();
                cinfo[2] = ConnectionsList[connid].ConnectionString;
                res.Add(cinfo);
            }
            return res;
        }

        public DbConnection ReturnConnection(string connName)
        {
            lastOpTime = DateTime.Now;

            //Dictionary<Guid, ConnectionManager> UCI = ConnectionInstance.Instance.GetAllUserConnInfo();
            //foreach (ConnectionManager Cmgr in UCI.Values)
            //{
            //    if (lastOpTime.AddMinutes(-3) > Cmgr.lastOpTime)
            //    {
            //        Cmgr.PauseAll();
            //    }
            //}

            if (connName.Trim() == "")
            {
                connName = defaultCommonStr;
            }
            if (ConnectionsList.ContainsKey(connName))
            {

                if (ConnectionsList[connName].State != System.Data.ConnectionState.Open)
                {
                    try
                    {
                        ConnectionsList[connName].Open();
                    }
                    catch (Exception exi)
                    {
                        throw new Exception("Á¬½Ó×Ö·û´®´íÎó:" + exi);
                    }
                }
                return ConnectionsList[connName];
            }
            else
            {
                throw new Exception("Á¬½Ó×Ö·û´®´íÎó");
            }
        }

        public void CloseConnection(string connName)
        {
            if (connName.Trim() == "")
            {
                connName = defaultCommonStr;
            }
            if (ConnectionsList.ContainsKey(connName))
            {
                ConnectionsList[connName].Close();
                ConnectionsList.Remove(connName);
            }

        }

        public void PauseAll()
        {
            foreach (DbConnection conn in ConnectionsList.Values)
            {
                conn.Close();
            }

        }

        public void CloseAll()
        {
            foreach (DbConnection conn in ConnectionsList.Values)
            {
                conn.Close();
            }
            ConnectionsList.Clear();
        }

        public void CloseAll(ref List<string> UserLog)
        {
            LogUser(ref UserLog, false);
            foreach (DbConnection conn in ConnectionsList.Values)
            {
                conn.Close();
            }
            ConnectionsList.Clear();
        }
    }
}

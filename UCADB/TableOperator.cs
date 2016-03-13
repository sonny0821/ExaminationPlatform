using System;
using System.Collections.Generic;
using System.Text;
using UCADB;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using TooFuns.Framework;
using TooFuns.Framework.Data;
using TooFuns.Framework.Access;
using System.Collections;
namespace UCADB
{
    public enum dbOpType {ExecuteNonQuery, };

    
    /// <summary>
    /// ���ݿ������
    /// </summary>
    public class TableOperator
    {
        protected dbTypeEnum dbType = dbTypeEnum.SqlDB;
        protected string connStr = "";

        public string ex;
        public string ReturnValue;
        public int RowCount;
        public object Result;
        public DataSet Ds;

        public DataBaseActivator dba = new DataBaseActivator();
        /// <summary>
        /// ctor.
        /// </summary>
        public TableOperator()
        {
        }
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="currentDba">�����ݿ�����������</param>
        public TableOperator(DataBaseActivator currentDba)
        {
            dba = currentDba;
        }

        public TableOperator(dbTypeEnum type, string cStr)
        {
            dbType = type;
            connStr = cStr;
        }
        DatabaseOperator DO = null;
        public DbConnection DbaActive(string connStr)
        {
            if(ConnectionInstance.Instance.ConnectionNodes.ContainsKey(connStr))
            {
                return dba.active(ConnectionInstance.Instance.ConnectionNodes[connStr], out DO);

            }

            DO = null;
            return null;

        }

       
        /// <summary>
        /// Sql�޷��ز���
        /// </summary>
        /// <param name="isProc">�Ƿ�洢����</param>
        /// <param name="procname">sql����洢������</param>
        /// <param name="prms">��������˳��</param>
        /// <param name="connStr">������</param>
        /// <returns>�����Ƿ�ִ�гɹ�</returns>
        public bool ExecuteNonQuery(bool isProc, string procname, object[] prms, string connStr)
        {
            return ExecuteNonQueryEx(isProc, procname, prms, null, connStr);
        }
        /// <summary>
        /// Sql�޷��ز���
        /// </summary>
        /// <param name="isProc">�Ƿ�洢����</param>
        /// <param name="procname">sql����洢������</param>
        /// <param name="connStr">������</param>
        /// <param name="keyPrms">��������key��</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(bool isProc, string procname, string connStr, Dictionary<string, object> keyPrms)
        {
            return ExecuteNonQueryEx(isProc, procname, null, keyPrms, connStr);
        }
        /// <summary>
        /// Sql�޷��ز���
        /// </summary>
        /// <param name="isProc">�Ƿ�洢����</param>
        /// <param name="procname">sql����洢������</param>
        /// <param name="connStr">������</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(bool isProc, string procname, string connStr)
        {
            return ExecuteNonQueryEx(isProc, procname, null, null, connStr);
        }

        public bool ExecuteNonQueryEx(bool isProc, string procname, object[] prms, Dictionary<string, object> keyPrms, string connStr)
        {
            if (prms == null)
            {
                prms = new object[0];


            }
            if (keyPrms == null)
            {
                keyPrms = new Dictionary<string, object>();


            }

            try
            {
                using (DbConnection conn = DbaActive(connStr))
                {
                    if (conn == null || DO == null)
                    {
                        ex = "������������" + connStr + "����";
                        return false;
                    }

                    if (isProc)
                    {
                        if (prms.Length != 0)
                        { DO.LoadCommandWrapper(procname, prms); }
                        else
                        { DO.LoadCommandWrapper(procname, keyPrms); }


                    }
                    else
                    {
                        if (prms.Length != 0)
                        { DO.LoadTextWrapper(procname, prms); }
                        else
                        { DO.LoadTextWrapper(procname, keyPrms); }


                    }

                    return DO.ExecuteNonQuery(out ReturnValue, out RowCount, out ex);


                }
            }
            catch(Exception exi)
            {
                ex = exi.Message;
                return false;
            }


        }



   


     



        public bool ScalarQuery(bool isProc, string procname, object[] prms, string connStr)
        {
            return ScalarQueryDoOperator(isProc, procname, prms,null, out ex, connStr, out Result);
        }

        public bool ScalarQuery(bool isProc, string procname, string connStr, Dictionary<string, object> keyPrms)
        {
            return ScalarQueryDoOperator(isProc, procname, null, keyPrms, out ex, connStr, out Result);
        }

        public bool ScalarQuery(bool isProc, string procname, string connStr)
        {
            return ScalarQueryDoOperator(isProc, procname, null, null, out ex, connStr, out Result);
        }


        public bool ScalarQueryDoOperator(bool isProc, string procname, object[] prms, Dictionary<string, object> keyPrms, out string Ex, string connStr, out object result)
        {
            if (prms == null)
            {
                prms = new object[0];


            }
            if (keyPrms == null)
            {
                keyPrms = new Dictionary<string, object>();


            }

            try
            {
                using (DbConnection conn = DbaActive(connStr))
                {


                    if (isProc)
                    {
                        if (prms.Length != 0)
                        { DO.LoadCommandWrapper(procname, prms); }
                        else
                        { DO.LoadCommandWrapper(procname, keyPrms); }


                    }
                    else
                    {
                        if (prms.Length != 0)
                        { DO.LoadTextWrapper(procname, prms); }
                        else
                        { DO.LoadTextWrapper(procname, keyPrms); }


                    }
                    return DO.ExecuteScalar(out result, out Ex);
                }
            }
            catch (Exception exi)
            {
                result = null;
                Ex = exi.Message;
                return false;
            }

        }






        /// <summary>
        /// �������ݼ�
        /// </summary>
        /// <param name="isProc">�Ƿ�洢���̣�����Ϊsql���</param>
        /// <param name="Ex">����ֵ������ɹ�Ϊok</param>
        /// <param name="procname">�洢������/sql���</param>
        /// <param name="prms">��������Ϊnull</param>
        /// <param name="keyPrms">����KV����Ϊnull</param>
        /// <param name="ds">��Ҫ���ص����ݼ�</param>
        /// <param name="connStr">������</param>
        /// <returns></returns>
        public bool ReturnDS(bool isProc, out string Ex, string procname, object[] prms, Dictionary<string, object> keyPrms, out DataSet ds, string connStr)
        {
            if (prms == null)
            {
                prms = new object[0];


            }
            if (keyPrms == null)
            {
                keyPrms = new Dictionary<string, object>();


            }

            try
            {
                using (DbConnection conn = DbaActive(connStr))
                {
                    if (conn == null || DO == null)
                    {
                        Ex = "������������" + connStr + "����";
                        ds = new DataSet();
                        return false;
                    }

                    if (isProc)
                    {
                        if (prms.Length != 0)
                        { DO.LoadCommandWrapper(procname, prms); }
                        else
                        { DO.LoadCommandWrapper(procname, keyPrms); }


                    }
                    else
                    {
                        if (prms.Length != 0)
                        { DO.LoadTextWrapper(procname, prms); }
                        else
                        { DO.LoadTextWrapper(procname, keyPrms); }


                    }

                    return DO.ExecuteDataSet(out ds, out Ex);

                }
            }
            catch (Exception exi)
            {
                Ex = exi.Message;
                ds = new DataSet();
                return false;
            }
        }

        

       

        /// <summary>
        /// ����DataSet
        /// </summary>
        /// <param name="isProc">�Ƿ�洢����</param>
        /// <param name="procname">�洢��������sql���</param>
        /// <param name="prms">������</param>
        /// <param name="connStr">��������</param>
        /// <returns></returns>
        public bool ReturnDS(bool isProc, string procname, object[] prms, string connStr)
        {
            return ReturnDS(isProc, out ex, procname, prms,null, out Ds, connStr);
        }

        /// <summary>
        /// ����DataSet
        /// </summary>
        /// <param name="isProc">�Ƿ�洢����</param>
        /// <param name="procname">�洢��������sql���</param>
        /// <param name="connStr">��������</param>
        /// <param name="prms">����KV����Ϊnull</param>
        /// <returns></returns>
        public bool ReturnDS(bool isProc, string procname, string connStr, Dictionary<string,object> prms)
        {
            return ReturnDS(isProc, out ex, procname, null, prms, out Ds, connStr);
        }

        public bool ReturnDS(bool isProc, string procname, string connStr)
        {
            return ReturnDS(isProc, out ex, procname, null, null, out Ds, connStr);
        }


        /// <summary>
        /// ����һ��ʵ��
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="entity">ʵ�����</param>
        /// <param name="tableName">����</param>
        /// <param name="connName">������</param>
        /// <param name="insertIgnoreColumns">��������ֶ�</param>
        /// <returns></returns>
        public bool InsertER<T>(T entity, string tableName,string connName,string[] insertIgnoreColumns)
        {

            if(insertIgnoreColumns ==null)
            {
                insertIgnoreColumns = new string[0];
            }

            List<string> igCases = new List<string>(insertIgnoreColumns);

          



            string PK_ID = "";
            string kf = "";


            System.Reflection.PropertyInfo[] array = entity.GetType().GetProperties();

            List<string> updateVals = new List<string>();
            List<string> insertFlds = new List<string>();
            List<string> insertVals = new List<string>();

            List<object> prms = new List<object>();

            Dictionary<string,object> keyPrms=new Dictionary<string,object> ();

            foreach (System.Reflection.PropertyInfo p in array)
            {
                

               
                object propVal = p.GetValue(entity, null);

                if (p is IList || p is IEnumerable)
                {
                    continue;
                }

                string pVal = "[@PRM" + keyPrms.Count.ToString() + "]";

                keyPrms["PRM" + keyPrms.Count.ToString()] = propVal;


                if (!igCases.Contains(p.Name))
                {
                    insertFlds.Add(p.Name);





                    insertVals.Add(pVal);
                }
               

                if (p.Name == "PK_ID" || p.Name.ToUpper() == "ID")
                {
                    PK_ID = propVal.ToString();

                    kf = p.Name;
                }
                else
                {

                    updateVals.Add(p.Name + "=" + pVal);

                }



            }
         

          

               
            



            string sql = "Update {0} Set {1} where {3}='{2}' ";

            sql = string.Format(sql, tableName, string.Join(",", updateVals.ToArray()), PK_ID, kf);

            if (!ExecuteNonQuery(false, sql, connName, keyPrms))
            {
                return false;
            }

            if (RowCount > 0)
            {
                return true;
            }
            else
            {

                sql = "Insert into {0} ({1}) values({2})";



                sql = string.Format(sql, tableName, string.Join(",", insertFlds.ToArray()), string.Join(",", insertVals.ToArray()));

                if (ExecuteNonQuery(false, sql, connName, keyPrms))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }



        public bool InsertER<T>(T entity, string tableName, string connName, string[] insertIgnoreColumns, string[] updateIgnoreColumns)
        {

            if (insertIgnoreColumns == null)
            {
                insertIgnoreColumns = new string[0];
            }

            if (updateIgnoreColumns == null)
            {
                updateIgnoreColumns = new string[0];
            }

            List<string> igCases = new List<string>(insertIgnoreColumns);
            List<string> ugCases = new List<string>(updateIgnoreColumns);




            string PK_ID = "";
            string kf = "";


            System.Reflection.PropertyInfo[] array = entity.GetType().GetProperties();

            List<string> updateVals = new List<string>();
            List<string> insertFlds = new List<string>();
            List<string> insertVals = new List<string>();

            List<object> prms = new List<object>();

            Dictionary<string, object> keyPrms = new Dictionary<string, object>();

            foreach (System.Reflection.PropertyInfo p in array)
            {



                object propVal = p.GetValue(entity, null);

                if (p is IList || p is IEnumerable)
                {
                    continue;
                }

                string pVal = "[@PRM" + keyPrms.Count.ToString() + "]";

                keyPrms["PRM" + keyPrms.Count.ToString()] = propVal;


                if (!igCases.Contains(p.Name))
                {
                    insertFlds.Add(p.Name);





                    insertVals.Add(pVal);
                }


                if (p.Name == "PK_ID" || p.Name.ToUpper() == "ID")
                {
                    PK_ID = propVal.ToString();

                    kf = p.Name;
                }
                else
                {
                    if (!ugCases.Contains(p.Name))
                    {
                        updateVals.Add(p.Name + "=" + pVal);
                    }

                }



            }









            string sql = "Update {0} Set {1} where {3}='{2}' ";

            sql = string.Format(sql, tableName, string.Join(",", updateVals.ToArray()), PK_ID, kf);

            if (!ExecuteNonQuery(false, sql, connName, keyPrms))
            {
                return false;
            }

            if (RowCount > 0)
            {
                return true;
            }
            else
            {

                sql = "Insert into {0} ({1}) values({2})";



                sql = string.Format(sql, tableName, string.Join(",", insertFlds.ToArray()), string.Join(",", insertVals.ToArray()));

                if (ExecuteNonQuery(false, sql, connName, keyPrms))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }
        public T FormERElement<T>(T mETDZ, DataRow dr)
        {
            System.Reflection.PropertyInfo[] array = mETDZ.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo p in array)
            {
                if (dr.Table.Columns.Contains(p.Name))
                {
                    p.SetValue(mETDZ, dr[p.Name], null);

                }



            }

            return mETDZ;


        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="PK_ID">����</param>
        /// <param name="idField">�����ֶ���</param>
        /// <param name="tableName">����</param>
        /// <param name="connName">������</param>
        /// <returns></returns>
        public T GetER<T>(string PK_ID,string idField, string tableName, string connName) where T : new()
        {

          
            string sql = "Select * from {0} where {2}='{1}' ";

            sql = string.Format(sql, tableName, PK_ID, idField);

            if (ReturnDS(false, sql, connName))
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    T tmpT = new T();

                    tmpT = FormERElement<T>(tmpT, dr);

                    return tmpT;
                }


            }

            return default(T);





        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="SQL">SQL�����ص�һ�е�ʵ��</param>
        /// <param name="connName">������</param>
        /// <returns></returns>
        public T GetER<T>(string SQL, string connName) where T : new()
        {

            T entity = new T();

            string sql = SQL;

         

            if (ReturnDS(false, sql, connName))
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    T tmpT = new T();

                    tmpT = FormERElement<T>(tmpT, dr);

                    return tmpT;
                }


            }

            return default(T);





        }

        /// <summary>
        /// ��ȡʵ���б�
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="SQL">SQL�����������е�ʵ���б�</param>
        /// <param name="connName">������</param>
        /// <returns></returns>
        public List<T> GetAllER<T>(string SQL, string connName) where T : new()
        {

           
            string sql = SQL;

            List<T> res = new List<T>();
            if (ReturnDS(false, sql, connName))
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    T tmpT = new T();

                    tmpT = FormERElement<T>(tmpT, dr);

                    res.Add(tmpT);
                }


            }

            return res;





        }

        /// <summary>
        /// ��ȡʵ���б�
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="isProc">�Ƿ�洢����</param>
        /// <param name="SQL">SQL���</param>
        /// <param name="prms">������ӦΪdictionary[string,object]��object[]</param>
        /// <param name="connName">��������</param>
        /// <returns></returns>
        public List<T> GetAllER<T>(bool isProc, string SQL,object prms, string connName) where T : new()
        {


            string sql = SQL;

            List<T> res = new List<T>();

            bool suc = true;

            if(prms is object[])
            {
                suc = ReturnDS(false, sql, (object[])prms, connName);
            }
            else if (prms is Dictionary<string,object>)
            {
                suc = ReturnDS(false, sql, connName, (Dictionary<string, object>)prms);
            }
            else
            {
                suc = ReturnDS(false, sql, connName);
            }



            if (suc)
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    T tmpT = new T();

                    tmpT = FormERElement<T>(tmpT, dr);

                    res.Add(tmpT);
                }


            }

            return res;





        }

        
    }

 

    
}

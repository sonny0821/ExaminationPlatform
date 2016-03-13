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
    /// 数据库操作类
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
        /// <param name="currentDba">新数据库库操作激活器</param>
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
        /// Sql无返回操作
        /// </summary>
        /// <param name="isProc">是否存储过程</param>
        /// <param name="procname">sql语句或存储过程名</param>
        /// <param name="prms">参数表（按顺序）</param>
        /// <param name="connStr">连接名</param>
        /// <returns>返回是否执行成功</returns>
        public bool ExecuteNonQuery(bool isProc, string procname, object[] prms, string connStr)
        {
            return ExecuteNonQueryEx(isProc, procname, prms, null, connStr);
        }
        /// <summary>
        /// Sql无返回操作
        /// </summary>
        /// <param name="isProc">是否存储过程</param>
        /// <param name="procname">sql语句或存储过程名</param>
        /// <param name="connStr">连接名</param>
        /// <param name="keyPrms">参数表（按key）</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(bool isProc, string procname, string connStr, Dictionary<string, object> keyPrms)
        {
            return ExecuteNonQueryEx(isProc, procname, null, keyPrms, connStr);
        }
        /// <summary>
        /// Sql无返回操作
        /// </summary>
        /// <param name="isProc">是否存储过程</param>
        /// <param name="procname">sql语句或存储过程名</param>
        /// <param name="connStr">连接名</param>
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
                        ex = "启动数据引擎" + connStr + "错误";
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
        /// 返回数据集
        /// </summary>
        /// <param name="isProc">是否存储过程，否则为sql语句</param>
        /// <param name="Ex">返回值，如果成功为ok</param>
        /// <param name="procname">存储过程名/sql语句</param>
        /// <param name="prms">参数表，可为null</param>
        /// <param name="keyPrms">参数KV表，可为null</param>
        /// <param name="ds">需要返回的数据集</param>
        /// <param name="connStr">连接名</param>
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
                        Ex = "启动数据引擎" + connStr + "错误";
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
        /// 返回DataSet
        /// </summary>
        /// <param name="isProc">是否存储过程</param>
        /// <param name="procname">存储过程名或sql语句</param>
        /// <param name="prms">参数表</param>
        /// <param name="connStr">连接名称</param>
        /// <returns></returns>
        public bool ReturnDS(bool isProc, string procname, object[] prms, string connStr)
        {
            return ReturnDS(isProc, out ex, procname, prms,null, out Ds, connStr);
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="isProc">是否存储过程</param>
        /// <param name="procname">存储过程名或sql语句</param>
        /// <param name="connStr">连接名称</param>
        /// <param name="prms">参数KV表，可为null</param>
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
        /// 插入一个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="tableName">表名</param>
        /// <param name="connName">连接名</param>
        /// <param name="insertIgnoreColumns">插入忽略字段</param>
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
        /// 获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="PK_ID">主键</param>
        /// <param name="idField">主键字段名</param>
        /// <param name="tableName">表名</param>
        /// <param name="connName">连接名</param>
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
        /// 获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="SQL">SQL，返回第一行的实体</param>
        /// <param name="connName">连接名</param>
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
        /// 获取实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="SQL">SQL，返回所有行的实体列表</param>
        /// <param name="connName">连接名</param>
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
        /// 获取实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="isProc">是否存储过程</param>
        /// <param name="SQL">SQL语句</param>
        /// <param name="prms">参数，应为dictionary[string,object]或object[]</param>
        /// <param name="connName">连接名称</param>
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

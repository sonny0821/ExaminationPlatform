using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Data;
using System.Data.Common;

namespace UCADB
{
    public abstract class DatabaseOperator
    {
        protected Collection<DBCommandWrapper> commandSerial;


        protected DbConnection conn;

        protected DbConnection GetCurrentConnection()
        {
            return conn;
        }


        
        public abstract void LoadTextWrapper(string sqlText, object[] param);
        public abstract void LoadTextWrapper(string sqlText, Dictionary<string, object> param);
        public abstract void LoadCommandWrapper(string sqlText, object[] param);
        public abstract void LoadCommandWrapper(string sqlText, Dictionary<string,object> param);


        
        /// <summary>
        /// </summary>
        public abstract bool ExecuteNonQuery(out string returnValue, out int rowCount, out string ex);

        public abstract bool ExecuteScalar(out object scalarValue, out string ex);

        public abstract bool ExecuteDataSet(out DataSet dataset, out string ex);

        public abstract bool ExecuteDataReader(out IDataReader dataRdr, out string ex);

        public abstract string ExecuteBatch();

        public void AddCommand(DBCommandWrapper command)
        {
            throw new System.NotImplementedException();
        }

        public abstract string ExecuteTableToDatabase(string TableName);
    }
}

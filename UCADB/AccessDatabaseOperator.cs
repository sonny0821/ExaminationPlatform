using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Data.Common;

namespace UCADB
{
    public class AccessDatabaseOperator : DatabaseOperator
    {
        DBCommandWrapper wrapper = null;

        OleDbConnection conn;

        public AccessDatabaseOperator( DbConnection connection)
        {
            conn = (OleDbConnection)connection;
        }


        public override void LoadCommandWrapper(string sqlText, object[] param)
        {
            wrapper = new OleDBTextWarpper(sqlText, param);
        }

        public override void LoadCommandWrapper(string sqlText, Dictionary<string, object> param)
        {
            wrapper = new OleDBTextWarpper(sqlText, param);
        }

        public override void LoadTextWrapper(string sqlText, Dictionary<string, object> param)
        {
            wrapper = new OleDBTextWarpper(sqlText, param);
        }
        

        public override void LoadTextWrapper(string sqlText, object[] param)
        {
            wrapper = new OleDBTextWarpper(sqlText, param);
        }

        public override bool ExecuteNonQuery(out string returnValue, out int rowCount, out string ex)
        {
            bool res = true;
         

            wrapper.SetConnection(conn);

            OleDbCommand cmd = new OleDbCommand();
            wrapper.DoWrap(cmd);


            int i = 0;
            cmd.Connection = conn;

            try
            {
                rowCount = cmd.ExecuteNonQuery();
                ex = "ok";
            }
            catch (Exception e)
            {
                rowCount = -255;
                ex = e.Message;
                res = false;

            }

            if (cmd.Parameters.Contains("@returnValue"))
            {
                returnValue = cmd.Parameters["@returnValue"].Value.ToString();
            }
            else
            {
                returnValue = "";
            }

            return res;
        }

        public override bool ExecuteScalar(out object scalarValue, out string ex)
        {
            bool res = true;
          

            wrapper.SetConnection(conn);
            OleDbCommand cmd = new OleDbCommand();
            wrapper.DoWrap(cmd);


            int i = 0;
            cmd.Connection = conn;

            try
            {
                scalarValue = cmd.ExecuteScalar();
                ex = "ok";
            }
            catch (Exception e)
            {
                scalarValue = null;
                ex = e.Message;
                res = false;
            }

            return res;
        }

        public override bool ExecuteDataSet(out System.Data.DataSet dataset, out string ex)
        {
            bool res = true;
            dataset = new DataSet();
      

            wrapper.SetConnection(conn);

            OleDbDataAdapter sd = new OleDbDataAdapter();
            OleDbCommand cmd = new OleDbCommand();

            wrapper.DoWrap(cmd);

            sd.SelectCommand = cmd;



            sd.SelectCommand.Connection = conn;

            try
            {
                sd.Fill(dataset);
                ex = "ok";
            }
            catch (Exception e)
            {
                ex = e.Message;
                res = false;
            }

            return res;
        }

        public override bool ExecuteDataReader(out System.Data.IDataReader dataRdr, out string ex)
        {
            bool res = true;

          

            wrapper.SetConnection(conn);

            OleDbCommand cmd = new OleDbCommand();
            wrapper.DoWrap(cmd);



            cmd.Connection = conn;

            try
            {
                dataRdr = cmd.ExecuteReader();
                ex = "ok";
            }
            catch (Exception e)
            {
                dataRdr = null;
                ex = e.Message;
                res = false;
            }

            return res;
        }

        public override string ExecuteBatch()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string ExecuteTableToDatabase(string TableName)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}

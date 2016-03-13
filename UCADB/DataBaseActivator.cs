using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCADB
{
    public class DataBaseActivator
    {
       
        public DataBaseActivator()
        {
            
        }


        public virtual DbConnection active(ConnectionConfig cc, out DatabaseOperator DO)
        {
            DbConnection conn = null;

            switch (cc.dbType)
            {
                case "SqlServer":

                    conn = new SqlConnection(cc.connectionString);
                    conn.Open();

                    DO = new DatabaseFactory().GetDatabaseOperator(conn);

                    return conn;

                case "Access":

                    conn = new OleDbConnection(cc.connectionString);
                    conn.Open();

                    DO = new DatabaseFactory().GetDatabaseOperator(conn);

                    return conn;
                default:

                    conn = new SqlConnection(cc.connectionString);
                    conn.Open();

                    DO = new DatabaseFactory().GetDatabaseOperator(conn);

                    return conn;


            }

        }



    }
}

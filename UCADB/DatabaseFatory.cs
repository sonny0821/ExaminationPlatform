using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Common;


namespace UCADB
{
    public class DatabaseFactory
    {
        

        

        public static DatabaseOperator GetDatabaseOperator(dbTypeEnum dbType, string connName)
        {
            DbConnection conn = ConnectionInstance.Instance.ReturnDBConnection(connName);

            switch (dbType)
            {
                case dbTypeEnum.SqlDB: return new SqlDatabaseOperator(conn);
                case dbTypeEnum.AccessDB: return new AccessDatabaseOperator(conn);
                default: return new SqlDatabaseOperator(conn);
            }
        }

        public DatabaseOperator GetDatabaseOperator(DbConnection conn)
        {
          

            if (conn!=null)
            {
                if (conn is SqlConnection)
                {
                    return new SqlDatabaseOperator(conn);
                }
                if (conn is OleDbConnection)
                {
                    return new AccessDatabaseOperator(conn);
                }
                
            }
            return new SqlDatabaseOperator(conn);
        }
    }
}

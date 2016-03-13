using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;


namespace UCADB
{
    public class SqlProcWrapper:DBCommandWrapper
    {
        public string Procname = "";
        public object[] Param = null;
        public Dictionary<string, object> StructParam;
        public List<object> tmpParam = new List<object>();

        public SqlProcWrapper(string procname, object[] prms)
        {
            Procname = procname;
            Param = prms;
            
            
        }

        public SqlProcWrapper(string procname, Dictionary<string,object> prms)
        {
            Procname = procname;
            StructParam = prms;


        }

        public override void SetConnection(System.Data.Common.DbConnection con)
        {
 	         base.SetConnection(con);


             ArrayList al = GetProcParam(Procname);
             commandText = Procname;
             commandType = CommandType.StoredProcedure;

             object[] prms;
             if (Param != null)
             {
                 prms = Param;
             }
             else
             {
                 prms = tmpParam.ToArray();
             }

             for (int i = 0; i < al.Count && i < prms.Length; i++)
             {
                 parameter.Add(new SqlParameter(al[i].ToString(), prms[i]));
             }

             SqlParameter spm = new SqlParameter("@returnValue", SqlDbType.Int);
             spm.Direction = ParameterDirection.ReturnValue;

             parameter.Add(spm);
             
        }
        

        public SqlProcWrapper()
        {

        }

        

        public ArrayList GetProcParam(string procname)
        {
            SqlConnection conn = (SqlConnection)this.conn;


            SqlCommand cmd = new SqlCommand("Select Name,xtype from syscolumns where  id=object_id('" + procname + "') order by colorder", conn);
            cmd.CommandType = CommandType.Text;
            DataTable td = new DataTable();
            
            SqlDataAdapter myadapter = new SqlDataAdapter(cmd);
            myadapter.Fill(td);
            myadapter.Dispose();

            ArrayList al = new ArrayList();
            tmpParam.Clear();
            for (int i = 0; i < td.Rows.Count; i++)
            {
                al.Add(td.Rows[i][0].ToString());
                if (StructParam != null)
                {
                    if (StructParam.ContainsKey(td.Rows[i][0].ToString()))
                    {
                        tmpParam.Add(StructParam[td.Rows[i][0].ToString()]);
                    }
                    else if (StructParam.ContainsKey(td.Rows[i][0].ToString().Replace("@", "")))
                    {
                        tmpParam.Add(StructParam[td.Rows[i][0].ToString().Replace("@", "")]);
                    }
                    else
                    {
                        tmpParam.Add(UHibernateOperator.createTypeDefault(UHibernateOperator.getDbType((byte)td.Rows[i][1])));
                    }
                }
            }

            return al;
            
        }

        public ArrayList GetProcParam()
        {
            SqlConnection conn = (SqlConnection)this.conn;


            SqlCommand cmd = new SqlCommand("sp_sproc_columns", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable td = new DataTable();
            cmd.Parameters.AddWithValue("@procedure_name", (object)Procname);
            SqlDataAdapter myadapter = new SqlDataAdapter(cmd);
            myadapter.Fill(td);
            myadapter.Dispose();

            ArrayList al = new ArrayList();
            for (int i = 1; i < td.Rows.Count; i++)
            {
                al.Add(td.Rows[i][3].ToString());
            }

            return al;

        }
    }
}

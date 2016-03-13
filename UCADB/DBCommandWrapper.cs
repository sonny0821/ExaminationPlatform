using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace UCADB
{
    public abstract class DBCommandWrapper
    {
        protected string commandText;
        protected CommandType commandType = new CommandType();
        protected Collection<DbParameter> parameter = new Collection<DbParameter>();
        protected DbConnection conn = null;
        public virtual void SetConnection(DbConnection con)
        {
            conn = con;
        }




        public void DoWrap(DbCommand cmd)
        {

            
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;
            cmd.Parameters.Clear();
            try
            {
                foreach (DbParameter param in parameter)
                {
                    cmd.Parameters.Add(param);
                }
            }
            catch (Exception e)
            {
                string ex = e.Message;
            }
            
            
        }

        protected void PrepareParamText(ref string sqltext, int prmscount)
        {
            if (prmscount == 0)
                return;
            Regex rgx = new Regex(@"\[@.*?\]");
            MatchCollection Mtc = rgx.Matches(sqltext);


            //List<string> Expr = new List<string>();
            int i = 0;

            List<string> prmNames = new List<string>();

            foreach (Match mtcval in Mtc)
            {
                if(prmNames.Contains(mtcval.Value))
                {
                    continue;
                }
                //if (sqltext.IndexOf(mtcval.Value) >= 0)
                //{
                //    int tmp = sqltext.IndexOf(mtcval.Value);
                sqltext = sqltext.Replace(mtcval.Value, "@AutoExpr" + i.ToString());  //Substring(0, tmp) + "@AutoExpr" + i.ToString() + sqltext.Substring(tmp + mtcval.Value.Length);
                   // Expr.Add(mtcval.Value);
                prmNames.Add(mtcval.Value);
                i++;
                //}
            }

            int pBegin = sqltext.IndexOf("[**ParamBegin*]");
            int pEnd = sqltext.IndexOf("[**ParamEnd*]");
            if (pBegin >= 0 && pEnd >= 0)
            {
                if (sqltext.Length > pEnd + 13)
                {

                    sqltext = sqltext.Substring(0, pBegin) + sqltext.Substring(pEnd + 13);
                }
                else
                {
                    sqltext = "";
                }
            }

            

            

            
            



        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace UCADB
{
    public class SqlTextWrapper : DBCommandWrapper
    {
        public string SrcText = "";
        public string SqlText
        {
            get
            {
                return commandText;
            }
        }
        
        public SqlTextWrapper(string sqltext)
        {
            SrcText = sqltext;
            commandText = sqltext;
            commandType = System.Data.CommandType.Text;
        }

        public SqlTextWrapper(string sqltext, object[] prms)
        {
            SrcText = sqltext;
            PrepareParamText(ref sqltext, prms.Length);
            string aaa = sqltext;
            commandText = sqltext;
            commandType = System.Data.CommandType.Text;

            int i;
            parameter.Clear();
            for (i = 0; i < prms.Length; i++)
            {
                parameter.Add(new SqlParameter("@AutoExpr" + i.ToString(), prms[i]));  
            }

        }

        public SqlTextWrapper(string sqltext, Dictionary<string,object> prms)
        {
            SrcText = sqltext;
            PrepareParamText(ref sqltext, prms);
            string aaa = sqltext;
            commandText = sqltext;
            commandType = System.Data.CommandType.Text;

            
          

        }

        protected void PrepareParamText(ref string sqltext, Dictionary<string, object> inputPrms)
        {

            parameter.Clear();

            Regex rgx = new Regex(@"\[@.*?\]");
            MatchCollection Mtc = rgx.Matches(sqltext);

            List<string> fldSymbol = new List<string>();


            //List<string> Expr = new List<string>();
            int i = 0;
            foreach (Match mtcval in Mtc)
            {
                if (fldSymbol.Contains(mtcval.Value))
                {
                    continue;
                }

                fldSymbol.Add(mtcval.Value);
                //if (sqltext.IndexOf(mtcval.Value) >= 0)
                //{
                //    int tmp = sqltext.IndexOf(mtcval.Value);

                if (inputPrms.ContainsKey(mtcval.Value.Substring(2, mtcval.Value.Length - 3)))
                {
                    parameter.Add(new SqlParameter("@AutoExpr" + i.ToString(), inputPrms[mtcval.Value.Substring(2, mtcval.Value.Length - 3)]));
                }
                else
                {
                    parameter.Add(new SqlParameter("@AutoExpr" + i.ToString(), ""));
                }



                sqltext = sqltext.Replace(mtcval.Value, "@AutoExpr" + i.ToString());  //Substring(0, tmp) + "@AutoExpr" + i.ToString() + sqltext.Substring(tmp + mtcval.Value.Length);
                // Expr.Add(mtcval.Value);
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

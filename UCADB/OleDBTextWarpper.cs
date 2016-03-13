using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace UCADB
{
    class OleDBTextWarpper : DBCommandWrapper
    {
        public string SrcText = "";
        public string SqlText
        {
            get
            {
                return commandText;
            }
        }

        public OleDBTextWarpper(string sqltext)
        {
            SrcText = sqltext;
            commandText = sqltext;
            commandType = System.Data.CommandType.Text;
        }

        public OleDBTextWarpper(string sqltext, object[] prms)
        {
            SrcText = sqltext;
            PrepareParamText(ref sqltext, prms.Length);
            string aaa = sqltext;
            commandText = sqltext;
            commandType = System.Data.CommandType.Text;

            int i;
            parameter.Clear();
            OleDbParameter odp;
            for (i = 0; i < prms.Length; i++)
            {
                odp = new OleDbParameter("@AutoExpr" + i.ToString(), prms[i]);
                if (prms[i] is DateTime)
                {
                    odp.OleDbType = OleDbType.Date;
                }
                if (prms[i] is int)
                {
                    odp.OleDbType = OleDbType.Integer;
                }
                if (prms[i] is decimal)
                {
                    odp.OleDbType = OleDbType.Decimal;
                }
                if (prms[i] is double || prms[i] is float)
                {
                    odp.OleDbType = OleDbType.Double;
                }
                parameter.Add(odp);
            }

        }


        public OleDBTextWarpper(string sqltext, Dictionary<string, object> prms)
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
                    object prm = inputPrms[mtcval.Value.Substring(2, mtcval.Value.Length - 3)];
                    OleDbParameter odp = new OleDbParameter("@AutoExpr" + i.ToString(), prm);
                    if (prm is DateTime)
                    {
                        odp.OleDbType = OleDbType.Date;
                    }
                    if (prm is int)
                    {
                        odp.OleDbType = OleDbType.Integer;
                    }
                    if (prm is decimal)
                    {
                        odp.OleDbType = OleDbType.Decimal;
                    }
                    if (prm is double || prm is float)
                    {
                        odp.OleDbType = OleDbType.Double;
                    }
                    parameter.Add(odp);

                    
                }
                else
                {
                    parameter.Add(new OleDbParameter("@AutoExpr" + i.ToString(), ""));
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
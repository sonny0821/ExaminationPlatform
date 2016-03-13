using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace UCADB
{
    public class UHibernateOperator
    {
        TableOperator to = new TableOperator();

        public string ex = "";
        

        protected List<string> _tableNames = new List<string>();
        protected Dictionary<string, string[]> _identifyFields = new Dictionary<string, string[]>();

        protected Dictionary<string, DataTable> _hiberTable = new Dictionary<string, DataTable>();
        protected Dictionary<string, List<string>> _hiberColumn = new Dictionary<string, List<string>>();
        protected Dictionary<string, List<string>> _dataColumn = new Dictionary<string, List<string>>();
        protected Dictionary<string, bool> _isTableIntIdentify = new Dictionary<string, bool>();
        protected string _cnStr = "";

        protected string sqlModel = @"if exists(Select * from ?Tbl? where ?idCon?)
Update ?Tbl? Set ?upStr? where ?idCon?
else
Insert into ?Tbl? (?col?) Values(?val?)";

        protected string genIdentityCondition(string tableName, Dictionary<string, object> prms)
        {
            List<string> resLst = new List<string>();
          
            if (_identifyFields.ContainsKey(tableName))
            {
                foreach (string idField in _identifyFields[tableName])
                {
                    if (prms.ContainsKey(idField))
                    {
                        resLst.Add(idField + "=[@" + idField + "]");
                      
                    }
                }
            }

            string res = string.Join(" and ", resLst.ToArray());


         

            return res;



        }

        protected string genParamTable(string tableName, Dictionary<string, object> prms, out object[] oprms)
        {
            StringBuilder sbr = new StringBuilder();
            List<object> resprms = new List<object>(); 
            sbr.AppendLine("[**ParamBegin*]");
            if (_hiberColumn.ContainsKey(tableName))
            {
                foreach (string cName in _hiberColumn[tableName])
                {
                    if (prms.ContainsKey(cName))
                    {
                        sbr.Append("[@" + cName + "]");
                        resprms.Add(prms[cName]);
                    }
                }
             
            }
            sbr.Append("[**ParamEnd*]");
            oprms = resprms.ToArray();
            return sbr.ToString();
        }

        protected string genUpdateStr(string tableName, Dictionary<string, object> prms)
        {
            List<string> colLst = new List<string>();
            if (_dataColumn.ContainsKey(tableName))
            {
                foreach (string cName in _dataColumn[tableName])
                {
                    if (prms.ContainsKey(cName))
                    {
                        colLst.Add(cName + "=[@" + cName + "]");
                        
                    }
                }

            }

            string res = "Update " + tableName + " Set " + string.Join(",", colLst.ToArray());

            return res;
        }

        protected string genInsertString(string tableName, Dictionary<string, object> prms)
        {
            bool intIdentify = false;
            if (_isTableIntIdentify.ContainsKey(tableName))
            {
                intIdentify = _isTableIntIdentify[tableName];
            }
            List<string> ids;
            if (_identifyFields.ContainsKey(tableName))
            {
                ids = new List<string>(_identifyFields[tableName]);
            }
            else
            {
                ids = new List<string>();
            }
            List<string> colLst = new List<string>();
            List<string> valLst = new List<string>();
            if (_hiberTable.ContainsKey(tableName))
            {
                DataRow defaultRow = InitRowData(_hiberTable[tableName]);
                foreach (DataColumn dc in defaultRow.Table.Columns)
                {
                    if (intIdentify && ids.Contains(dc.ColumnName))
                    {
                        continue;
                    }

                    colLst.Add(dc.ColumnName);
                    if (prms.ContainsKey(dc.ColumnName))
                    {
                        valLst.Add("[@" + dc.ColumnName + "]");

                    }
                    else
                    {
                        valLst.Add("'" + defaultRow[dc.ColumnName].ToString() + "'");
                    }
                }

                string colStr = string.Join(",", colLst.ToArray());
                string valStr = string.Join(",", valLst.ToArray());

                return "Insert into " + tableName + " (" + colStr + ") values(" + valStr + ")";
            }
            else
            {
                return "";
            }

            





        }

    
        public bool ModifyDataRow(string tableName,Dictionary<string,object> prms)
        {
            object[] tmpPrms;
            string prmHeader = genParamTable(tableName, prms, out tmpPrms) + Environment.NewLine;

            

            string sql = prmHeader + genUpdateStr(tableName, prms) + " Where " + genIdentityCondition(tableName, prms);


            if (to.ExecuteNonQuery(false, sql, tmpPrms, _cnStr))
            {
                if (to.RowCount <= 0)
                {

                    sql = genInsertString(tableName, prms);
                    if (sql != "")
                    {
                        sql = prmHeader + Environment.NewLine + sql;
                        bool res = to.ExecuteNonQuery(false, sql, tmpPrms, _cnStr);

                        ex = to.ex;
                        return res;

                    }
                    else
                    {
                        ex = "Insert语句无法生成";
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                ex = to.ex;
                return false;
            }
        }
        


        public static DataTable GenTableClone(string tableName, string connectionName)
        {
            DataTable resDt = new DataTable();
            string sql = "Select Name,xtype from syscolumns where  id=object_id('" + tableName + "') order by colorder";
            TableOperator to = new TableOperator();
            if (to.ReturnDS(false, sql, null, connectionName))
            {
                foreach (DataRow dr in to.Ds.Tables[0].Rows)
                {
                    resDt.Columns.Add(dr["Name"].ToString(), getDbType(Convert.ToInt32(dr["xtype"].ToString())));

                    
                }
            }

            return resDt;
        }

        public static DataRow InitRowData(DataTable inputDt)
        {
            DataRow resDr = inputDt.NewRow(); 
            foreach (DataColumn dc in inputDt.Columns)
            {
                resDr[dc.ColumnName] = createTypeDefault(dc.DataType);
                
            }




            return resDr;
            

        }

        public DataTable GetStandardTable(string tableName,out List<string> columns,out List<string> dataColumns)
        {
            columns = new List<string>();
            dataColumns = new List<string>();
            DataTable resDt = new DataTable();
            string sql = "Select Name,xtype from syscolumns where  id=object_id('" + tableName + "') order by colorder";

            if (to.ReturnDS(false, sql, null, _cnStr))
            {
                foreach (DataRow dr in to.Ds.Tables[0].Rows)
                {
                    resDt.Columns.Add(dr["Name"].ToString(), getDbType((byte)dr["xtype"]));
                    
                    columns.Add(dr["Name"].ToString());
                    List<string> tmpLst = new List<string>();
                    if (_identifyFields[tableName] != null)
                    {
                        tmpLst = new List<string>(_identifyFields[tableName]);
                    }

                    if (!tmpLst.Contains(dr["Name"].ToString()))
                    {
                        dataColumns.Add(dr["Name"].ToString());
                    }
                }
            }

            return resDt;
        }


        public static Type getDbType(int typeNum)
        {
            switch (typeNum)
            {
                case 36: return typeof(Guid);
                case 56: return typeof(int);
                case 48: return typeof(int);
                case 59: return typeof(double);
                case 62: return typeof(double);
                case 60: return typeof(decimal);
                case 61: return typeof(DateTime);
                case 167: return typeof(string);
               
                default: return typeof(string);
            }


        }

        public static object createTypeDefault(Type type)
        {



            if (type == typeof(Guid)) return Guid.Empty;
            if (type == typeof(int)) return 0;
            if (type == typeof(short)) return 0;
            if (type == typeof(byte)) return 0;
            if (type == typeof(float)) return 0.00;
            if (type == typeof(double)) return 0.00;
            if (type == typeof(decimal)) return 0.00M;
            if (type == typeof(DateTime)) return new DateTime(1900, 1, 1);
            return "";
            
        }

        public static bool checkGuid(string input)
        {
            Regex rgx1 = new Regex("[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}");
            return rgx1.IsMatch(input);
        }

        public static object parseValue(string input,Type type)
        {



            if (type == typeof(Guid))
            {
                if (checkGuid(input))
                {
                    return new Guid(input);
                }
                else
                {
                    return Guid.Empty;
                }
            }
            else if (type == typeof(int))
            {
                int tmp;
                if (int.TryParse(input, out tmp))
                {
                    return tmp;
                }
                else
                {
                    return 0;
                }
            }
            else if (type == typeof(short))
            {
                short tmp;
                if (short.TryParse(input, out tmp))
                {
                    return tmp;
                }
                else
                {
                    return 0;
                }
            }
            else if (type == typeof(byte))
            {
                byte tmp;
                if (byte.TryParse(input, out tmp))
                {
                    return tmp;
                }
                else
                {
                    return 0;
                }
            }
            else if (type == typeof(float))
            {
                float tmp;
                if (float.TryParse(input, out tmp))
                {
                    return tmp;
                }
                else
                {
                    return 0.00;
                }
            }
            else if (type == typeof(double))
            {
                double tmp;
                if (double.TryParse(input, out tmp))
                {
                    return tmp;
                }
                else
                {
                    return 0.00;
                }
            }
            else if (type == typeof(decimal))
            {
                decimal tmp;
                if (decimal.TryParse(input, out tmp))
                {
                    return tmp;
                }
                else
                {
                    return 0.00M;
                }
            }
            else if (type == typeof(DateTime))
            {
                DateTime tmp;
                if (DateTime.TryParse(input, out tmp))
                {
                    return tmp;
                }
                else
                {
                    return new DateTime(1900, 1, 1);
                }
            }
            else
            {
                return input;
            }
       
         

        }




        public UHibernateOperator(string connectionName, string tableName,bool isIdIntIdentify, params string[] identifyField)
        {
            _cnStr = connectionName;
          

            LoadTable(tableName, isIdIntIdentify, identifyField);

        }

        public void LoadTable(string tableName,bool isIdIntIdentify, string[] identifyField)
        {
            if (!_tableNames.Contains(tableName))
            {
                _tableNames.Add(tableName);
                _identifyFields.Add(tableName,identifyField);
                _isTableIntIdentify.Add(tableName, isIdIntIdentify);
                InitTableObject(tableName);
            }
        }

        public void InitTableObject(string tableName)
        {
            if (!_hiberTable.ContainsKey(tableName))
            {
                List<string> cols, dcols;
                _hiberTable.Add(tableName, GetStandardTable(tableName, out cols, out dcols));
                _hiberColumn.Add(tableName, cols);
                _dataColumn.Add(tableName, dcols);
            }
        }

        //public object[] getParamForTable(Dictionary<string, object> prms, string table)
        //{
        //    DataRow dr = InitRowData(_tableNames
        //}


        public string getInsertSql()
        {

            

            StringBuilder sbr = new StringBuilder();
            sbr.AppendLine("[**ParamBegin*]");
            foreach (string tName in _hiberColumn.Keys)
            {
                string[] tmpArr = _hiberColumn[tName].ToArray();
                sbr.Append("[@" + string.Join("][@", tmpArr) + "]");
            }
            sbr.Append("[**ParamEnd*]");

            foreach (string tName in _tableNames)
            {
                sbr.Append(getInsertSqlforTable(tName));
            }

            return sbr.ToString();
            
        }

        protected string getInsertSqlforTable(string tableName)
        {
            string res = sqlModel;

            res = res.Replace("?Tbl?", tableName);

            List<string> tmplst = new List<string>();

            List<string> idlst = new List<string>(_identifyFields[tableName]);

            foreach (string ids in _identifyFields[tableName])
            {
                tmplst.Add("[" + ids + "]=[@" + ids + "]");
            }

            res = res.Replace("?idCon?", string.Join(" and ", tmplst.ToArray()));

            tmplst.Clear();

            List<string> dataCol = new List<string>();

            foreach (string col in _dataColumn[tableName])
            {
               
                tmplst.Add("[" + col + "]=[@" + col + "]");
                
            }

            res = res.Replace("?upStr?", string.Join(",", tmplst.ToArray()));

            string colStr, valStr;

            if (_isTableIntIdentify[tableName])
            {
                colStr = "[" + string.Join("],[", _dataColumn[tableName].ToArray()) + "]";
                valStr = "[@" + string.Join("],[", _dataColumn[tableName].ToArray()) + "]";
            }
            else
            {
                colStr = "[" + string.Join("],[", _hiberColumn[tableName].ToArray()) + "]";
                valStr = "[@" + string.Join("],[", _hiberColumn[tableName].ToArray()) + "]";
            }

            res = res.Replace("?col?", colStr);

            res = res.Replace("?val?", valStr);

            return res;

        }






    }


}


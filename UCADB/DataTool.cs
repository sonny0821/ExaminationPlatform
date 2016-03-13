using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

namespace UCADB
{
    public class DataTool
    {
        public static DataTable SortTable(DataTable inputDt, List<string> sortedList, string sortField)
        {
            DataTable resDt = inputDt.Clone();

            foreach (string id in sortedList)
            {
                int index = -1;
                for (int i = 0; i < inputDt.Rows.Count; i++)
                {
                    if (inputDt.Rows[i][sortField].ToString().ToLower() == id)
                    {
                        index = i;
                        break;
                    }
                }
                if (index >= 0)
                {
                    resDt.ImportRow(inputDt.Rows[index]);
                    
                }
                
            }

            return resDt;
        }

        public static DataTable CreateOrderTableByList(DataTable inputDt, List<string> sortedList, string[] sortFields)
        {
            DataTable resDt = inputDt.Clone();

            foreach (string id in sortedList)
            {
                int index = -1;
                for (int i = 0; i < inputDt.Rows.Count; i++)
                {
                    foreach (string sortField in sortFields)
                    {

                        if (inputDt.Rows[i][sortField].ToString().ToLower() == id)
                        {
                            index = i;
                            resDt.ImportRow(inputDt.Rows[index]);
                            break;
                        }
                    }
                }
              

            }

            return resDt;
        }

        //public static DataTable SortNFillTable(DataTable inputDt, List<string[]> sortedList, string sortField,string[] fillField)
        //{
        //    DataTable resDt = inputDt.Clone();

        //    foreach (string[] id in sortedList)
        //    {
        //        DataRow dr = resDt.NewRow();
        //        dr[sortField] = id[0];

        //        for (int i = 0; i < (id.Length - 1) && i < fillField.Length; i++)
        //        {
        //            dr[fillField[i]] = id[i + 1];
        //        }


                



        //        int index = -1;
        //        for (int i = 0; i < inputDt.Rows.Count; i++)
        //        {
        //            if (inputDt.Rows[i][sortField].ToString().ToLower() == id[0])
        //            {
        //                index = i;
        //                break;
        //            }
        //        }
        //        if (index >= 0)
        //        {
        //            resDt.ImportRow(inputDt.Rows[index]);

        //        }
        //        else
        //        {
        //            resDt.Rows.Add(dr);
        //        }
                

        //    }

        //    return resDt;
        //}

        public static string MapUIPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "common\\ui\\" + fileName);
        }
    }
}

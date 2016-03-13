using System;
using System.Collections.Generic;
using System.Text;

namespace UCADB
{
    public  class Common
    {
        public static object ObjParse(string Input, string objType)
        {
            object objVal = "";
            try
            {
                switch (objType)
                {
                    case "Guid": objVal = new Guid(Input); break;
                    case "Int": objVal = Convert.ToInt32(Input); break;
                    case "Byte": objVal = Convert.ToByte(Input); break;
                    case "Single": objVal = Convert.ToSingle(Input); break;
                    case "Decimal": objVal = Convert.ToDecimal(Input); break;
                    case "DateTime": objVal = Convert.ToDateTime(Input); break;
                    default: objVal = Input; break;

                }
            }
            catch (Exception e)
            {
                switch (objType)
                {
                    case "Guid": objVal = Guid.Empty; break;
                    case "Int": objVal = 0; break;
                    case "Byte": objVal = 0; break;
                    case "Single": objVal = 0.0; break;
                    case "Decimal": objVal = 0.00M; break;
                    case "DateTime": objVal = new DateTime(1900, 1, 1); break;
                    default: objVal = Input; break;

                }
            }

            return objVal;

        }
    }
}

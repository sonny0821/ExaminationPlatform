using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace UCADB
{
    public class CommonObject
    {
        protected object _obj = "";
        protected string XType = "String";

        public static int IntParse(string input)
        {
            int res;
            if (int.TryParse(input, out res))
            {
                return res;
            }
            else
            {
                return 0;
            }


        }

        public static double DoubleParse(string input)
        {
            double res;
            if (double.TryParse(input, out res))
            {
                return res;
            }
            else
            {
                return 0.0;
            }
        }

        public static DateTime DateTimeParse(string input)
        {
            DateTime res;
            if (DateTime.TryParse(input, out res))
            {
                return res;
            }
            else
            {
                return new DateTime(1900, 1, 1);
            }
        }

        public static decimal DecimalParse(string input)
        {
            decimal res;
            if (decimal.TryParse(input, out res))
            {
                return res;
            }
            else
            {
                return 0.0M;
            }
        }

        public CommonObject()
        {
            XType = "String";
            _obj = "";
        }

        public void loadObject(string type, object input)
        {
            bool correct = false;
            switch (type)
            {
                case "Int": input = IntParse(input.ToString()); correct = true; break;
                case "Double": input = DoubleParse(input.ToString()); correct = true; break;
                case "DateTime": input = DateTimeParse(input.ToString()); correct = true; break;
                    

            }

            if (correct)
            {
                _obj = input;
                XType = type;
            }
        }

        public void loadObject(object input)
        {
            if (input is int)
            {
                XType = "Int";
            }
            else if (input is float)
            {
                XType = "Double";
                input = Convert.ToDouble((float)input);
            }
            else if (input is double)
            {
                XType = "Double";
            }
            else if (input is decimal)
            {
                XType = "Double";
                input = Convert.ToDouble((decimal)input);
            }
            else if (input is DateTime)
            {
                XType = "DateTime";

            }
            else if (input is XmlDocument)
            {
                XType = "Xml";
            }
            else
            {
                XType = "String";
                input = input.ToString();
            }

            _obj = input;
        }

        public CommonObject(object input)
        {
            loadObject(input);
        }

        public object Result
        {
            get
            {
                return _obj;
            }
        }
        public string ObjType
        {
            get
            {
                return XType;
            }
        }

        
    }
}

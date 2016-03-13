using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationPlatform.Entities.Common
{
    public class Result
    {
        private bool isSuccess;
        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }

        private string errorCode;
        public string ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }

        private object data;
        public object Data
        {
            get { return data; }
            set { data = value; }
        }

        private Exception oException;
        public Exception OException
        {
            get { return oException; }
        }

        public void SetException(Exception ex)
        {
            isSuccess = false;
            oException = ex;
        }

        public void SetException(string eMsg)
        {
            isSuccess = false;
            oException = new Exception(eMsg);
        }

        public void SetException(string eMsg, Exception innerException)
        {
            isSuccess = false;
            oException = new Exception(eMsg, innerException);
        }
    }
}

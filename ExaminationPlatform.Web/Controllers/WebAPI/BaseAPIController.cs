using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UCADB;

namespace ExaminationPlatform.Web.Controllers.WebAPI
{
    public class BaseAPIController : ApiController
    {
        protected TableOperator opTo = new TableOperator();
    }
}

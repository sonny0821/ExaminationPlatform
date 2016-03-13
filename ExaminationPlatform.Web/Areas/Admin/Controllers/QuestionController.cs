using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExaminationPlatform.Web.Areas.Admin.Controllers
{
    public class QuestionController : ManageController
    {
        //
        // GET: /Admin/Question/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Info()
        {
            return View();
        }
    }
}

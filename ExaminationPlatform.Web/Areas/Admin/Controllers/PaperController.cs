using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExaminationPlatform.Web.Areas.Admin.Controllers
{
    public class PaperController : Controller
    {
        //
        // GET: /Admin/Paper/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Template()
        {
            return View();
        }

        public ActionResult Plan()
        {
            return View();
        }
    }
}

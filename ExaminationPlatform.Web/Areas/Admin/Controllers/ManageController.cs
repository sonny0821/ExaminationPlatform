using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ExaminationPlatform.Web.Areas.Admin.Controllers
{
    public class ManageController : Controller
    {
        //
        // GET: /Admin/Administrator/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string account, string pwd)
        {
            if (account == "admin" && pwd == "123456")
            {
                
                return RedirectToAction("Index", "Question");
            }
            //MD5 md5 = new MD5Cng();
            //byte[] output = md5.ComputeHash(Encoding.Default.GetBytes(password));
            //string md5Pass = BitConverter.ToString(output);
            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Excel.Web.Controllers
{
    public class AccountingController : Controller
    {
        // GET: Accounting
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}
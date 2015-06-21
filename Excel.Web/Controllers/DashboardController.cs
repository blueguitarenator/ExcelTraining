using Excel.Entities;
using Excel.Web.DataContexts;
using Excel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Excel.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Dashboard
        public ActionResult Index()
        {

            DashboardModel model = new DashboardModel();

            model.MySessions = getFutureSessions();

            return View(model);
        }

        private List<Session> getFutureSessions()
        {
            DateTime saveNow = DateTime.Now.Date;
            var userId = User.Identity.GetUserId();
            var appUser = db.Users.Where(u => u.Id == userId).SingleOrDefault();
            var athlete = db.Athletes.Where(a => a.Id == appUser.Athlete.Id).SingleOrDefault();


            var q = from s in db.Sessions
                    where s.Day.Year >= saveNow.Year &&
                        s.Day.Month >= saveNow.Month &&
                        s.Day.Day >= saveNow.Day &&
                        s.Athletes.Any(a => a.Id == athlete.Id)
                    select s;


            return q.ToList();
        }
    }
}
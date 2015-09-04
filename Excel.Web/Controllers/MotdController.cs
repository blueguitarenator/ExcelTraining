using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel.Web.Models;
using Microsoft.AspNet.Identity;

namespace Excel.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class MotdController : Controller
    {
        private IAthleteRepository athleteRepository;

        public Func<string> GetUserId; //For testing

        public MotdController()
        {
            athleteRepository = new AthleteRepository();
            GetUserId = () => User.Identity.GetUserId();
        }

        public MotdController(IAthleteRepository db)
        {
            athleteRepository = db;
        }

        // GET: Motd
        public ActionResult Index()
        { 
            MotdViewModel motdViewModel = new MotdViewModel();
            var motd = athleteRepository.GetMotd();
            if (motd != null)
            {
                motdViewModel.Motd = motd.Message;
                motdViewModel.ExpireDateTime = motd.ExpireDateTime;
            }
            else
            {
                motdViewModel.ExpireDateTime = DateTime.Now;
            }
            return View(motdViewModel);
        }
    }
}
using Excel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Excel.Web.Controllers
{
    public class ScheduleController : Controller
    {
        private IAthleteRepository athleteRepository;
      
        public ScheduleController()
        {
            athleteRepository = new AthleteRepository();
            //GetUserId = () => User.Identity.GetUserId();
        }

        public ScheduleController(IAthleteRepository db)
        {
            athleteRepository = db;
        }

        // GET: Schedule
        public ActionResult Index()
        {
            ScheduleViewModel model = new ScheduleViewModel();
            model.DardennePersonalTrainingSchedule = athleteRepository.GetDardenneSchedule(Entities.AthleteTypes.PersonalTraining);
            model.DardenneSportsTrainingSchedule = athleteRepository.GetDardenneSchedule(Entities.AthleteTypes.SportsTraining);

            return View(model);
        }
    }
}
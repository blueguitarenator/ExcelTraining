using Excel.Entities;
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
            model.DardennePersonalTrainingSchedule = athleteRepository.GetDardenneSchedule(AthleteTypes.PersonalTraining);
            model.DardenneSportsTrainingSchedule = athleteRepository.GetDardenneSchedule(AthleteTypes.SportsTraining);

            return View(model);
        }

        public ActionResult ChangePersonalStatus(bool ischecked, int hour)
        {
            var schedules = athleteRepository.GetDardenneSchedule(AthleteTypes.PersonalTraining).ToList();
            foreach(var schedule in schedules)
            {
                if (schedule.Hour == hour)
                {
                    athleteRepository.SetScheduleStatus(schedule.Id, ischecked);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult ChangeSportsStatus(bool ischecked, int hour)
        {
            var schedules = athleteRepository.GetDardenneSchedule(AthleteTypes.SportsTraining).ToList();
            foreach (var schedule in schedules)
            {
                if (schedule.Hour == hour)
                {
                    athleteRepository.SetScheduleStatus(schedule.Id, ischecked);
                }
            }
            return RedirectToAction("Index");
        }

    }
}
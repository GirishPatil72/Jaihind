using JaiHindEdutech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JaiHindEdutech.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateCoachingCentre()
        {
            CoachingCentreModel model = new CoachingCentreModel();
            return View(model);
        }
        public ActionResult EditCoachingCentre()
        {
            return View();
        }
    }
}
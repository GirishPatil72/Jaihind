using JaiHindEdutech.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JaiHindEdutech.Controllers
{
    public class BaseController : Controller
    {
        protected List<CollegeInfo> CollegeInfoSharedData;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Example: fetch data (from DB, config, user session, etc.)
            CollegeInfoSharedData = GetCommonData();

            // Optional: pass it to all views via ViewBag
            ViewBag.CollegeInfoSharedData = CollegeInfoSharedData;
        }

        private List<CollegeInfo> GetCommonData()
        {
            using (JaiHindEduEntitiesNew db = new JaiHindEduEntitiesNew())
            {
                return db.CollegeInfoes.Where(x => x.IsActive == true).ToList();
            }
        }
    }
}
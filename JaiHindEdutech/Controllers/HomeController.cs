using JaiHindEdutech.Entity;
using JaiHindEdutech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace JaiHindEdutech.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            using (var db = new JaiHindEduEntitiesNew())
            {
                var admissions = db.AddmissionForms
                    .Where(x => x.IsDeleted == false)
                    .ToList();

                var academicYears = db.LutAcademicYears
                    .OrderBy(x => x.AcademicYearId)
                    .ToList();

                var colleges = db.CollegeInfoes
                    .Where(x => x.IsActive == true)
                    .ToList();

                var collegeLookup = colleges.ToDictionary(x => x.CollegeInfoId, x => x.CollegeName);

                var currentAcademicYearId = academicYears
                    .Where(x => x.IsCurrent == true)
                    .Select(x => x.AcademicYearId)
                    .FirstOrDefault();

                if (currentAcademicYearId == 0 && academicYears.Any())
                {
                    currentAcademicYearId = academicYears.Last().AcademicYearId;
                }

                var currentAcademicYearName = academicYears
                    .Where(x => x.AcademicYearId == currentAcademicYearId)
                    .Select(x => x.Year)
                    .FirstOrDefault() ?? "All Academic Years";

                var collegeAdmissions = admissions
                    .Where(a => a.CollegeInfoId.HasValue)
                    .GroupBy(a => a.CollegeInfoId.Value)
                    .Select(g =>
                    {
                        string collegeName;
                        return collegeLookup.TryGetValue(g.Key, out collegeName)
                            ? new NamedCountItem { Name = collegeName, Count = g.Count() }
                            : null;
                    })
                    .Where(x => x != null)
                    .OrderByDescending(x => x.Count)
                    .ThenBy(x => x.Name)
                    .ToList();

                var admissionsByYearLookup = admissions
                    .Where(a => a.AcademicYearId.HasValue)
                    .GroupBy(a => a.AcademicYearId.Value)
                    .ToDictionary(g => g.Key, g => g.Count());

                var academicYearAdmissions = academicYears
                    .Select(ay => new NamedCountItem
                    {
                        Name = ay.Year,
                        Count = admissionsByYearLookup.ContainsKey(ay.AcademicYearId)
                            ? admissionsByYearLookup[ay.AcademicYearId]
                            : 0
                    })
                    .ToList();

                var classAdmissions = admissions
                    .Where(a => !string.IsNullOrWhiteSpace(a.ClassName))
                    .GroupBy(a => a.ClassName)
                    .Select(g => new NamedCountItem
                    {
                        Name = g.Key,
                        Count = g.Count()
                    })
                    .OrderByDescending(x => x.Count)
                    .ThenBy(x => x.Name)
                    .ToList();

                var model = new HomeDashboardViewModel
                {
                    TotalAdmissions = admissions.Count,
                    CurrentAcademicYearAdmissions = currentAcademicYearId > 0
                        ? admissions.Count(a => a.AcademicYearId == currentAcademicYearId)
                        : 0,
                    TotalColleges = colleges.Count,
                    TotalAcademicYears = academicYears.Count,
                    CurrentAcademicYearName = currentAcademicYearName,
                    CollegeAdmissions = collegeAdmissions.Take(10).ToList(),
                    AcademicYearAdmissions = academicYearAdmissions,
                    ClassAdmissions = classAdmissions,
                    TopColleges = collegeAdmissions.Take(5).ToList()
                };

                return View(model);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult AccessDenied(string message)
        {
            ViewBag.Message = string.IsNullOrWhiteSpace(message)
                ? "You are not authorized to take this action."
                : message;

            return View();
        }

        [AllowAnonymous]
        public ActionResult Error(string reference)
        {
            ViewBag.ErrorReference = reference;
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
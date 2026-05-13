using JaiHindEdutech.Entity;
using JaiHindEdutech.Infrastructure;
using JaiHindEdutech.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace JaiHindEdutech.Controllers
{
    public class CoachingCentresController : BaseController
    {
        private JaiHindEduEntitiesNew db = new JaiHindEduEntitiesNew();

        private string CurrentUserName
        {
            get { return User != null && User.Identity != null && User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty; }
        }
        // GET: CoachingCentres
        [PermissionAuthorize(Right = "View")]
        public ActionResult Index()
        {
            return View(db.CoachingCentres.ToList());
        }

        // GET: CoachingCentres/Details/5
        [PermissionAuthorize(Right = "View")]
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminModel adminModel = new AdminModel();
            return View(adminModel.GetCoachingCentreInfo(id));
        }

        // GET: CoachingCentres/Create
        [PermissionAuthorize(Right = "Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CoachingCentres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CoachingCentreId,CoachingCentreName,AddressLine1,AddressLine2,City,State,Zip,ContactNo1,IsActive,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] CoachingCentreModel coachingCentre)
        {
            AdminModel adminModel = new AdminModel();
            if (ModelState.IsValid)
            {
                coachingCentre.CreatedBy = CurrentUserName;
                coachingCentre.CreatedOn = DateTime.Now;
                coachingCentre.ModifiedBy = CurrentUserName;
                coachingCentre.ModifiedOn = DateTime.Now;

                adminModel.CreateCoachingCentre(coachingCentre);
                return RedirectToAction("Index");
            }

            return View(coachingCentre);
        }

        // GET: CoachingCentres/Edit/5

        [PermissionAuthorize(Right = "Edit")]
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminModel adminModel = new AdminModel();
            var coachingCentre = adminModel.GetCoachingCentreInfo(id);
            if (coachingCentre == null)
            {
                return HttpNotFound();
            }
            return View(coachingCentre);
        }

        // POST: CoachingCentres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CoachingCentreId,CoachingCentreName,AddressLine1,AddressLine2,City,State,Zip,ContactNo1,IsActive,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] CoachingCentreModel coachingCentre)
        {
            if (ModelState.IsValid)
            {
                coachingCentre.ModifiedBy = CurrentUserName;
                coachingCentre.ModifiedOn = DateTime.Now;
                AdminModel adminModel = new AdminModel();
                adminModel.CreateCoachingCentre(coachingCentre);
                return RedirectToAction("Index");
            }
            return View(coachingCentre);
        }

        // GET: CoachingCentres/Delete/5
        [PermissionAuthorize(Right = "Edit")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoachingCentre coachingCentre = db.CoachingCentres.Find(id);
            if (coachingCentre == null)
            {
                return HttpNotFound();
            }
            return View(coachingCentre);
        }

        // POST: CoachingCentres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //CoachingCentre coachingCentre = db.CoachingCentres.Find(id);
            //db.CoachingCentres.Remove(coachingCentre);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

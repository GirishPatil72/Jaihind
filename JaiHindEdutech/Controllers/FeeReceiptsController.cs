using JaiHindEdutech.Entity;
using JaiHindEdutech.Infrastructure;
using JaiHindEdutech.Models;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.IO;

namespace JaiHindEdutech.Controllers
{
    public class FeeReceiptsController : BaseController
    {
        private JaiHindEduEntitiesNew db = new JaiHindEduEntitiesNew();

        private string CurrentUserName
        {
            get { return User != null && User.Identity != null && User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty; }
        }

        // GET: FeeReceipts
        [PermissionAuthorize(Right = "View")]
        public ActionResult Index()
        {
            return View(db.FeeReceipts.ToList());
        }

        // GET: FeeReceipts/Details/5

        [PermissionAuthorize(Right = "View")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeReceipt feeReceipt = db.FeeReceipts.Find(id);
            if (feeReceipt == null)
            {
                return HttpNotFound();
            }
            return View(feeReceipt);
        }

        // GET: FeeReceipts/Create
        [PermissionAuthorize(Right = "Create")]
        public ActionResult Create(int AdmissionFormId)
        {
            AdmissionBL admissionBL = new AdmissionBL();
            var result = admissionBL.GetAdmissionDataById(AdmissionFormId);
            FeeReceiptModel model = new FeeReceiptModel();
            model.FullName = result.FullName;
            model.CollegeName = result.CollegeName;
            model.ClassName = result.ClassName;
            model.Stream = result.Stream;
            model.FeeReceipts = db.FeeReceipts.Where(x => x.AdmissionFormId == AdmissionFormId).ToList();
            return View(model);
        }

        // POST: FeeReceipts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FeeReceiptModel feeReceipt)
        {
            AdmissionBL adminModel = new AdmissionBL();
            if (ModelState.IsValid)
            {
                feeReceipt.CreatedBy = CurrentUserName;
                feeReceipt.CreatedOn = DateTime.Now;
                feeReceipt.ModifiedBy = CurrentUserName;
                feeReceipt.ModifiedOn = DateTime.Now;

                adminModel.SaveFeeReceipt(feeReceipt);
                return RedirectToAction("GetAllStudents", "Admission");
            }
            feeReceipt.FeeReceipts = db.FeeReceipts.Where(x => x.AdmissionFormId == feeReceipt.AdmissionFormId).ToList();
            return View(feeReceipt);
        }

        // GET: FeeReceipts/Edit/5
        [PermissionAuthorize(Right = "Edit")]
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdmissionBL adminModel = new AdmissionBL();
            var feeReceipt = adminModel.GetFeeReceipt(id);
            if (feeReceipt == null)
            {
                return HttpNotFound();
            }
            return View(feeReceipt);
        }

        // POST: FeeReceipts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FeeReceipt feeReceipt)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(feeReceipt).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feeReceipt);
        }

        // GET: FeeReceipts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeReceipt feeReceipt = db.FeeReceipts.Find(id);
            if (feeReceipt == null)
            {
                return HttpNotFound();
            }
            return View(feeReceipt);
        }

        // POST: FeeReceipts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Right = "Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            FeeReceipt feeReceipt = db.FeeReceipts.Find(id);
            db.FeeReceipts.Remove(feeReceipt);
            db.SaveChanges();
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

        public ActionResult FeeReceiptPDF(int AdmissionFormId)
        {
            try
            {
                AdmissionBL admissionBL = new AdmissionBL();
                var result = admissionBL.GetAdmissionDataById(AdmissionFormId);

                FeeReceiptModel model = new FeeReceiptModel
                {
                    FullName = result.FullName,
                    CollegeName = result.CollegeName,
                    ClassName = result.ClassName,
                    Stream = result.Stream,
                    CollegeType = result.CollegeType,
                    CollegeLogo = result.CollegeLogo,
                    CollegeAddress1 = result.CollegeAddress1,
                    CollegeAddress2 = result.CollegeAddress2,
                    CollegeCity = result.CollegeCity,
                    CollegeContact = result.CollegeContact,
                    FeeReceipts = db.FeeReceipts.Where(x => x.AdmissionFormId == AdmissionFormId).ToList()
                };

                ActivityLogHelper.Log(
                    "Viewed",
                    "FeeReceipt",
                    AdmissionFormId.ToString(),
                    "Opened fee receipt for print",
                    controllerName: "FeeReceipts",
                    actionName: "FeeReceiptPDF",
                    userName: CurrentUserName,
                    roleName: User != null && User.IsInRole("Admin") ? "Admin" : User != null && User.IsInRole("Staff") ? "Staff" : "User");

                return View("FeeReceiptPDF", model);
            }
            catch
            {
                throw;
            }
        }


        public ActionResult AllFeeRecords()
        {
            AdmissionBL model = new AdmissionBL();
            return View(model.GetAllFeeData());
        }

        private string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, new ViewDataDictionary(model), TempData, sw);

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                return sw.ToString();
            }
        }
    }
}

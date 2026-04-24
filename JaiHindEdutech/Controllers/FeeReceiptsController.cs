using DocumentFormat.OpenXml.EMMA;
using JaiHindEdutech.Entity;
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
    public class FeeReceiptsController : Controller
    {
        private JaiHindEduEntitiesNew db = new JaiHindEduEntitiesNew();

        // GET: FeeReceipts
        public ActionResult Index()
        {
            return View(db.FeeReceipts.ToList());
        }

        // GET: FeeReceipts/Details/5
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FeeReceiptModel feeReceipt)
        {
            AdmissionBL adminModel = new AdmissionBL();
            if (ModelState.IsValid)
            {
                feeReceipt.CreatedBy = Session["UserName"].ToString();
                feeReceipt.CreatedOn = DateTime.Now;
                feeReceipt.ModifiedBy = Session["UserName"].ToString();
                feeReceipt.ModifiedOn = DateTime.Now;

                adminModel.SaveFeeReceipt(feeReceipt);
                return RedirectToAction("GetAllStudents", "Admission");
            }
            feeReceipt.FeeReceipts = db.FeeReceipts.Where(x => x.AdmissionFormId == feeReceipt.AdmissionFormId).ToList();
            return View(feeReceipt);
        }

        // GET: FeeReceipts/Edit/5
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
                FeeReceiptModel model = new FeeReceiptModel();
                model.FullName = result.FullName;
                model.CollegeName = result.CollegeName;
                model.ClassName = result.ClassName;
                model.Stream = result.Stream;
                model.CollegeType = result.CollegeType;
                model.CollegeLogo = result.CollegeLogo;
                model.FeeReceipts = db.FeeReceipts.Where(x => x.AdmissionFormId == AdmissionFormId).ToList();
                //return View(model);

                return new Rotativa.ViewAsPdf("FeeReceiptPDF", model)
                {
                    FileName = "FeeReceipt.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = new Rotativa.Options.Margins(20, 10, 10, 10)
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public ActionResult AllFeeRecords()
        {
            AdmissionBL model = new AdmissionBL();
            return View(model.GetAllFeeData());
        }
    }
}

using JaiHindEdutech.Entity;
using JaiHindEdutech.Infrastructure;
using JaiHindEdutech.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace JaiHindEdutech.Controllers
{
    public class AdmissionController : BaseController
    {
        private string CurrentUserName
        {
            get { return User != null && User.Identity != null && User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty; }
        }

        private string CurrentRoleName
        {
            get
            {
                if (User != null && User.IsInRole("Admin"))
                    return "Admin";
                if (User != null && User.IsInRole("Staff"))
                    return "Staff";
                return "User";
            }
        }

        // GET: Admission
        [PermissionAuthorize(Right = "Create,Edit")]
        public ActionResult Index(int collegeId, string collegeName, int studentInfoId = 0)
        {
            ViewBag.collegeId = collegeId;
            ViewBag.collegeName = collegeName;
            StudentInfoModel studentInfoModel = new StudentInfoModel();
            StudentInfoBL studentInfoBL = new StudentInfoBL();
            ViewBag.CoachingCentres = new SelectList(studentInfoBL.GetCoachingCentres(), "CoachingCentreId", "CoachingCentreName");
            studentInfoModel = studentInfoBL.GetStudentInfoById(studentInfoId);
            return View(studentInfoModel);
        }
        public ActionResult Form(int collegeId, string collegeName)
        {
            ViewBag.collegeId = collegeId;
            ViewBag.collegeName = collegeName;
            return View();
        }

        [PermissionAuthorize(Right = "View")]
        public ActionResult GetAllStudents(int? academicYearId)
        {
            AdmissionBL admission = new AdmissionBL();

            using (var db = new JaiHindEduEntitiesNew())
            {
                var academicYears = db.LutAcademicYears
                    .OrderByDescending(x => x.IsCurrent)
                    .ThenByDescending(x => x.Year)
                    .Select(x => new SelectListItem
                    {
                        Value = x.AcademicYearId.ToString(),
                        Text = x.Year + (x.IsCurrent == true ? " (Current)" : "")
                    })
                    .ToList();

                var currentYear = db.LutAcademicYears.FirstOrDefault(x => x.IsCurrent == true);
                int selectedAcademicYearId = academicYearId ?? (currentYear != null ? currentYear.AcademicYearId : 0);

                ViewBag.AcademicYears = new SelectList(academicYears, "Value", "Text", selectedAcademicYearId);
                ViewBag.SelectedAcademicYearId = selectedAcademicYearId;
                ViewBag.College = new SelectList(admission.GetAllColleges(), "CollegeInfoId", "CollegeName");

                string selectedYearName = db.LutAcademicYears
                    .Where(x => x.AcademicYearId == selectedAcademicYearId)
                    .Select(x => x.Year)
                    .FirstOrDefault();

                return View(admission.GetAllStudents(0, selectedYearName));
            }
        }

        [PermissionAuthorize(Right = "View")]
        public ActionResult StudentInfo(int studentInfoId)
        {
            StudentInfoModel studentInfoModel = new StudentInfoModel();
            StudentInfoBL studentInfoBL = new StudentInfoBL();
            studentInfoModel = studentInfoBL.GetStudentInfoById(studentInfoId);
            return PartialView(studentInfoModel);
        }
        [HttpPost]
        public ActionResult StudentInfoPost(StudentInfoModel studentInfoModel)
        {
            StudentInfoBL studentInfoBL = new StudentInfoBL();

            ModelState.Remove("CoachingCentreId");
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    Debug.WriteLine($"Field: {key}, Error: {error.ErrorMessage}");
                }
            }

            if (ModelState.IsValid)
            {
                studentInfoModel.CreatedBy = CurrentUserName;
                studentInfoModel.CreatedOn = DateTime.Now;
                studentInfoModel.ModifiedBy = CurrentUserName;
                studentInfoModel.ModifiedOn = DateTime.Now;

                int studentInfoId = studentInfoBL.CreateStudentInfo(studentInfoModel);
                bool result = false;
                if (studentInfoId > 0)
                {
                    studentInfoModel.StudentInfoId = studentInfoId;
                    studentInfoModel.AddmissionFormId = studentInfoBL.CreateAdmissionInfo(studentInfoModel);
                    result = true;
                }
                return Json(new { result = result, StudentInfoId = studentInfoId });
            }
            return PartialView("StudentInfo", studentInfoModel);
        }

        public ActionResult ParentInfo(int studentInfoId)
        {
            StudentInfoBL studentInfoBL = new StudentInfoBL();
            ParentInfoFormModel parentInfoModel = new ParentInfoFormModel();
            parentInfoModel = studentInfoBL.GetParentInfoByStudentInfoId(studentInfoId);
            return View(parentInfoModel);
        }

        [HttpPost]
        public ActionResult ParentInfoPost(ParentInfoFormModel parentInfoFormModel)
        {
            StudentInfoBL studentInfoBL = new StudentInfoBL();
            if (ModelState.IsValid)
            {
                foreach (var parent in parentInfoFormModel.Parents)
                {
                    parent.CreatedBy = CurrentUserName;
                    parent.CreatedOn = DateTime.Now;
                    parent.ModifiedBy = CurrentUserName;
                    parent.ModifiedOn = DateTime.Now;
                    parent.StudentInfoId = parentInfoFormModel.StudentInfoId;
                    studentInfoBL.CreateParentInfo(parent);
                }
                string RollNumber = studentInfoBL.SubmitAndGenerateRollNo(parentInfoFormModel.StudentInfoId);
                return Json(new { result = true, StudentInfoId = parentInfoFormModel.StudentInfoId, RollNumber = RollNumber });
            }
            return PartialView("ParentInfo", parentInfoFormModel);
        }


        public ActionResult StepPartial(int step, int studentInfoId)
        {
            StudentInfoBL studentInfoBL = new StudentInfoBL();
            switch (step)
            {
                case 1:
                    StudentInfoModel studentInfoModel = new StudentInfoModel();
                    studentInfoModel = studentInfoBL.GetStudentInfoById(studentInfoId);
                    return PartialView("StudentInfo", studentInfoModel);
                case 2:
                    ParentInfoFormModel parentmodel = new ParentInfoFormModel();
                    parentmodel = studentInfoBL.GetParentInfoByStudentInfoId(studentInfoId);
                    return PartialView("ParentInfo", parentmodel);
                case 3:
                    AddressInfoFormModel addressInfoFormModel = new AddressInfoFormModel();
                    addressInfoFormModel = studentInfoBL.GetAddressInfoByStudentInfoId(studentInfoId);
                    return PartialView("Address", addressInfoFormModel);
                case 4:
                    List<UploadedImagesModel> uploadedImagesModels = studentInfoBL.GetUploadedImages(studentInfoId);
                    foreach (var image in uploadedImagesModels)
                    {
                        if (image.DocumentType == "StudentPhoto")
                            ViewBag.StudentPhoto = image.DocumentURL;

                        else if (image.DocumentType == "StudentSignature")
                            ViewBag.StudentSignature = image.DocumentURL;

                        else if (image.DocumentType == "FathersPhoto")
                            ViewBag.FathersPhoto = image.DocumentURL;

                        else if (image.DocumentType == "FathersSignature")
                            ViewBag.FathersSignature = image.DocumentURL;

                        else if (image.DocumentType == "MothersPhoto")
                            ViewBag.MothersPhoto = image.DocumentURL;

                        else if (image.DocumentType == "MothersSignature")
                            ViewBag.MothersSignature = image.DocumentURL;

                    }
                    return PartialView("UploadStudentAndParentImages", studentInfoId);
                case 5:
                    StudentAcademicViewModel studentAcademicViewModel = new StudentAcademicViewModel();
                    studentAcademicViewModel.SelectedClass = studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).ClassName;
                    studentAcademicViewModel.StudentInfoId = studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).StudentInfoId;
                    studentAcademicViewModel.TenthClass = studentInfoBL.GetAcademicStudentInfoId(studentInfoId, "Class 10th");
                    studentAcademicViewModel.EleventhClass = studentInfoBL.GetAcademicStudentInfoId(studentInfoId, "Class 11th");
                    return PartialView("PreviousAcademicInfo", studentAcademicViewModel);
                case 6:
                    SubjectsAndDocumentsSelectionModel subjectSelectionModel = new SubjectsAndDocumentsSelectionModel();
                    subjectSelectionModel.StudentInfoId = studentInfoId;
                    subjectSelectionModel.subjectSelectionModel = studentInfoBL.GetSelectedSubject(studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).AddmissionFormId);
                    subjectSelectionModel.documentSelectionModel = studentInfoBL.GetSelectedDocument(studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).AddmissionFormId);
                    return PartialView("SubjectAndDocumentSelection", subjectSelectionModel);
                //case 7:
                //    return PartialView("DocumentSelection", studentInfoId);
                case 7:

                    AdmissionConfirmationModel admissionConfirmationModel = new AdmissionConfirmationModel();
                    admissionConfirmationModel.StudentInfoId = studentInfoId;
                    admissionConfirmationModel.studentInfoModel = studentInfoBL.GetStudentInfoById(studentInfoId);
                    admissionConfirmationModel.ParentInfoFormModel = studentInfoBL.GetParentInfoByStudentInfoId(studentInfoId);
                    admissionConfirmationModel.addressInfoFormModel = studentInfoBL.GetAddressInfoByStudentInfoId(studentInfoId);

                    StudentAcademicViewModel studentAcademics = new StudentAcademicViewModel();
                    studentAcademics.SelectedClass = studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).ClassName;
                    studentAcademics.StudentInfoId = studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).StudentInfoId;
                    studentAcademics.TenthClass = studentInfoBL.GetAcademicStudentInfoId(studentInfoId, "Class 10th");
                    studentAcademics.EleventhClass = studentInfoBL.GetAcademicStudentInfoId(studentInfoId, "Class 11th");
                    admissionConfirmationModel.previousAcademicInfoModel = studentAcademics;

                    admissionConfirmationModel.subjectSelectionModel = studentInfoBL.GetSelectedSubject(studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).AddmissionFormId);
                    admissionConfirmationModel.documentSelectionModel = studentInfoBL.GetSelectedDocument(studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).AddmissionFormId);

                    admissionConfirmationModel.uploadedImagesModel = studentInfoBL.GetUploadedImages(studentInfoId);



                    return PartialView("Confirmation", admissionConfirmationModel);
                default: return HttpNotFound();
            }
        }

        public ActionResult RegistrationSuccess(int studentInfoId, string RollNumber)
        {
            ViewBag.RollNumber = RollNumber;
            return View();
        }

        public ActionResult GenerateFormPDF(int studentInfoId)
        {
            try
            {
                StudentInfoBL studentInfoBL = new StudentInfoBL();
                AdmissionConfirmationModel admissionConfirmationModel = new AdmissionConfirmationModel
                {
                    StudentInfoId = studentInfoId,
                    studentInfoModel = studentInfoBL.GetStudentInfoById(studentInfoId),
                    ParentInfoFormModel = studentInfoBL.GetParentInfoByStudentInfoId(studentInfoId),
                    addressInfoFormModel = studentInfoBL.GetAddressInfoByStudentInfoId(studentInfoId)
                };

                StudentAcademicViewModel studentAcademics = new StudentAcademicViewModel();
                studentAcademics.SelectedClass = studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).ClassName;
                studentAcademics.StudentInfoId = studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).StudentInfoId;
                studentAcademics.TenthClass = studentInfoBL.GetAcademicStudentInfoId(studentInfoId, "Class 10th");
                studentAcademics.EleventhClass = studentInfoBL.GetAcademicStudentInfoId(studentInfoId, "Class 11th");
                admissionConfirmationModel.previousAcademicInfoModel = studentAcademics;

                admissionConfirmationModel.subjectSelectionModel =
                    studentInfoBL.GetSelectedSubject(studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).AddmissionFormId);

                admissionConfirmationModel.documentSelectionModel =
                    studentInfoBL.GetSelectedDocument(studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).AddmissionFormId);

                admissionConfirmationModel.uploadedImagesModel = studentInfoBL.GetUploadedImages(studentInfoId);
                admissionConfirmationModel.collegeInfoModel = studentInfoBL.GetColelgeInfo(studentInfoId);


                ActivityLogHelper.Log(
                    "Viewed",
                    "AdmissionForm",
                    studentInfoId.ToString(),
                    "Opened admission form for print",
                    controllerName: "Admission",
                    actionName: "GenerateFormPDF",
                    userName: CurrentUserName,
                    roleName: CurrentRoleName);

                return View("GenerateFormPDF", admissionConfirmationModel);
            }
            catch
            {
                throw;
            }
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

        public ActionResult AllAddmissionExportToExcel(int CollegeInfoId, int? academicYearId)
        {
            StudentInfoBL studentInfoBL = new StudentInfoBL();

            using (var db = new JaiHindEduEntitiesNew())
            {
                var currentYear = db.LutAcademicYears.FirstOrDefault(x => x.IsCurrent == true);
                int selectedAcademicYearId = academicYearId ?? (currentYear != null ? currentYear.AcademicYearId : 0);

                string selectedYearName = db.LutAcademicYears
                    .Where(x => x.AcademicYearId == selectedAcademicYearId)
                    .Select(x => x.Year)
                    .FirstOrDefault();

                var data = studentInfoBL.GetAllAdmissionData(CollegeInfoId, selectedYearName);

                ActivityLogHelper.Log(
                    "Downloaded",
                    "AdmissionReport",
                    CollegeInfoId.ToString(),
                    "Exported admission data to Excel",
                    controllerName: "Admission",
                    actionName: "AllAddmissionExportToExcel",
                    userName: CurrentUserName,
                    roleName: CurrentRoleName);

                return ExportDataToExcel(data);
            }
        }
        private ActionResult ExportDataToExcel<T>(List<T> data)
        {
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("AdmissionReport");
                var currentRow = 1;

                var properties = typeof(T).GetProperties();

                // Header
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cell(currentRow, i + 1).Value = properties[i].Name;
                }

                // Body
                foreach (var item in data)
                {
                    currentRow++;
                    for (int i = 0; i < properties.Length; i++)
                    {
                        var value = properties[i].GetValue(item, null);
                        worksheet.Cell(currentRow, i + 1).Value = value == null ? string.Empty : value.ToString();
                    }
                }

                // Stream and return as file
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "AdmissionReport.xlsx");
                }
            }
        }


        public ActionResult FeeReceipt(int AdmissionFormId)
        {
            AdmissionBL admissionBL = new AdmissionBL();
            var result = admissionBL.GetAdmissionDataById(AdmissionFormId);
            FeeReceiptModel model = new FeeReceiptModel();
            model.FullName = result.FullName;
            model.CollegeName = result.CollegeName;
            model.ClassName = result.ClassName;
            model.Stream= result.Stream;
            return View(model);
        }

        //[HttpPost]
        //public ActionResult FeeReceiptPost(FeeReceiptModel feeReceipt)
        //{
        //    AdmissionBL admissionBL= new AdmissionBL();

        //    if (ModelState.IsValid)
        //    {
        //        feeReceipt.CreatedBy = Session["UserName"].ToString();
        //        feeReceipt.CreatedOn = DateTime.Now;
        //        feeReceipt.ModifiedBy = Session["UserName"].ToString();
        //        feeReceipt.ModifiedOn = DateTime.Now;
        //        int feeReceiptId = admissionBL.SaveFeeReceipt(feeReceipt);
        //        bool result = false;
        //        if (feeReceiptId > 0)
        //        {
        //        }
        //        return Json(new { result = result, StudentInfoId = studentInfoId });
        //    }
        //    return PartialView("StudentInfo", studentInfoModel);
        //}
    }
}
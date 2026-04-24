using JaiHindEdutech.Entity;
using JaiHindEdutech.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JaiHindEdutech.Controllers
{
    public class AdmissionController : BaseController
    {
        // GET: Admission
        public ActionResult Index(int collegeId, string collegeName, int studentInfoId = 0)
        {
            bool aaaa = Utility.CheckLoggedInUserRoleByRoleName("", null);
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

        public ActionResult GetAllStudents()
        {
            AdmissionBL admission = new AdmissionBL();
            ViewBag.College = new SelectList(admission.GetAllColleges(), "CollegeInfoId", "CollegeName");
            return View(admission.GetAllStudents());
        }

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
                studentInfoModel.CreatedBy = Session["UserName"].ToString();
                studentInfoModel.CreatedOn = DateTime.Now;
                studentInfoModel.ModifiedBy = Session["UserName"].ToString();
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
                    parent.CreatedBy = Session["UserName"].ToString();
                    parent.CreatedOn = DateTime.Now;
                    parent.ModifiedBy = Session["UserName"].ToString();
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
                admissionConfirmationModel.collegeInfoModel = studentInfoBL.GetColelgeInfo(studentInfoId);


                //return PartialView("GenerateFormPDF", admissionConfirmationModel);

                return new Rotativa.ViewAsPdf("GenerateFormPDF", admissionConfirmationModel)
                {
                    FileName = "Student-Form.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10)
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult AllAddmissionExportToExcel(int CollegeInfoId)
        {
            StudentInfoBL studentInfoBL = new StudentInfoBL();
            var data = studentInfoBL.GetAllAdmissionData(CollegeInfoId); // Call your stored procedure here
            return ExportDataToExcel(data);
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
                        worksheet.Cell(currentRow, i + 1).Value = value?.ToString() ?? string.Empty;
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
using JaiHindEdutech.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JaiHindEdutech.Controllers
{
    public class StudentInfoesController : BaseController
    {
        // GET: StudentInfoes
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
            foreach (var entry in ModelState)
            {
                if (entry.Value.Errors.Count > 0)
                {
                    var fieldName = entry.Key;
                    foreach (var error in entry.Value.Errors)
                    {
                        // Log or debug the field and error message
                        //System.Diagnostics.Debug.WriteLine($"Field: {fieldName}, Error: {error.ErrorMessage}");
                    }
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
                return Json(new { result = true, StudentInfoId = parentInfoFormModel.StudentInfoId });
            }
            return PartialView("ParentInfo", parentInfoFormModel);
        }
        public ActionResult Address(int studentInfoId)
        {
            StudentInfoBL studentInfoBL = new StudentInfoBL();
            AddressInfoFormModel addressInfoFormModel = new AddressInfoFormModel();
            addressInfoFormModel = studentInfoBL.GetAddressInfoByStudentInfoId(studentInfoId);
            return PartialView(addressInfoFormModel);
        }

        [HttpPost]
        public ActionResult AddressInfoPost(AddressInfoFormModel addressInfoFormModel)
        {
            StudentInfoBL studentInfoBL = new StudentInfoBL();
            if (ModelState.IsValid)
            {
                foreach (var address in addressInfoFormModel.Address)
                {
                    address.CreatedBy = Session["UserName"].ToString();
                    address.CreatedOn = DateTime.Now;
                    address.ModifiedBy = Session["UserName"].ToString();
                    address.ModifiedOn = DateTime.Now;
                    address.StudentInfoId = addressInfoFormModel.StudentInfoId;
                    studentInfoBL.CreateAddressInfo(address);
                }
                return Json(new { result = true, StudentInfoId = addressInfoFormModel.StudentInfoId });
            }
            return PartialView("Address", addressInfoFormModel);
        }
        public ActionResult UploadStudentAndParentImages(int studentInfoId)
        {
            StudentInfoBL studentInfoBL = new StudentInfoBL();
            List<UploadedImagesModel> uploadedImagesModels = studentInfoBL.GetUploadedImages(studentInfoId);
            foreach(var image in uploadedImagesModels)
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
            return PartialView();
        }
        [HttpPost]
        public ActionResult UploadImages(int studentInfoId, HttpPostedFileBase StudentPhoto, HttpPostedFileBase StudentSignature,
                                 HttpPostedFileBase FatherPhoto, HttpPostedFileBase FatherSignature, HttpPostedFileBase MotherPhoto, HttpPostedFileBase MotherSignature)
        {
            StudentInfoBL studentInfoBL = new StudentInfoBL();
            if (ModelState.IsValid)
            {
                var files = new List<StudentUploadModel>
                {
                new StudentUploadModel { File = StudentPhoto, Name =DateTime.Now.ToString("yyyyMMdd_HHmmss"),FileOwner= "StudentPhoto" },
                new StudentUploadModel { File = StudentSignature, Name =DateTime.Now.ToString("yyyyMMdd_HHmmss"),FileOwner=  "StudentSignature" },
                new StudentUploadModel { File = FatherPhoto, Name =DateTime.Now.ToString("yyyyMMdd_HHmmss"),FileOwner=  "FatherPhoto" },
                new StudentUploadModel { File = FatherSignature, Name =DateTime.Now.ToString("yyyyMMdd_HHmmss"),FileOwner=  "FatherSignature" },
                new StudentUploadModel { File = MotherPhoto, Name =DateTime.Now.ToString("yyyyMMdd_HHmmss"),FileOwner=  "MotherPhoto" },
                new StudentUploadModel { File = MotherSignature, Name =DateTime.Now.ToString("yyyyMMdd_HHmmss"),FileOwner=  "MotherSignature" }
                };

                foreach (var fileItem in files)
                {
                    if (fileItem.File != null && fileItem.File.ContentLength > 0)
                    {
                        string folderPath = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        string fileName = $"{fileItem.Name}_{Path.GetFileName(fileItem.File.FileName)}";
                        string fullPath = Path.Combine(folderPath, fileName);

                        fileItem.File.SaveAs(fullPath);

                        string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                        baseUrl = baseUrl + "/Uploads/" + fileName;
                        int DocumentId = studentInfoBL.SaveUploadedDocument(studentInfoId, fileItem.FileOwner, baseUrl, fileName, fileItem.File.ContentLength, Session["UserName"].ToString());
                    }
                }
                return Json(new { result = true, StudentInfoId = studentInfoId });
            }
            else
            {
                return PartialView("UploadStudentAndParentImages", studentInfoId);
            }
        }

        public ActionResult PreviousAcademicInfo(int studentInfoId)
        {
            StudentAcademicViewModel studentAcademicViewModel = new StudentAcademicViewModel();
            StudentInfoBL studentInfoBL = new StudentInfoBL();
            studentAcademicViewModel.SelectedClass = studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).ClassName;
            studentAcademicViewModel.StudentInfoId = studentInfoBL.GetClassInfoByStudentInfoId(studentInfoId).StudentInfoId;
            return PartialView(studentAcademicViewModel);
        }

        [HttpPost]
        public ActionResult PreviousAcademicInfoPost(StudentAcademicViewModel academicViewModel)
        {
            StudentInfoBL studentInfoBL = new StudentInfoBL();

            if (academicViewModel.SelectedClass == "11th")
            {
                var keysToClear = ModelState.Keys.Where(k => k.StartsWith("EleventhClass.")).ToList();

                foreach (var key in keysToClear)
                {
                    ModelState[key].Errors.Clear();
                }
            }

            if (ModelState.IsValid)
            {
                academicViewModel.CreatedBy = Session["UserName"].ToString();
                academicViewModel.CreatedOn = DateTime.Now;
                academicViewModel.ModifiedBy = Session["UserName"].ToString();
                academicViewModel.ModifiedOn = DateTime.Now;
                academicViewModel.StudentInfoId = academicViewModel.StudentInfoId;
                studentInfoBL.CreatePreviousAcademicInfo(academicViewModel);
                return Json(new { result = true, StudentInfoId = academicViewModel.StudentInfoId });
            }
            return PartialView("PreviousAcademicInfo", academicViewModel);
        }

        public ActionResult SubjectAndDocumentSelection(int studentInfoId)
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult SubjectAndDocumentSelectionPost(SubjectsAndDocumentsSelectionModel subjectsAndDocumentsSelectionModel)
        {
            StudentInfoBL studentInfoBL = new StudentInfoBL();
            if (ModelState.IsValid)
            {
                subjectsAndDocumentsSelectionModel.subjectSelectionModel.AddmissionFormId = studentInfoBL.GetClassInfoByStudentInfoId(subjectsAndDocumentsSelectionModel.StudentInfoId).AddmissionFormId;
                subjectsAndDocumentsSelectionModel.subjectSelectionModel.CreatedBy = Session["UserName"].ToString();
                studentInfoBL.SaveSelectedSubjectsInfo(subjectsAndDocumentsSelectionModel.subjectSelectionModel);

                subjectsAndDocumentsSelectionModel.documentSelectionModel.AddmissionFormId = studentInfoBL.GetClassInfoByStudentInfoId(subjectsAndDocumentsSelectionModel.StudentInfoId).AddmissionFormId;
                subjectsAndDocumentsSelectionModel.documentSelectionModel.CreatedBy = Session["UserName"].ToString();
                studentInfoBL.SaveSelectedDocumentInfo(subjectsAndDocumentsSelectionModel.documentSelectionModel);

                return Json(new { result = true, StudentInfoId = subjectsAndDocumentsSelectionModel.StudentInfoId });
            }
            return PartialView("SubjectSelection", subjectsAndDocumentsSelectionModel);
        }
        public ActionResult DocumentSelection(int studentInfoId)
        {
            //AdmissionConfirmationModel 

            StudentInfoBL studentInfoBL = new StudentInfoBL();
            AdmissionConfirmationModel admissionConfirmationModel = new AdmissionConfirmationModel();
            admissionConfirmationModel.StudentInfoId = studentInfoId;
            admissionConfirmationModel.studentInfoModel = studentInfoBL.GetStudentInfoById(studentInfoId);
            admissionConfirmationModel.ParentInfoFormModel = studentInfoBL.GetParentInfoByStudentInfoId(studentInfoId);
            admissionConfirmationModel.addressInfoFormModel = studentInfoBL.GetAddressInfoByStudentInfoId(studentInfoId);
            admissionConfirmationModel.previousAcademicInfoModel.TenthClass = studentInfoBL.GetAcademicStudentInfoId(studentInfoId, "Class 10th");
            admissionConfirmationModel.previousAcademicInfoModel.EleventhClass = studentInfoBL.GetAcademicStudentInfoId(studentInfoId, "Class 11th");
            admissionConfirmationModel.subjectSelectionModel = studentInfoBL.GetSelectedSubject(studentInfoId);
            admissionConfirmationModel.documentSelectionModel = studentInfoBL.GetSelectedDocument(studentInfoId);
            return PartialView(admissionConfirmationModel);
        }
        [HttpPost]
        public ActionResult DocumentSelectionPost(int studentInfoId)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = true, StudentInfoId = studentInfoId });
            }
            return PartialView("DocumentSelection", studentInfoId);
        }
        public ActionResult Confirmation(int studentInfoId)
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ConfirmationPost(int studentInfoId)
        {
            StudentInfoBL studentInfoBL = new StudentInfoBL();
            if (ModelState.IsValid)
            {
                string RollNo=studentInfoBL.SubmitAndGenerateRollNo(studentInfoId);
                return Json(new { result = true, StudentInfoId = studentInfoId, RollNumber= RollNo });
            }
            return PartialView("Confirmation", studentInfoId);
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
    }
}
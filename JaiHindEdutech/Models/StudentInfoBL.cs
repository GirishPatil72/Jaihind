using DocumentFormat.OpenXml.Spreadsheet;
using JaiHindEdutech.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace JaiHindEdutech.Models
{
    public class StudentInfoBL
    {

        public List<StudentInfoModel> GetStudentsInfo()
        {
            try
            {
                List<StudentInfoModel> studentInfoList = new List<StudentInfoModel>();
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var users = db.StudentInfoes.Where(s => s.IsDeleted == false).ToList();

                    foreach (var user in users)
                    {
                        var studentInfoModel = new StudentInfoModel
                        {
                            StudentInfoId = user.StudentInfoId,
                            FirstName = user.FirstName,
                            MiddleName = user.MiddleName,
                            LastName = user.LastName,
                            FullName = user.FullName,
                            DateOfBirth = user.DateOfBirth,
                            Gender = user.Gender,
                            AadharNo = user.AadharNo,
                            IsSingleGirlChild = user.IsSingleGirlChild ?? false,
                            BloodGroup = user.BloodGroup,
                            Nationality = user.Nationality,
                            Category = user.Category,
                            MobileNo = user.MobileNo,
                            EmailId = user.EmailId,
                            Domicile = user.Domicile,
                            IsActive = user.IsActive,
                            IsDeleted = user.IsDeleted,
                            CreatedBy = user.CreatedBy,
                            CreatedOn = user.CreatedOn,
                            ModifiedBy = user.ModifiedBy,
                            ModifiedOn = user.ModifiedOn
                        };

                        studentInfoList.Add(studentInfoModel);
                    }

                    return studentInfoList;
                }
            }
            catch
            {
                throw;
            }
        }
        public StudentInfoModel GetStudentInfoById(int StudentInfoId)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var user = db.StudentInfoes.Where(s => s.StudentInfoId == StudentInfoId && s.IsDeleted == false).FirstOrDefault();
                    if (user != null)
                    {
                        var AdmissionInfo = db.AddmissionForms.Where(a => a.StudentInfoId == user.StudentInfoId && a.IsDeleted == false).FirstOrDefault();
                        var studentInfoModel = new StudentInfoModel
                        {
                            StudentInfoId = user.StudentInfoId,
                            FirstName = user.FirstName,
                            MiddleName = user.MiddleName,
                            LastName = user.LastName,
                            FullName = user.FullName,
                            DateOfBirth = user.DateOfBirth,
                            Gender = user.Gender,
                            AadharNo = user.AadharNo,
                            IsSingleGirlChild = user.IsSingleGirlChild ?? false,
                            BloodGroup = user.BloodGroup,
                            Nationality = user.Nationality,
                            Category = user.Category,
                            MobileNo = user.MobileNo,
                            EmailId = user.EmailId,
                            Domicile = user.Domicile,
                            IsActive = user.IsActive,
                            IsDeleted = user.IsDeleted,
                            CreatedBy = user.CreatedBy,
                            CreatedOn = user.CreatedOn,
                            ModifiedBy = user.ModifiedBy,
                            ModifiedOn = user.ModifiedOn,
                            AddmissionFormId = AdmissionInfo.AddmissionFormId,
                            ClassName = AdmissionInfo.ClassName,
                            Stream = AdmissionInfo.Stream,
                            CoachingCentreId = AdmissionInfo.CoachingCentreId ?? 0,
                            CollegeInfoId = AdmissionInfo.CollegeInfoId ?? 0,
                            AdmissionFormNo = AdmissionInfo.AdmissionFormNo,
                            Remark = AdmissionInfo.Remark,
                            TotalFees = AdmissionInfo.TotalFees,
                            Scholarship = AdmissionInfo.Scholarship,
                            FeesPayable = AdmissionInfo.FeesPayable,
                        };
                        return studentInfoModel;
                    }
                    else
                    {
                        return new StudentInfoModel();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public int CreateStudentInfo(StudentInfoModel user)
        {
            try
            {
                int studentInfoId = 0;

                if (user.IsActive == null)
                    user.IsActive = true;
                if (user.IsDeleted == null)
                    user.IsDeleted = false;

                using (var db = new JaiHindEduEntitiesNew())
                {
                    var userinfo = db.StudentInfoes.Where(s => s.StudentInfoId == user.StudentInfoId && s.IsDeleted == false).FirstOrDefault();
                    if (userinfo != null && userinfo.StudentInfoId > 0)
                    {
                        var beforeSnapshot = new
                        {
                            userinfo.StudentInfoId,
                            userinfo.FirstName,
                            userinfo.MiddleName,
                            userinfo.LastName,
                            userinfo.FullName,
                            userinfo.DateOfBirth,
                            userinfo.Gender,
                            userinfo.AadharNo,
                            userinfo.MobileNo,
                            userinfo.EmailId,
                            userinfo.Domicile,
                            userinfo.IsActive,
                            userinfo.IsDeleted
                        };

                        userinfo.FirstName = user.FirstName;
                        userinfo.MiddleName = user.MiddleName;
                        userinfo.LastName = user.LastName;
                        userinfo.FullName = user.FirstName + " " + user.MiddleName + " " + user.LastName;
                        userinfo.DateOfBirth = user.DateOfBirth;
                        userinfo.Gender = user.Gender;
                        userinfo.AadharNo = user.AadharNo;
                        userinfo.IsSingleGirlChild = user.IsSingleGirlChild;
                        userinfo.BloodGroup = user.BloodGroup;
                        userinfo.Nationality = user.Nationality;
                        userinfo.Category = user.Category;
                        userinfo.MobileNo = user.MobileNo;
                        userinfo.EmailId = user.EmailId;
                        userinfo.Domicile = user.Domicile;
                        userinfo.IsActive = user.IsActive;
                        userinfo.IsDeleted = user.IsDeleted;
                        userinfo.ModifiedBy = user.ModifiedBy;
                        userinfo.ModifiedOn = user.ModifiedOn;

                        db.SaveChanges();
                        studentInfoId = userinfo.StudentInfoId;

                        var afterSnapshot = new
                        {
                            userinfo.StudentInfoId,
                            user.FirstName,
                            user.MiddleName,
                            user.LastName,
                            FullName = user.FirstName + " " + user.MiddleName + " " + user.LastName,
                            user.DateOfBirth,
                            user.Gender,
                            user.AadharNo,
                            user.MobileNo,
                            user.EmailId,
                            user.Domicile,
                            user.IsActive,
                            user.IsDeleted
                        };

                        TrackActivity(
                            "Updated",
                            "StudentInfo",
                            studentInfoId.ToString(),
                            "Student record updated",
                            beforeSnapshot,
                            afterSnapshot);
                    }
                    else
                    {
                        var studentInfoEntity = new StudentInfo
                        {
                            FirstName = user.FirstName,
                            MiddleName = user.MiddleName,
                            LastName = user.LastName,
                            FullName = user.FirstName + " " + user.MiddleName + " " + user.LastName,
                            DateOfBirth = user.DateOfBirth,
                            Gender = user.Gender,
                            AadharNo = user.AadharNo,
                            IsSingleGirlChild = user.IsSingleGirlChild,
                            BloodGroup = user.BloodGroup,
                            Nationality = user.Nationality,
                            Category = user.Category,
                            MobileNo = user.MobileNo,
                            EmailId = user.EmailId,
                            Domicile = user.Domicile,
                            IsActive = user.IsActive,
                            IsDeleted = user.IsDeleted,
                            CreatedBy = user.CreatedBy,
                            CreatedOn = user.CreatedOn,
                            ModifiedBy = user.ModifiedBy,
                            ModifiedOn = user.ModifiedOn
                        };
                        db.StudentInfoes.Add(studentInfoEntity);
                        db.SaveChanges();
                        studentInfoId = studentInfoEntity.StudentInfoId;

                        var afterSnapshot = new
                        {
                            studentInfoEntity.StudentInfoId,
                            user.FirstName,
                            user.MiddleName,
                            user.LastName,
                            FullName = user.FirstName + " " + user.MiddleName + " " + user.LastName,
                            user.DateOfBirth,
                            user.Gender,
                            user.AadharNo,
                            user.MobileNo,
                            user.EmailId,
                            user.Domicile,
                            user.IsActive,
                            user.IsDeleted
                        };

                        TrackActivity(
                            "Created",
                            "StudentInfo",
                            studentInfoId.ToString(),
                            "Student record created",
                            null,
                            afterSnapshot);
                    }

                    return studentInfoId;
                }
            }
            catch
            {
                return 0;
            }
        }

        public int CreateAdmissionInfo(StudentInfoModel user)
        {
            try
            {
                int addmissionFormId = 0;
                using (var db = new JaiHindEduEntitiesNew())
                {
                    int academicYearId = db.LutAcademicYears
                        .Where(a => a.IsCurrent == true)
                        .OrderByDescending(a => a.AcademicYearId)
                        .Select(a => a.AcademicYearId)
                        .FirstOrDefault();

                    var classinfo = db.AddmissionForms.FirstOrDefault(s =>
                            s.StudentInfoId == user.StudentInfoId &&
                            s.IsDeleted == false &&
                            s.AcademicYearId == academicYearId);

                    if (classinfo != null && classinfo.AddmissionFormId > 0)
                    {
                        var beforeSnapshot = new
                        {
                            classinfo.AddmissionFormId,
                            classinfo.StudentInfoId,
                            classinfo.CollegeInfoId,
                            classinfo.ClassName,
                            classinfo.Stream,
                            classinfo.CoachingCentreId,
                            classinfo.AcademicYearId,
                            classinfo.AdmissionFormNo,
                            classinfo.Remark,
                            classinfo.TotalFees,
                            classinfo.Scholarship,
                            classinfo.FeesPayable,
                            classinfo.IsDeleted
                        };

                        classinfo.StudentInfoId = user.StudentInfoId;
                        classinfo.CollegeInfoId = user.CollegeInfoId;
                        classinfo.ClassName = user.ClassName;
                        classinfo.Stream = user.Stream;
                        classinfo.CoachingCentreId = user.CoachingCentreId;
                        classinfo.IsDeleted = false;
                        classinfo.ModifiedBy = user.ModifiedBy;
                        classinfo.ModifiedOn = user.ModifiedOn;
                        classinfo.AdmissionFormNo = user.AdmissionFormNo;
                        classinfo.Remark = user.Remark;
                        classinfo.TotalFees = user.TotalFees;
                        classinfo.Scholarship = user.Scholarship;
                        classinfo.FeesPayable = user.FeesPayable;

                        db.SaveChanges();
                        addmissionFormId = classinfo.AddmissionFormId;

                        var afterSnapshot = new
                        {
                            classinfo.AddmissionFormId,
                            classinfo.StudentInfoId,
                            classinfo.CollegeInfoId,
                            classinfo.ClassName,
                            classinfo.Stream,
                            classinfo.CoachingCentreId,
                            classinfo.AcademicYearId,
                            classinfo.AdmissionFormNo,
                            classinfo.Remark,
                            classinfo.TotalFees,
                            classinfo.Scholarship,
                            classinfo.FeesPayable,
                            classinfo.IsDeleted
                        };

                        TrackActivity(
                            "Updated",
                            "AddmissionForm",
                            addmissionFormId.ToString(),
                            "Admission form updated",
                            beforeSnapshot,
                            afterSnapshot);
                    }
                    else
                    {
                        var admissionInfoModel = new AddmissionForm
                        {
                            StudentInfoId = user.StudentInfoId,
                            CollegeInfoId = user.CollegeInfoId,
                            ClassName = user.ClassName,
                            Stream = user.Stream,
                            CoachingCentreId = user.CoachingCentreId,
                            IsDeleted = user.IsDeleted,
                            CreatedBy = user.CreatedBy,
                            CreatedOn = user.CreatedOn,
                            ModifiedBy = user.ModifiedBy,
                            ModifiedOn = user.ModifiedOn,
                            AcademicYearId = academicYearId,
                            AdmissionFormNo = user.AdmissionFormNo,
                            Remark = user.Remark,
                            TotalFees = user.TotalFees,
                            Scholarship = user.Scholarship,
                            FeesPayable = user.FeesPayable
                        };

                        db.AddmissionForms.Add(admissionInfoModel);
                        db.SaveChanges();
                        addmissionFormId = admissionInfoModel.AddmissionFormId;

                        var afterSnapshot = new
                        {
                            admissionInfoModel.AddmissionFormId,
                            admissionInfoModel.StudentInfoId,
                            admissionInfoModel.CollegeInfoId,
                            admissionInfoModel.ClassName,
                            admissionInfoModel.Stream,
                            admissionInfoModel.CoachingCentreId,
                            admissionInfoModel.AcademicYearId,
                            admissionInfoModel.AdmissionFormNo,
                            admissionInfoModel.Remark,
                            admissionInfoModel.TotalFees,
                            admissionInfoModel.Scholarship,
                            admissionInfoModel.FeesPayable,
                            admissionInfoModel.IsDeleted
                        };

                        TrackActivity(
                            "Created",
                            "AddmissionForm",
                            addmissionFormId.ToString(),
                            "Admission form created",
                            null,
                            afterSnapshot);
                    }

                    return addmissionFormId;
                }
            }
            catch (DbEntityValidationException)
            {
                return 0;
            }
        }

        public ParentInfoFormModel GetParentInfoByStudentInfoId(int StudentInfoId)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var parentinfo = db.AssStudentParentInfoes
                        .Include(x => x.ParentInfo)
                        .Where(x => x.StudentInfoId == StudentInfoId && x.IsDeleted == false).ToList();

                    var parents = parentinfo.Select(x => new ParentInfoModel
                    {
                        ParentType = ParseParentType(x.ParentInfo.ParentType),
                        ParentInfoId = x.ParentInfo.ParentInfoId,
                        FullName = x.ParentInfo.FullName,
                        Occupation = x.ParentInfo.Occupation,
                        AnnualIncome = x.ParentInfo.AnnualIncome,
                        AadharNo = x.ParentInfo.AadharNo,
                        MobileNo = x.ParentInfo.MobileNo,
                        EmailId = x.ParentInfo.EmailId,
                        IsActive = x.ParentInfo.IsActive,
                        IsDeleted = x.ParentInfo.IsDeleted,
                        CreatedBy = x.ParentInfo.CreatedBy,
                        CreatedOn = x.ParentInfo.CreatedOn,
                        ModifiedBy = x.ParentInfo.ModifiedBy,
                        ModifiedOn = x.ParentInfo.ModifiedOn,
                        StudentInfoId = x.StudentInfoId ?? 0
                    }).ToList();

                    var parentInfoFormModel = new ParentInfoFormModel
                    {
                        StudentInfoId = StudentInfoId,
                        Parents = parents.Count() == 0 ? new List<ParentInfoModel>
                                                            {
                                                                new ParentInfoModel { ParentType = ParentType.Father },
                                                                new ParentInfoModel { ParentType = ParentType.Mother }
                                                            } : parents
                    };
                    return parentInfoFormModel;
                }
            }
            catch
            {
                throw;
            }
        }
        private ParentType ParseParentType(string value)
        {
            return Enum.TryParse<ParentType>(value, true, out var result) ? result : ParentType.Father;
        }

        public int CreateParentInfo(ParentInfoModel user)
        {
            try
            {
                int parentInfoId = 0;

                if (user.IsActive == null)
                    user.IsActive = true;
                if (user.IsDeleted == null)
                    user.IsDeleted = false;

                using (var db = new JaiHindEduEntitiesNew())
                {
                    var userinfo = db.ParentInfoes.Where(s => s.ParentInfoId == user.ParentInfoId && s.IsDeleted == false).FirstOrDefault();
                    if (userinfo != null && userinfo.ParentInfoId > 0)
                    {
                        var beforeSnapshot = new
                        {
                            userinfo.ParentInfoId,
                            userinfo.ParentType,
                            userinfo.FullName,
                            userinfo.AadharNo,
                            userinfo.Occupation,
                            userinfo.AnnualIncome,
                            userinfo.EmailId,
                            userinfo.MobileNo,
                            userinfo.IsActive,
                            userinfo.IsDeleted
                        };

                        userinfo.ParentType = user.ParentType.ToString();
                        userinfo.FullName = user.FullName;
                        userinfo.AadharNo = user.AadharNo;
                        userinfo.Occupation = user.Occupation;
                        userinfo.AnnualIncome = user.AnnualIncome;
                        userinfo.EmailId = user.EmailId;
                        userinfo.MobileNo = user.MobileNo;
                        userinfo.IsActive = user.IsActive;
                        userinfo.IsDeleted = user.IsDeleted;
                        userinfo.ModifiedBy = user.ModifiedBy;
                        userinfo.ModifiedOn = user.ModifiedOn;

                        db.SaveChanges();
                        parentInfoId = userinfo.ParentInfoId;

                        var afterSnapshot = new
                        {
                            userinfo.ParentInfoId,
                            ParentType = user.ParentType.ToString(),
                            user.FullName,
                            user.AadharNo,
                            user.Occupation,
                            user.AnnualIncome,
                            user.EmailId,
                            user.MobileNo,
                            user.IsActive,
                            user.IsDeleted
                        };

                        TrackActivity(
                            "Updated",
                            "ParentInfo",
                            parentInfoId.ToString(),
                            "Parent information updated",
                            beforeSnapshot,
                            afterSnapshot);
                    }
                    else
                    {
                        var parentInfoModel = new ParentInfo
                        {
                            ParentType = user.ParentType.ToString(),
                            FullName = user.FullName,
                            AadharNo = user.AadharNo,
                            Occupation = user.Occupation,
                            AnnualIncome = user.AnnualIncome,
                            EmailId = user.EmailId,
                            MobileNo = user.MobileNo,
                            IsActive = user.IsActive,
                            IsDeleted = user.IsDeleted,
                            CreatedBy = user.CreatedBy,
                            CreatedOn = user.CreatedOn,
                            ModifiedBy = user.ModifiedBy,
                            ModifiedOn = user.ModifiedOn
                        };
                        db.ParentInfoes.Add(parentInfoModel);
                        db.SaveChanges();
                        parentInfoId = parentInfoModel.ParentInfoId;

                        var assStudentParent = new AssStudentParentInfo
                        {
                            ParentType = parentInfoModel.ParentType,
                            StudentInfoId = user.StudentInfoId,
                            ParentInfoId = parentInfoModel.ParentInfoId,
                            CreatedBy = parentInfoModel.CreatedBy,
                            CreatedOn = parentInfoModel.CreatedOn,
                            ModifiedBy = parentInfoModel.ModifiedBy,
                            ModifiedOn = parentInfoModel.ModifiedOn,
                            IsDeleted = user.IsDeleted
                        };
                        db.AssStudentParentInfoes.Add(assStudentParent);
                        db.SaveChanges();

                        var afterSnapshot = new
                        {
                            parentInfoModel.ParentInfoId,
                            parentInfoModel.ParentType,
                            parentInfoModel.FullName,
                            parentInfoModel.AadharNo,
                            parentInfoModel.Occupation,
                            parentInfoModel.AnnualIncome,
                            parentInfoModel.EmailId,
                            parentInfoModel.MobileNo,
                            parentInfoModel.IsActive,
                            parentInfoModel.IsDeleted
                        };

                        TrackActivity(
                            "Created",
                            "ParentInfo",
                            parentInfoId.ToString(),
                            "Parent information created",
                            null,
                            afterSnapshot);
                    }

                    return parentInfoId;
                }
            }
            catch
            {
                return 0;
            }
        }

        public AddressInfoFormModel GetAddressInfoByStudentInfoId(int StudentInfoId)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var rawAddressList = db.AssStudentAddressStudentInfoes
                                        .Where(x => x.StudentInfoId == StudentInfoId && x.IsDeleted == false)
                                        .ToList();

                    var addressList = rawAddressList.Select(x => new AddressInfoModel
                    {
                        AddressType = ParseAddressType(x.StudentAddressInfo.AddressType),
                        StudentAddressInfoId = x.StudentAddressInfo.StudentAddressInfoId,
                        HouseNo = x.StudentAddressInfo.HouseNo,
                        Area = x.StudentAddressInfo.Area,
                        Village = x.StudentAddressInfo.Village,
                        PO = x.StudentAddressInfo.PO,
                        District = x.StudentAddressInfo.District,
                        City = x.StudentAddressInfo.City,
                        State = x.StudentAddressInfo.State,
                        Zip = x.StudentAddressInfo.Zip,
                        SameAsPermanent = x.StudentAddressInfo.SameAsPermanent ?? false,
                        IsDeleted = x.StudentAddressInfo.IsDeleted,
                        CreatedBy = x.StudentAddressInfo.CreatedBy,
                        CreatedOn = x.StudentAddressInfo.CreatedOn,
                        ModifiedBy = x.StudentAddressInfo.ModifiedBy,
                        ModifiedOn = x.StudentAddressInfo.ModifiedOn,
                    }).ToList();
                    var addressInfoFormModel = new AddressInfoFormModel
                    {
                        StudentInfoId = StudentInfoId,
                        Address = addressList.Count() == 0 ? new List<AddressInfoModel>
                                                            {
                                                                new AddressInfoModel{ AddressType = AddressType.PemanentAddress },
                                                                new AddressInfoModel{ AddressType = AddressType.PresentAddress }
                                                            } : addressList
                    };
                    return addressInfoFormModel;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private AddressType ParseAddressType(string value)
        {
            return Enum.TryParse<AddressType>(value, true, out var result) ? result : AddressType.PemanentAddress;
        }
        public int CreateAddressInfo(AddressInfoModel address)
        {
            try
            {
                int addressInfoId = 0;

                if (address.IsDeleted == null)
                    address.IsDeleted = false;

                using (var db = new JaiHindEduEntitiesNew())
                {
                    var addressinfo = db.StudentAddressInfoes.Where(s => s.StudentAddressInfoId == address.StudentAddressInfoId && s.IsDeleted == false).FirstOrDefault();
                    if (addressinfo != null && addressinfo.StudentAddressInfoId > 0)
                    {
                        var beforeSnapshot = new
                        {
                            addressinfo.StudentAddressInfoId,
                            addressinfo.AddressType,
                            addressinfo.HouseNo,
                            addressinfo.Area,
                            addressinfo.Village,
                            addressinfo.PO,
                            addressinfo.District,
                            addressinfo.City,
                            addressinfo.State,
                            addressinfo.Zip,
                            addressinfo.SameAsPermanent,
                            addressinfo.IsDeleted
                        };

                        addressinfo.AddressType = address.AddressType.ToString();
                        addressinfo.HouseNo = address.HouseNo;
                        addressinfo.Area = address.Area;
                        addressinfo.Village = address.Village;
                        addressinfo.PO = address.PO;
                        addressinfo.District = address.District;
                        addressinfo.City = address.City;
                        addressinfo.State = address.State;
                        addressinfo.Zip = address.Zip;
                        addressinfo.SameAsPermanent = address.SameAsPermanent;
                        addressinfo.IsDeleted = address.IsDeleted;
                        addressinfo.ModifiedBy = address.ModifiedBy;
                        addressinfo.ModifiedOn = address.ModifiedOn;

                        db.SaveChanges();
                        addressInfoId = addressinfo.StudentAddressInfoId;

                        var afterSnapshot = new
                        {
                            addressinfo.StudentAddressInfoId,
                            AddressType = address.AddressType.ToString(),
                            address.HouseNo,
                            address.Area,
                            address.Village,
                            address.PO,
                            address.District,
                            address.City,
                            address.State,
                            address.Zip,
                            address.SameAsPermanent,
                            address.IsDeleted
                        };

                        TrackActivity(
                            "Updated",
                            "StudentAddressInfo",
                            addressInfoId.ToString(),
                            "Address information updated",
                            beforeSnapshot,
                            afterSnapshot);
                    }
                    else
                    {
                        var addressEntity = new StudentAddressInfo
                        {
                            AddressType = address.AddressType.ToString(),
                            HouseNo = address.HouseNo,
                            Area = address.Area,
                            Village = address.Village,
                            PO = address.PO,
                            District = address.District,
                            City = address.City,
                            State = address.State,
                            Zip = address.Zip,
                            SameAsPermanent = address.SameAsPermanent,
                            IsDeleted = address.IsDeleted,
                            CreatedBy = address.CreatedBy,
                            CreatedOn = address.CreatedOn,
                            ModifiedBy = address.ModifiedBy,
                            ModifiedOn = address.ModifiedOn
                        };
                        db.StudentAddressInfoes.Add(addressEntity);
                        db.SaveChanges();
                        addressInfoId = addressEntity.StudentAddressInfoId;

                        var assStudentAddressStudent = new AssStudentAddressStudentInfo
                        {
                            StudentInfoId = address.StudentInfoId,
                            StudentAddressInfoId = addressEntity.StudentAddressInfoId,
                            CreatedBy = addressEntity.CreatedBy,
                            CreatedOn = addressEntity.CreatedOn,
                            ModifiedBy = addressEntity.ModifiedBy,
                            ModifiedOn = addressEntity.ModifiedOn,
                            IsDeleted = address.IsDeleted
                        };
                        db.AssStudentAddressStudentInfoes.Add(assStudentAddressStudent);
                        db.SaveChanges();

                        var afterSnapshot = new
                        {
                            addressEntity.StudentAddressInfoId,
                            addressEntity.AddressType,
                            addressEntity.HouseNo,
                            addressEntity.Area,
                            addressEntity.Village,
                            addressEntity.PO,
                            addressEntity.District,
                            addressEntity.City,
                            addressEntity.State,
                            addressEntity.Zip,
                            addressEntity.SameAsPermanent,
                            addressEntity.IsDeleted
                        };

                        TrackActivity(
                            "Created",
                            "StudentAddressInfo",
                            addressInfoId.ToString(),
                            "Address information created",
                            null,
                            afterSnapshot);
                    }

                    return addressInfoId;
                }
            }
            catch
            {
                return 0;
            }
        }

        public int SaveUploadedDocument(int StudentInfoId, string FileOwner, string fullPath, string documentName, int fileSize, string CreatedBy)
        {
            try
            {
                int documentId = 0;
                int documentTypeId = 0;

                using (var db = new JaiHindEduEntitiesNew())
                {
                    if (FileOwner == "StudentPhoto")
                        documentTypeId = 1;
                    else if (FileOwner == "StudentSignature")
                        documentTypeId = 2;
                    else if (FileOwner == "FatherPhoto")
                        documentTypeId = 5;
                    else if (FileOwner == "FatherSignature")
                        documentTypeId = 6;
                    else if (FileOwner == "MotherPhoto")
                        documentTypeId = 3;
                    else if (FileOwner == "MotherSignature")
                        documentTypeId = 4;

                    var document = new Document
                    {
                        DocumentTypeId = documentTypeId,
                        Name = documentName,
                        Link = fullPath,
                        FileSize = fileSize.ToString(),
                        IsDeleted = false,
                        CreatedBy = CreatedBy,
                        CreatedOn = DateTime.Now,
                        ModifiedBy = CreatedBy,
                        ModifiedOn = DateTime.Now
                    };

                    db.Documents.Add(document);
                    db.SaveChanges();
                    documentId = document.DocumentId;

                    if (FileOwner == "StudentPhoto")
                    {
                        var userinfo = db.StudentInfoes.Where(s => s.StudentInfoId == StudentInfoId && s.IsDeleted == false).FirstOrDefault();
                        if (userinfo != null && userinfo.StudentInfoId > 0)
                        {
                            userinfo.StudentPhotoId = documentId;
                            userinfo.ModifiedBy = CreatedBy;
                            userinfo.ModifiedOn = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else if (FileOwner == "StudentSignature")
                    {
                        var userinfo = db.StudentInfoes.Where(s => s.StudentInfoId == StudentInfoId && s.IsDeleted == false).FirstOrDefault();
                        if (userinfo != null && userinfo.StudentInfoId > 0)
                        {
                            userinfo.StudentSignatureId = documentId;
                            userinfo.ModifiedBy = CreatedBy;
                            userinfo.ModifiedOn = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else if (FileOwner == "FatherPhoto")
                    {
                        AssStudentParentInfo assStudentParent = db.AssStudentParentInfoes.Where(p => p.StudentInfoId == StudentInfoId && p.IsDeleted == false && p.ParentType == "Father").FirstOrDefault();

                        var userinfo = db.ParentInfoes.Where(s => s.ParentInfoId == assStudentParent.ParentInfoId && s.IsDeleted == false).FirstOrDefault();
                        if (userinfo != null && userinfo.ParentInfoId > 0)
                        {
                            userinfo.ParentPhotoId = documentId;
                            userinfo.ModifiedBy = CreatedBy;
                            userinfo.ModifiedOn = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else if (FileOwner == "FatherSignature")
                    {
                        AssStudentParentInfo assStudentParent = db.AssStudentParentInfoes.Where(p => p.StudentInfoId == StudentInfoId && p.IsDeleted == false && p.ParentType == "Father").FirstOrDefault();

                        var userinfo = db.ParentInfoes.Where(s => s.ParentInfoId == assStudentParent.ParentInfoId && s.IsDeleted == false).FirstOrDefault();
                        if (userinfo != null && userinfo.ParentInfoId > 0)
                        {
                            userinfo.ParentSignatureId = documentId;
                            userinfo.ModifiedBy = CreatedBy;
                            userinfo.ModifiedOn = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else if (FileOwner == "MotherPhoto")
                    {
                        AssStudentParentInfo assStudentParent = db.AssStudentParentInfoes.Where(p => p.StudentInfoId == StudentInfoId && p.IsDeleted == false && p.ParentType == "Mother").FirstOrDefault();

                        var userinfo = db.ParentInfoes.Where(s => s.ParentInfoId == assStudentParent.ParentInfoId && s.IsDeleted == false).FirstOrDefault();
                        if (userinfo != null && userinfo.ParentInfoId > 0)
                        {
                            userinfo.ParentPhotoId = documentId;
                            userinfo.ModifiedBy = CreatedBy;
                            userinfo.ModifiedOn = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else if (FileOwner == "MotherSignature")
                    {
                        AssStudentParentInfo assStudentParent = db.AssStudentParentInfoes.Where(p => p.StudentInfoId == StudentInfoId && p.IsDeleted == false && p.ParentType == "Mother").FirstOrDefault();

                        var userinfo = db.ParentInfoes.Where(s => s.ParentInfoId == assStudentParent.ParentInfoId && s.IsDeleted == false).FirstOrDefault();
                        if (userinfo != null && userinfo.ParentInfoId > 0)
                        {
                            userinfo.ParentSignatureId = documentId;
                            userinfo.ModifiedBy = CreatedBy;
                            userinfo.ModifiedOn = DateTime.Now;
                            db.SaveChanges();
                        }
                    }

                    TrackActivity(
                        "Created",
                        "Document",
                        documentId.ToString(),
                        "Uploaded document: " + FileOwner,
                        null,
                        new
                        {
                            document.DocumentId,
                            document.DocumentTypeId,
                            document.Name,
                            document.Link,
                            document.FileSize,
                            FileOwner,
                            StudentInfoId
                        });

                    return documentId;
                }
            }
            catch
            {
                return 0;
            }
        }

        public AddmissionForm GetClassInfoByStudentInfoId(int StudentInfoId)
        {
            try
            {
                //string CurrentAcademicYear = WebConfigurationManager.AppSettings["CurrentAcademicYear"].ToString();
                using (var db = new JaiHindEduEntitiesNew())
                {
                    int academicyearid = db.LutAcademicYears.Where(a => a.IsCurrent == true).OrderByDescending(a => a.AcademicYearId).Select(a => a.AcademicYearId).FirstOrDefault();

                    return db.AddmissionForms.Where(x => x.StudentInfoId == StudentInfoId && x.IsDeleted == false && x.AcademicYearId == academicyearid).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public PreviousAcademicInfoModel GetAcademicStudentInfoId(int StudentInfoId, string ExamName)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var academicinfo = db.StudentPreviousAcademicInfoes
                        .Where(x => x.StudentInfoId == StudentInfoId && x.ExamName == ExamName && x.IsDeleted == false).FirstOrDefault();
                    if (academicinfo != null)
                    {
                        var academics = new PreviousAcademicInfoModel
                        {
                            StudentPreviousAcademicInfoId = academicinfo.StudentPreviousAcademicInfoId,
                            ExamName = academicinfo.ExamName,
                            Board = academicinfo.Board,
                            SchoolOrCollege = academicinfo.SchoolOrCollege,
                            ExamSeatNo = academicinfo.ExamSeatNo,
                            EligibilityCertNo = academicinfo.EligibilityCertNo,
                            MonthYearOfPassing = academicinfo.MonthYearOfPassing,
                            TotalMarks = academicinfo.TotalMarks,
                            Percentage = academicinfo.Percentage,
                            IsDeleted = false,
                            CreatedBy = academicinfo.CreatedBy,
                            CreatedOn = academicinfo.CreatedOn,
                            ModifiedBy = academicinfo.ModifiedBy,
                            ModifiedOn = academicinfo.ModifiedOn
                        };
                        return academics;
                    }
                    else return new PreviousAcademicInfoModel();
                }
            }
            catch
            {
                throw;
            }
        }

        public int CreatePreviousAcademicInfo(StudentAcademicViewModel academicViewModel)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var academicinfo = db.StudentPreviousAcademicInfoes.Where(s => s.StudentInfoId == academicViewModel.StudentInfoId && s.IsDeleted == false && s.ExamName == "Class 10th").FirstOrDefault();
                    if (academicinfo != null && academicinfo.StudentPreviousAcademicInfoId > 0)
                    {
                        var beforeSnapshot = new
                        {
                            academicinfo.StudentPreviousAcademicInfoId,
                            academicinfo.StudentInfoId,
                            academicinfo.ExamName,
                            academicinfo.Board,
                            academicinfo.SchoolOrCollege,
                            academicinfo.ExamSeatNo,
                            academicinfo.EligibilityCertNo,
                            academicinfo.MonthYearOfPassing,
                            academicinfo.TotalMarks,
                            academicinfo.Percentage
                        };

                        academicinfo.ExamName = academicViewModel.TenthClass.ExamName;
                        academicinfo.Board = academicViewModel.TenthClass.Board;
                        academicinfo.SchoolOrCollege = academicViewModel.TenthClass.SchoolOrCollege;
                        academicinfo.ExamSeatNo = academicViewModel.TenthClass.ExamSeatNo;
                        academicinfo.EligibilityCertNo = academicViewModel.TenthClass.EligibilityCertNo;
                        academicinfo.MonthYearOfPassing = academicViewModel.TenthClass.MonthYearOfPassing;
                        academicinfo.TotalMarks = academicViewModel.TenthClass.TotalMarks;
                        academicinfo.Percentage = academicViewModel.TenthClass.Percentage;
                        academicinfo.ModifiedBy = academicViewModel.ModifiedBy;
                        academicinfo.ModifiedOn = academicViewModel.ModifiedOn;
                        db.SaveChanges();

                        TrackActivity(
                            "Updated",
                            "StudentPreviousAcademicInfo",
                            academicinfo.StudentPreviousAcademicInfoId.ToString(),
                            "Class 10th academic info updated",
                            beforeSnapshot,
                            new
                            {
                                academicinfo.StudentPreviousAcademicInfoId,
                                academicViewModel.StudentInfoId,
                                academicViewModel.TenthClass.ExamName,
                                academicViewModel.TenthClass.Board,
                                academicViewModel.TenthClass.SchoolOrCollege,
                                academicViewModel.TenthClass.ExamSeatNo,
                                academicViewModel.TenthClass.EligibilityCertNo,
                                academicViewModel.TenthClass.MonthYearOfPassing,
                                academicViewModel.TenthClass.TotalMarks,
                                academicViewModel.TenthClass.Percentage
                            });
                    }
                    else
                    {
                        var academicinfomodel = new StudentPreviousAcademicInfo
                        {
                            StudentInfoId = academicViewModel.StudentInfoId,
                            ExamName = academicViewModel.TenthClass.ExamName,
                            Board = academicViewModel.TenthClass.Board,
                            SchoolOrCollege = academicViewModel.TenthClass.SchoolOrCollege,
                            ExamSeatNo = academicViewModel.TenthClass.ExamSeatNo,
                            EligibilityCertNo = academicViewModel.TenthClass.EligibilityCertNo,
                            MonthYearOfPassing = academicViewModel.TenthClass.MonthYearOfPassing,
                            TotalMarks = academicViewModel.TenthClass.TotalMarks,
                            Percentage = academicViewModel.TenthClass.Percentage,
                            IsDeleted = false,
                            CreatedBy = academicViewModel.CreatedBy,
                            CreatedOn = academicViewModel.CreatedOn,
                            ModifiedBy = academicViewModel.ModifiedBy,
                            ModifiedOn = academicViewModel.ModifiedOn
                        };
                        db.StudentPreviousAcademicInfoes.Add(academicinfomodel);
                        db.SaveChanges();

                        TrackActivity(
                            "Created",
                            "StudentPreviousAcademicInfo",
                            academicinfomodel.StudentPreviousAcademicInfoId.ToString(),
                            "Class 10th academic info created",
                            null,
                            new
                            {
                                academicinfomodel.StudentPreviousAcademicInfoId,
                                academicViewModel.StudentInfoId,
                                academicViewModel.TenthClass.ExamName,
                                academicViewModel.TenthClass.Board,
                                academicViewModel.TenthClass.SchoolOrCollege,
                                academicViewModel.TenthClass.ExamSeatNo,
                                academicViewModel.TenthClass.EligibilityCertNo,
                                academicViewModel.TenthClass.MonthYearOfPassing,
                                academicViewModel.TenthClass.TotalMarks,
                                academicViewModel.TenthClass.Percentage
                            });
                    }

                    if (academicViewModel.SelectedClass == "12th")
                    {
                        var academicinfo11 = db.StudentPreviousAcademicInfoes.Where(s => s.StudentInfoId == academicViewModel.StudentInfoId && s.IsDeleted == false && s.ExamName == "Class 11th").FirstOrDefault();
                        if (academicinfo11 != null && academicinfo11.StudentPreviousAcademicInfoId > 0)
                        {
                            var beforeSnapshot11 = new
                            {
                                academicinfo11.StudentPreviousAcademicInfoId,
                                academicinfo11.StudentInfoId,
                                academicinfo11.ExamName,
                                academicinfo11.Board,
                                academicinfo11.SchoolOrCollege,
                                academicinfo11.ExamSeatNo,
                                academicinfo11.EligibilityCertNo,
                                academicinfo11.MonthYearOfPassing,
                                academicinfo11.TotalMarks,
                                academicinfo11.Percentage
                            };

                            academicinfo11.ExamName = academicViewModel.EleventhClass.ExamName;
                            academicinfo11.Board = academicViewModel.EleventhClass.Board;
                            academicinfo11.SchoolOrCollege = academicViewModel.EleventhClass.SchoolOrCollege;
                            academicinfo11.ExamSeatNo = academicViewModel.EleventhClass.ExamSeatNo;
                            academicinfo11.EligibilityCertNo = academicViewModel.EleventhClass.EligibilityCertNo;
                            academicinfo11.MonthYearOfPassing = academicViewModel.EleventhClass.MonthYearOfPassing;
                            academicinfo11.TotalMarks = academicViewModel.EleventhClass.TotalMarks;
                            academicinfo11.Percentage = academicViewModel.EleventhClass.Percentage;
                            academicinfo11.ModifiedBy = academicViewModel.ModifiedBy;
                            academicinfo11.ModifiedOn = academicViewModel.ModifiedOn;
                            db.SaveChanges();

                            TrackActivity(
                                "Updated",
                                "StudentPreviousAcademicInfo",
                                academicinfo11.StudentPreviousAcademicInfoId.ToString(),
                                "Class 11th academic info updated",
                                beforeSnapshot11,
                                new
                                {
                                    academicinfo11.StudentPreviousAcademicInfoId,
                                    academicViewModel.StudentInfoId,
                                    academicViewModel.EleventhClass.ExamName,
                                    academicViewModel.EleventhClass.Board,
                                    academicViewModel.EleventhClass.SchoolOrCollege,
                                    academicViewModel.EleventhClass.ExamSeatNo,
                                    academicViewModel.EleventhClass.EligibilityCertNo,
                                    academicViewModel.EleventhClass.MonthYearOfPassing,
                                    academicViewModel.EleventhClass.TotalMarks,
                                    academicViewModel.EleventhClass.Percentage
                                });
                        }
                        else
                        {
                            var academicinfo11model = new StudentPreviousAcademicInfo
                            {
                                StudentInfoId = academicViewModel.StudentInfoId,
                                ExamName = academicViewModel.EleventhClass.ExamName,
                                Board = academicViewModel.EleventhClass.Board,
                                SchoolOrCollege = academicViewModel.EleventhClass.SchoolOrCollege,
                                ExamSeatNo = academicViewModel.EleventhClass.ExamSeatNo,
                                EligibilityCertNo = academicViewModel.EleventhClass.EligibilityCertNo,
                                MonthYearOfPassing = academicViewModel.EleventhClass.MonthYearOfPassing,
                                TotalMarks = academicViewModel.EleventhClass.TotalMarks,
                                Percentage = academicViewModel.EleventhClass.Percentage,
                                IsDeleted = false,
                                CreatedBy = academicViewModel.CreatedBy,
                                CreatedOn = academicViewModel.CreatedOn,
                                ModifiedBy = academicViewModel.ModifiedBy,
                                ModifiedOn = academicViewModel.ModifiedOn
                            };
                            db.StudentPreviousAcademicInfoes.Add(academicinfo11model);
                            db.SaveChanges();

                            TrackActivity(
                                "Created",
                                "StudentPreviousAcademicInfo",
                                academicinfo11model.StudentPreviousAcademicInfoId.ToString(),
                                "Class 11th academic info created",
                                null,
                                new
                                {
                                    academicinfo11model.StudentPreviousAcademicInfoId,
                                    academicViewModel.StudentInfoId,
                                    academicViewModel.EleventhClass.ExamName,
                                    academicViewModel.EleventhClass.Board,
                                    academicViewModel.EleventhClass.SchoolOrCollege,
                                    academicViewModel.EleventhClass.ExamSeatNo,
                                    academicViewModel.EleventhClass.EligibilityCertNo,
                                    academicViewModel.EleventhClass.MonthYearOfPassing,
                                    academicViewModel.EleventhClass.TotalMarks,
                                    academicViewModel.EleventhClass.Percentage
                                });
                        }
                    }

                    return 1;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var validationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
                return 0;
            }
        }

        public int SaveSelectedSubjectsInfo(SubjectSelectionModel subjectSelection)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var subjects = db.SubjectsChoosens.Where(x => x.AddmissionFormId == subjectSelection.AddmissionFormId).FirstOrDefault();
                    if (subjects != null && subjects.SubjectsChoosenId > 0)
                    {
                        var beforeSnapshot = new
                        {
                            subjects.SubjectsChoosenId,
                            subjects.AddmissionFormId,
                            subjects.Physics,
                            subjects.English,
                            subjects.Chemistry,
                            subjects.EVS,
                            subjects.PhysicalEducation,
                            subjects.Hindi,
                            subjects.Marathi,
                            subjects.Mathemcatics,
                            subjects.Biology,
                            subjects.Geography,
                            subjects.ComputerScience,
                            subjects.InformationTechnology,
                            subjects.InformaticsPractices,
                            subjects.CropScience
                        };

                        subjects.AddmissionFormId = subjectSelection.AddmissionFormId;
                        subjects.Physics = subjectSelection.Physics;
                        subjects.English = subjectSelection.English;
                        subjects.Chemistry = subjectSelection.Chemistry;
                        subjects.EVS = subjectSelection.EVS;
                        subjects.PhysicalEducation = subjectSelection.PhysicalEducation;
                        subjects.Hindi = subjectSelection.Hindi;
                        subjects.Marathi = subjectSelection.Marathi;
                        subjects.Mathemcatics = subjectSelection.Mathemcatics;
                        subjects.Biology = subjectSelection.Biology;
                        subjects.Geography = subjectSelection.Geography;
                        subjects.ComputerScience = subjectSelection.ComputerScience;
                        subjects.InformationTechnology = subjectSelection.InformationTechnology;
                        subjects.InformaticsPractices = subjectSelection.InformaticsPractices;
                        subjects.CropScience = subjectSelection.CropScience;
                        subjects.ModifiedBy = subjectSelection.CreatedBy;
                        subjects.ModifiedOn = DateTime.Now;
                        db.SaveChanges();

                        TrackActivity(
                            "Updated",
                            "SubjectsChoosen",
                            subjects.SubjectsChoosenId.ToString(),
                            "Subject selection updated",
                            beforeSnapshot,
                            new
                            {
                                subjects.SubjectsChoosenId,
                                subjectSelection.AddmissionFormId,
                                subjectSelection.Physics,
                                subjectSelection.English,
                                subjectSelection.Chemistry,
                                subjectSelection.EVS,
                                subjectSelection.PhysicalEducation,
                                subjectSelection.Hindi,
                                subjectSelection.Marathi,
                                subjectSelection.Mathemcatics,
                                subjectSelection.Biology,
                                subjectSelection.Geography,
                                subjectSelection.ComputerScience,
                                subjectSelection.InformationTechnology,
                                subjectSelection.InformaticsPractices,
                                subjectSelection.CropScience
                            });
                    }
                    else
                    {
                        var subjectSave = new SubjectsChoosen
                        {
                            AddmissionFormId = subjectSelection.AddmissionFormId,
                            Physics = subjectSelection.Physics,
                            English = subjectSelection.English,
                            Chemistry = subjectSelection.Chemistry,
                            EVS = subjectSelection.EVS,
                            PhysicalEducation = subjectSelection.PhysicalEducation,
                            Hindi = subjectSelection.Hindi,
                            Marathi = subjectSelection.Marathi,
                            Mathemcatics = subjectSelection.Mathemcatics,
                            Biology = subjectSelection.Biology,
                            Geography = subjectSelection.Geography,
                            ComputerScience = subjectSelection.ComputerScience,
                            InformationTechnology = subjectSelection.InformationTechnology,
                            InformaticsPractices = subjectSelection.InformaticsPractices,
                            CropScience = subjectSelection.CropScience,
                            CreatedBy = subjectSelection.CreatedBy,
                            CreatedOn = DateTime.Now,
                            ModifiedBy = subjectSelection.CreatedBy,
                            ModifiedOn = DateTime.Now
                        };
                        db.SubjectsChoosens.Add(subjectSave);
                        db.SaveChanges();

                        TrackActivity(
                            "Created",
                            "SubjectsChoosen",
                            subjectSave.SubjectsChoosenId.ToString(),
                            "Subject selection created",
                            null,
                            new
                            {
                                subjectSave.SubjectsChoosenId,
                                subjectSelection.AddmissionFormId,
                                subjectSelection.Physics,
                                subjectSelection.English,
                                subjectSelection.Chemistry,
                                subjectSelection.EVS,
                                subjectSelection.PhysicalEducation,
                                subjectSelection.Hindi,
                                subjectSelection.Marathi,
                                subjectSelection.Mathemcatics,
                                subjectSelection.Biology,
                                subjectSelection.Geography,
                                subjectSelection.ComputerScience,
                                subjectSelection.InformationTechnology,
                                subjectSelection.InformaticsPractices,
                                subjectSelection.CropScience
                            });
                    }
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int SaveSelectedDocumentInfo(DocumentSelectionModel documentSelection)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var documents = db.DocumentSubmitteds.Where(x => x.AddmissionFormId == documentSelection.AddmissionFormId).FirstOrDefault();
                    if (documents != null && documents.DocumentSubmittedId > 0)
                    {
                        var beforeSnapshot = new
                        {
                            documents.DocumentSubmittedId,
                            documents.AddmissionFormId,
                            documents.ClassXMarksheet,
                            documents.ClassXIMarksheet,
                            documents.SchoolLC,
                            documents.CasteCertificate,
                            documents.AadharCard,
                            documents.GapCertificate,
                            documents.SnglieGirlChild,
                            documents.ElegibilityCert,
                            documents.PassingCert,
                            documents.EBCCertificate,
                            documents.PassportSizePhoto,
                            documents.CopyOfLOC,
                            documents.AadharFather,
                            documents.AadharMother
                        };

                        documents.AddmissionFormId = documentSelection.AddmissionFormId;
                        documents.ClassXMarksheet = documentSelection.ClassXMarksheet;
                        documents.ClassXIMarksheet = documentSelection.ClassXIMarksheet;
                        documents.SchoolLC = documentSelection.SchoolLC;
                        documents.CasteCertificate = documentSelection.CasteCertificate;
                        documents.AadharCard = documentSelection.AadharCard;
                        documents.GapCertificate = documentSelection.GapCertificate;
                        documents.SnglieGirlChild = documentSelection.SnglieGirlChild;
                        documents.ElegibilityCert = documentSelection.ElegibilityCert;
                        documents.PassingCert = documentSelection.PassingCert;
                        documents.EBCCertificate = documentSelection.EBCCertificate;
                        documents.PassportSizePhoto = documentSelection.PassportSizePhoto;
                        documents.CopyOfLOC = documentSelection.CopyOfLOC;
                        documents.AadharFather = documentSelection.AadharFather;
                        documents.AadharMother = documentSelection.AadharMother;
                        documents.ModifiedBy = documentSelection.CreatedBy;
                        documents.ModifiedOn = DateTime.Now;
                        db.SaveChanges();

                        TrackActivity(
                            "Updated",
                            "DocumentSubmitted",
                            documents.DocumentSubmittedId.ToString(),
                            "Document selection updated",
                            beforeSnapshot,
                            new
                            {
                                documents.DocumentSubmittedId,
                                documentSelection.AddmissionFormId,
                                documentSelection.ClassXMarksheet,
                                documentSelection.ClassXIMarksheet,
                                documentSelection.SchoolLC,
                                documentSelection.CasteCertificate,
                                documentSelection.AadharCard,
                                documentSelection.GapCertificate,
                                documentSelection.SnglieGirlChild,
                                documentSelection.ElegibilityCert,
                                documentSelection.PassingCert,
                                documentSelection.EBCCertificate,
                                documentSelection.PassportSizePhoto,
                                documentSelection.CopyOfLOC,
                                documentSelection.AadharFather,
                                documentSelection.AadharMother
                            });
                    }
                    else
                    {
                        var documentSave = new DocumentSubmitted
                        {
                            AddmissionFormId = documentSelection.AddmissionFormId,
                            ClassXMarksheet = documentSelection.ClassXMarksheet,
                            ClassXIMarksheet = documentSelection.ClassXIMarksheet,
                            SchoolLC = documentSelection.SchoolLC,
                            CasteCertificate = documentSelection.CasteCertificate,
                            AadharCard = documentSelection.AadharCard,
                            GapCertificate = documentSelection.GapCertificate,
                            SnglieGirlChild = documentSelection.SnglieGirlChild,
                            ElegibilityCert = documentSelection.ElegibilityCert,
                            PassingCert = documentSelection.PassingCert,
                            EBCCertificate = documentSelection.EBCCertificate,
                            PassportSizePhoto = documentSelection.PassportSizePhoto,
                            CopyOfLOC = documentSelection.CopyOfLOC,
                            AadharFather = documentSelection.AadharFather,
                            AadharMother = documentSelection.AadharMother,
                            CreatedBy = documentSelection.CreatedBy,
                            CreatedOn = DateTime.Now,
                            ModifiedBy = documentSelection.CreatedBy,
                            ModifiedOn = DateTime.Now
                        };
                        db.DocumentSubmitteds.Add(documentSave);
                        db.SaveChanges();

                        TrackActivity(
                            "Created",
                            "DocumentSubmitted",
                            documentSave.DocumentSubmittedId.ToString(),
                            "Document selection created",
                            null,
                            new
                            {
                                documentSave.DocumentSubmittedId,
                                documentSelection.AddmissionFormId,
                                documentSelection.ClassXMarksheet,
                                documentSelection.ClassXIMarksheet,
                                documentSelection.SchoolLC,
                                documentSelection.CasteCertificate,
                                documentSelection.AadharCard,
                                documentSelection.GapCertificate,
                                documentSelection.SnglieGirlChild,
                                documentSelection.ElegibilityCert,
                                documentSelection.PassingCert,
                                documentSelection.EBCCertificate,
                                documentSelection.PassportSizePhoto,
                                documentSelection.CopyOfLOC,
                                documentSelection.AadharFather,
                                documentSelection.AadharMother
                            });
                    }
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public SubjectSelectionModel GetSelectedSubject(int AddmissionFormId)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var subjectSelection = db.SubjectsChoosens
                        .Where(x => x.AddmissionFormId == AddmissionFormId).FirstOrDefault();
                    if (subjectSelection != null)
                    {
                        var subjects = new SubjectSelectionModel
                        {
                            AddmissionFormId = subjectSelection.AddmissionFormId,
                            Physics = subjectSelection.Physics ?? false,
                            English = subjectSelection.English ?? false,
                            Chemistry = subjectSelection.Chemistry ?? false,
                            EVS = subjectSelection.EVS ?? false,
                            PhysicalEducation = subjectSelection.PhysicalEducation ?? false,
                            Hindi = subjectSelection.Hindi ?? false,
                            Marathi = subjectSelection.Marathi ?? false,
                            Mathemcatics = subjectSelection.Mathemcatics ?? false,
                            Biology = subjectSelection.Biology ?? false,
                            Geography = subjectSelection.Geography ?? false,
                            ComputerScience = subjectSelection.ComputerScience ?? false,
                            InformationTechnology = subjectSelection.InformationTechnology ?? false,
                            InformaticsPractices = subjectSelection.InformaticsPractices ?? false,
                            CropScience = subjectSelection.CropScience ?? false,
                            CreatedBy = subjectSelection.CreatedBy,
                            CreatedOn = DateTime.Now,
                            ModifiedBy = subjectSelection.CreatedBy,
                            ModifiedOn = DateTime.Now,
                        };
                        return subjects;
                    }
                    else return new SubjectSelectionModel();
                }
            }
            catch
            {
                throw;
            }
        }

        public DocumentSelectionModel GetSelectedDocument(int AddmissionFormId)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var documentSelection = db.DocumentSubmitteds
                        .Where(x => x.AddmissionFormId == AddmissionFormId).FirstOrDefault();
                    if (documentSelection != null)
                    {
                        var subjects = new DocumentSelectionModel
                        {
                            AddmissionFormId = documentSelection.AddmissionFormId,
                            ClassXMarksheet = documentSelection.ClassXMarksheet ?? false,
                            ClassXIMarksheet = documentSelection.ClassXIMarksheet ?? false,
                            SchoolLC = documentSelection.SchoolLC ?? false,
                            CasteCertificate = documentSelection.CasteCertificate ?? false,
                            AadharCard = documentSelection.AadharCard ?? false,
                            GapCertificate = documentSelection.GapCertificate ?? false,
                            SnglieGirlChild = documentSelection.SnglieGirlChild ?? false,
                            ElegibilityCert = documentSelection.ElegibilityCert ?? false,
                            PassingCert = documentSelection.PassingCert ?? false,
                            EBCCertificate = documentSelection.EBCCertificate ?? false,
                            PassportSizePhoto = documentSelection.PassportSizePhoto ?? false,
                            CopyOfLOC = documentSelection.CopyOfLOC ?? false,
                            AadharFather = documentSelection.AadharFather ?? false,
                            AadharMother = documentSelection.AadharMother ?? false,
                            CreatedBy = documentSelection.CreatedBy,
                            CreatedOn = DateTime.Now,
                            ModifiedBy = documentSelection.CreatedBy,
                            ModifiedOn = DateTime.Now,
                        };
                        return subjects;
                    }
                    else return new DocumentSelectionModel();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<UploadedImagesModel> GetUploadedImages(int StudentInfoId)
        {
            List<UploadedImagesModel> uploadedImagesModel = new List<UploadedImagesModel>();

            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    UploadedImagesModel uploaded = new UploadedImagesModel();

                    var student = db.StudentInfoes.Where(s => s.StudentInfoId == StudentInfoId).FirstOrDefault();

                    if (student != null)
                    {
                        if (student.StudentPhotoId != null)
                        {
                            var studentPhoto = db.Documents.FirstOrDefault(d => d.DocumentId == student.StudentPhotoId);
                            if (studentPhoto != null)
                            {
                                uploaded.DocumentName = studentPhoto.Name;
                                uploaded.DocumentURL = studentPhoto.Link;
                                uploaded.DocumentOwner = "Student";
                                uploaded.DocumentType = studentPhoto.DocumentTypeId == 1 ? "StudentPhoto" : "";
                                uploaded.StudentInfoId = StudentInfoId;
                                uploadedImagesModel.Add(uploaded);
                            }
                        }
                        if (student.StudentSignatureId != null)
                        {
                            uploaded = new UploadedImagesModel();
                            var studentPhoto = db.Documents.FirstOrDefault(d => d.DocumentId == student.StudentSignatureId);
                            if (studentPhoto != null)
                            {
                                uploaded.DocumentName = studentPhoto.Name;
                                uploaded.DocumentURL = studentPhoto.Link;
                                uploaded.DocumentOwner = "Student";
                                uploaded.DocumentType = studentPhoto.DocumentTypeId == 2 ? "StudentSignature" : "";
                                uploaded.StudentInfoId = StudentInfoId;
                                uploadedImagesModel.Add(uploaded);
                            }
                        }
                    }
                    var parentinfo = db.AssStudentParentInfoes
                        .Include(x => x.ParentInfo)
                        .Where(x => x.StudentInfoId == StudentInfoId && x.IsDeleted == false).ToList();

                    if (parentinfo.Count() > 0)
                    {
                        foreach (var parent in parentinfo)
                        {
                            uploaded = new UploadedImagesModel();
                            var ParentPhoto = db.Documents.FirstOrDefault(d => d.DocumentId == parent.ParentInfo.ParentPhotoId);
                            if (ParentPhoto != null)
                            {
                                uploaded.DocumentName = ParentPhoto.Name;
                                uploaded.DocumentURL = ParentPhoto.Link;
                                uploaded.DocumentOwner = parent.ParentInfo.ParentType;
                                if (parent.ParentInfo.ParentType == "Father")
                                    uploaded.DocumentType = ParentPhoto.DocumentTypeId == 5 ? "FathersPhoto" : "";
                                if (parent.ParentInfo.ParentType == "Mother")
                                    uploaded.DocumentType = ParentPhoto.DocumentTypeId == 3 ? "MothersPhoto" : "";
                                uploaded.StudentInfoId = StudentInfoId;
                                uploadedImagesModel.Add(uploaded);
                            }

                            uploaded = new UploadedImagesModel();
                            var ParentSignature = db.Documents.FirstOrDefault(d => d.DocumentId == parent.ParentInfo.ParentSignatureId);
                            if (ParentSignature != null)
                            {
                                uploaded.DocumentName = ParentSignature.Name;
                                uploaded.DocumentURL = ParentSignature.Link;
                                uploaded.DocumentOwner = parent.ParentInfo.ParentType;
                                if (parent.ParentInfo.ParentType == "Father")
                                    uploaded.DocumentType = ParentSignature.DocumentTypeId == 6 ? "FathersSignature" : "";
                                if (parent.ParentInfo.ParentType == "Mother")
                                    uploaded.DocumentType = ParentSignature.DocumentTypeId == 4 ? "MothersSignature" : "";
                                uploaded.StudentInfoId = StudentInfoId;
                                uploadedImagesModel.Add(uploaded);
                            }
                        }
                    }
                }
                return uploadedImagesModel;

            }
            catch (Exception ex)
            {
                return uploadedImagesModel;
            }
        }

        public string SubmitAndGenerateRollNo(int StudentInfoId)
        {
            try
            {
                string rollNumberReturn = "";
                string rollNumber = "";

                using (var db = new JaiHindEduEntitiesNew())
                {
                    var admissionform = db.AddmissionForms.Where(a => a.StudentInfoId == StudentInfoId && a.IsDeleted == false).OrderByDescending(s => s.RollNumber).FirstOrDefault();

                    var college = db.CollegeInfoes.FirstOrDefault(s => s.CollegeInfoId == admissionform.CollegeInfoId && s.IsActive == true);
                    string classno = "";
                    if (admissionform.ClassName == "11th")
                        classno = "11";
                    if (admissionform.ClassName == "12th")
                        classno = "12";

                    string stream = "";
                    if (admissionform.Stream == "Science")
                        stream = "S";
                    if (admissionform.Stream == "Commerce")
                        stream = "C";
                    string academicyear = db.LutAcademicYears.Where(a => a.IsCurrent == true).OrderByDescending(a => a.AcademicYearId).Select(a => a.Year).FirstOrDefault();
                    string prefix = college.CollegeNameShort + "/" + academicyear + "/" + classno + "/" + stream + "/";// school.SchoolCode;
                    //string prefix = college.CollegeNameShort + "/" + WebConfigurationManager.AppSettings["CurrentAcademicYear"].ToString() + "/" + classno + "/" + stream + "/";// school.SchoolCode;

                    //string latestRollNo = db.AddmissionForms.Where(a=>a.CollegeInfoId == admissionform.CollegeInfoId && a.RollNumber.StartsWith(prefix)).OrderByDescending(s => s.RollNumber).Select(s => s.RollNumber).FirstOrDefault();

                    //string latestRollNo = db.AddmissionForms.Where(a => a.CollegeInfoId == admissionform.CollegeInfoId && a.IsDeleted == false).OrderByDescending(s => s.RollNumber).Select(s => s.RollNumber).FirstOrDefault();

                    string latestRollNo = db.AddmissionForms.Where(a => a.CollegeInfoId == admissionform.CollegeInfoId && a.IsDeleted == false && a.RollNumber.StartsWith(prefix)).OrderByDescending(s => s.RollNumber).Select(s => s.RollNumber).FirstOrDefault();

                    int nextNumber = 1;

                    if (!string.IsNullOrEmpty(latestRollNo) && latestRollNo.Length > prefix.Length)
                    {
                        string numericPart = latestRollNo.Substring(prefix.Length);
                        if (int.TryParse(numericPart, out int parsed))
                        {
                            nextNumber = parsed + 1;
                        }
                    }

                    // Step 3: Format roll number as SBP001, SBP002, etc.
                    rollNumber = $"{prefix}{nextNumber.ToString("D3")}";


                    if (admissionform.RollNumber == null)
                        admissionform.RollNumber = rollNumber;
                    db.SaveChanges();

                    rollNumberReturn = admissionform.RollNumber;

                }
                return rollNumberReturn;
            }
            catch (Exception e)
            {
                return "";
            }

        }

        public List<CoachingCentreModel> GetCoachingCentres()
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    return db.CoachingCentres.Where(c => c.IsActive == true).Select(c => new CoachingCentreModel
                    {
                        CoachingCentreId = c.CoachingCentreId,
                        CoachingCentreName = c.CoachingCentreName
                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public CollegeInfoModel GetColelgeInfo(int StudentInfoId)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var collegeid = db.AddmissionForms.Where(x => x.StudentInfoId == StudentInfoId && x.IsDeleted == false).FirstOrDefault().CollegeInfoId;
                    return db.CollegeInfoes.Where(c => c.IsActive == true && c.CollegeInfoId == collegeid).Select(c => new CollegeInfoModel
                    {
                        CollegeInfoId = c.CollegeInfoId,
                        CollegeName = c.CollegeName,
                        CollegeNameShort = c.CollegeNameShort,
                        CollegeType = c.CollegeType,
                        AddressLine1 = c.AddressLine1,
                        AddressLine2 = c.AddressLine2,
                        City = c.City,
                        State = c.State,
                        Zip = c.Zip,
                        ContactNo1 = c.ContactNo1,
                        ContactNo2 = c.ContactNo2,
                        LogoURL = c.LogoURL,
                        IsActive = c.IsActive
                    }).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public List<AllAdmissionDataModel> GetAllAdmissionData(int CollegeInfoId)
        {
            using (var context = new JaiHindEduEntitiesNew())
            {
                var result = context.Database.SqlQuery<AllAdmissionDataModel>("EXEC JHE.GetAllAddmissionInfo @CollegeInfoId",
                        new SqlParameter("@CollegeInfoId", CollegeInfoId)).ToList();
                return result;
            }
        }

        public List<AllAdmissionDataModel> GetAllAdmissionData(int CollegeInfoId, string academicYear)
        {
            using (var context = new JaiHindEduEntitiesNew())
            {
                var result = context.Database.SqlQuery<AllAdmissionDataModel>(
                    "EXEC JHE.GetAllAddmissionInfo @CollegeInfoId, @AcademicYear",
                    new SqlParameter("@CollegeInfoId", CollegeInfoId),
                    new SqlParameter("@AcademicYear", academicYear ?? (object)DBNull.Value)
                ).ToList();

                return result;
            }
        }

        private void TrackActivity(string activityType, string entityName, string entityId, string description, object beforeValues = null, object afterValues = null)
        {
            ActivityLogHelper.Log(activityType, entityName,entityId, description, beforeValues, afterValues);
        }

        public int CreateOrUpdateAddressInfo(AddressInfoModel address)
        {
            try
            {
                int addressInfoId = 0;

                if (address.IsDeleted == null)
                    address.IsDeleted = false;

                using (var db = new JaiHindEduEntitiesNew())
                {
                    // First, try to find existing address by StudentAddressInfoId
                    StudentAddressInfo addressinfo = null;
                    
                    if (address.StudentAddressInfoId > 0)
                    {
                        addressinfo = db.StudentAddressInfoes
                            .FirstOrDefault(s => s.StudentAddressInfoId == address.StudentAddressInfoId && s.IsDeleted == false);
                    }
                    
                    // If not found by ID, check if address already exists for this student + address type
                    if (addressinfo == null && address.StudentInfoId > 0)
                    {
                        var existingAssociation = db.AssStudentAddressStudentInfoes
                            .Include("StudentAddressInfo")
                            .FirstOrDefault(x => x.StudentInfoId == address.StudentInfoId 
                                && x.StudentAddressInfo.AddressType == address.AddressType.ToString()
                                && x.IsDeleted == false);
                        
                        if (existingAssociation != null)
                        {
                            addressinfo = existingAssociation.StudentAddressInfo;
                        }
                    }

                    if (addressinfo != null && addressinfo.StudentAddressInfoId > 0)
                    {
                        // UPDATE existing address
                        var beforeSnapshot = new
                        {
                            addressinfo.StudentAddressInfoId,
                            addressinfo.AddressType,
                            addressinfo.HouseNo,
                            addressinfo.Area,
                            addressinfo.Village,
                            addressinfo.PO,
                            addressinfo.District,
                            addressinfo.City,
                            addressinfo.State,
                            addressinfo.Zip,
                            addressinfo.SameAsPermanent,
                            addressinfo.IsDeleted
                        };

                        addressinfo.AddressType = address.AddressType.ToString();
                        addressinfo.HouseNo = address.HouseNo;
                        addressinfo.Area = address.Area;
                        addressinfo.Village = address.Village;
                        addressinfo.PO = address.PO;
                        addressinfo.District = address.District;
                        addressinfo.City = address.City;
                        addressinfo.State = address.State;
                        addressinfo.Zip = address.Zip;
                        addressinfo.SameAsPermanent = address.SameAsPermanent;
                        addressinfo.IsDeleted = address.IsDeleted;
                        addressinfo.ModifiedBy = address.ModifiedBy;
                        addressinfo.ModifiedOn = address.ModifiedOn;

                        db.SaveChanges();
                        addressInfoId = addressinfo.StudentAddressInfoId;

                        var afterSnapshot = new
                        {
                            addressinfo.StudentAddressInfoId,
                            AddressType = address.AddressType.ToString(),
                            address.HouseNo,
                            address.Area,
                            address.Village,
                            address.PO,
                            address.District,
                            address.City,
                            address.State,
                            address.Zip,
                            address.SameAsPermanent,
                            address.IsDeleted
                        };

                        TrackActivity(
                            "Updated",
                            "StudentAddressInfo",
                            addressInfoId.ToString(),
                            "Address information updated",
                            beforeSnapshot,
                            afterSnapshot);
                    }
                    else
                    {
                        // CREATE new address
                        var addressEntity = new StudentAddressInfo
                        {
                            AddressType = address.AddressType.ToString(),
                            HouseNo = address.HouseNo,
                            Area = address.Area,
                            Village = address.Village,
                            PO = address.PO,
                            District = address.District,
                            City = address.City,
                            State = address.State,
                            Zip = address.Zip,
                            SameAsPermanent = address.SameAsPermanent,
                            IsDeleted = address.IsDeleted,
                            CreatedBy = address.CreatedBy,
                            CreatedOn = address.CreatedOn,
                            ModifiedBy = address.ModifiedBy,
                            ModifiedOn = address.ModifiedOn
                        };
                        db.StudentAddressInfoes.Add(addressEntity);
                        db.SaveChanges();
                        addressInfoId = addressEntity.StudentAddressInfoId;

                        var assStudentAddressStudent = new AssStudentAddressStudentInfo
                        {
                            StudentInfoId = address.StudentInfoId,
                            StudentAddressInfoId = addressEntity.StudentAddressInfoId,
                            CreatedBy = addressEntity.CreatedBy,
                            CreatedOn = addressEntity.CreatedOn,
                            ModifiedBy = addressEntity.ModifiedBy,
                            ModifiedOn = addressEntity.ModifiedOn,
                            IsDeleted = address.IsDeleted
                        };
                        db.AssStudentAddressStudentInfoes.Add(assStudentAddressStudent);
                        db.SaveChanges();

                        var afterSnapshot = new
                        {
                            addressEntity.StudentAddressInfoId,
                            addressEntity.AddressType,
                            addressEntity.HouseNo,
                            addressEntity.Area,
                            addressEntity.Village,
                            addressEntity.PO,
                            addressEntity.District,
                            addressEntity.City,
                            addressEntity.State,
                            addressEntity.Zip,
                            addressEntity.SameAsPermanent,
                            addressEntity.IsDeleted
                        };

                        TrackActivity(
                            "Created",
                            "StudentAddressInfo",
                            addressInfoId.ToString(),
                            "Address information created",
                            null,
                            afterSnapshot);
                    }

                    return addressInfoId;
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
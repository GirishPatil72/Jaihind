using JaiHindEdutech.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace JaiHindEdutech.Models
{
    public class AdmissionBL
    {

        public List<ApplicantStudentDto> GetAllStudents(int studentInfoId = 0, string academicYear = null)
        {
            List<ApplicantStudentDto> applicantStudentDto = new List<ApplicantStudentDto>();
            try
            {
                string currentAcademicYear = string.IsNullOrWhiteSpace(academicYear)
                    ? WebConfigurationManager.AppSettings["CurrentAcademicYear"].ToString()
                    : academicYear;

                using (var db = new JaiHindEduEntitiesNew())
                {
                    applicantStudentDto = db.Database.SqlQuery<ApplicantStudentDto>(
                        "EXEC [JHE].[GetAllApplicantStudentsByStudentId] @StudentInfoId,@AcademicYear",
                        new SqlParameter("@StudentInfoId", studentInfoId),
                        new SqlParameter("@AcademicYear", currentAcademicYear)).ToList();

                    return applicantStudentDto;
                }
            }
            catch
            {
                return applicantStudentDto;
            }

        }

        public AdmissionInfoByAddmissionModel GetAdmissionDataById(int AddmissionFormId)
        {
            using (var context = new JaiHindEduEntitiesNew())
            {
                AdmissionInfoByAddmissionModel result = context.Database.SqlQuery<AdmissionInfoByAddmissionModel>("EXEC [JHE].[GetAdmissionInfoByAddmissionFormId] @AddmissionFormId",
                     new SqlParameter("@AddmissionFormId", AddmissionFormId)).FirstOrDefault();
                return result;
            }
        }


        public int SaveFeeReceipt(FeeReceiptModel feeReceipt)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var fees = db.FeeReceipts.Where(x => x.AdmissionFormId == feeReceipt.AdmissionFormId).FirstOrDefault();
                    if (fees != null && fees.FeeReceiptId > 0)
                    {
                        var beforeSnapshot = new
                        {
                            fees.FeeReceiptId,
                            fees.AdmissionFormId,
                            fees.AdmissionFee,
                            fees.C1StInstallment,
                            fees.C2ndInstallment,
                            fees.C3rdInstallment,
                            fees.C4thInstallment,
                            fees.BalanceAmount,
                            fees.ExamFee,
                            fees.Misclellaneous,
                            fees.TotalAmount,
                            fees.IsDeleted
                        };

                        fees.AdmissionFee = feeReceipt.AdmissionFee;
                        fees.C1StInstallment = feeReceipt.C1StInstallment;
                        fees.C2ndInstallment = feeReceipt.C2ndInstallment;
                        fees.C3rdInstallment = feeReceipt.C3rdInstallment;
                        fees.C4thInstallment = feeReceipt.C4thInstallment;
                        fees.BalanceAmount = feeReceipt.BalanceAmount;
                        fees.ExamFee = feeReceipt.ExamFee;
                        fees.Misclellaneous = feeReceipt.Misclellaneous;
                        fees.TotalAmount = feeReceipt.TotalAmount;
                        fees.ModifiedBy = feeReceipt.CreatedBy;
                        fees.ModifiedOn = DateTime.Now;

                        db.SaveChanges();

                        TrackActivity(
                            "Updated",
                            "FeeReceipt",
                            fees.FeeReceiptId.ToString(),
                            "Fee receipt updated",
                            beforeSnapshot,
                            new
                            {
                                fees.FeeReceiptId,
                                fees.AdmissionFormId,
                                feeReceipt.AdmissionFee,
                                feeReceipt.C1StInstallment,
                                feeReceipt.C2ndInstallment,
                                feeReceipt.C3rdInstallment,
                                feeReceipt.C4thInstallment,
                                feeReceipt.BalanceAmount,
                                feeReceipt.ExamFee,
                                feeReceipt.Misclellaneous,
                                feeReceipt.TotalAmount,
                                fees.IsDeleted
                            });
                    }
                    else
                    {
                        var fee = new FeeReceipt
                        {
                            AdmissionFormId = feeReceipt.AdmissionFormId,
                            AdmissionFee = feeReceipt.AdmissionFee,
                            C1StInstallment = feeReceipt.C1StInstallment,
                            C2ndInstallment = feeReceipt.C2ndInstallment,
                            C3rdInstallment = feeReceipt.C3rdInstallment,
                            C4thInstallment = feeReceipt.C4thInstallment,
                            BalanceAmount = feeReceipt.BalanceAmount,
                            ExamFee = feeReceipt.ExamFee,
                            Misclellaneous = feeReceipt.Misclellaneous,
                            TotalAmount = feeReceipt.TotalAmount,
                            CreatedBy = feeReceipt.CreatedBy,
                            CreatedOn = DateTime.Now,
                            ModifiedBy = feeReceipt.CreatedBy,
                            ModifiedOn = DateTime.Now,
                        };
                        db.FeeReceipts.Add(fee);
                        db.SaveChanges();

                        TrackActivity(
                            "Created",
                            "FeeReceipt",
                            fee.FeeReceiptId.ToString(),
                            "Fee receipt created",
                            null,
                            new
                            {
                                fee.FeeReceiptId,
                                fee.AdmissionFormId,
                                fee.AdmissionFee,
                                fee.C1StInstallment,
                                fee.C2ndInstallment,
                                fee.C3rdInstallment,
                                fee.C4thInstallment,
                                fee.BalanceAmount,
                                fee.ExamFee,
                                fee.Misclellaneous,
                                fee.TotalAmount,
                                fee.IsDeleted
                            });
                    }
                }

                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public FeeReceiptModel GetFeeReceipt(int FeeReceiptId)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    return db.FeeReceipts.Where(c => c.IsDeleted == false && c.FeeReceiptId == FeeReceiptId).Select(c => new FeeReceiptModel
                    {
                        FeeReceiptId = c.FeeReceiptId,
                        AdmissionFee = c.AdmissionFee,
                        C1StInstallment = c.C1StInstallment,
                        C2ndInstallment = c.C2ndInstallment,
                        C3rdInstallment = c.C3rdInstallment,
                        //C4thInstallment=c.C4thInstallment,
                        BalanceAmount = c.BalanceAmount,
                        ExamFee = c.ExamFee,
                        Misclellaneous = c.Misclellaneous,
                        TotalAmount = c.TotalAmount,
                        IsDeleted = c.IsDeleted,
                        CreatedBy = c.CreatedBy,
                        CreatedOn = c.CreatedOn,
                        ModifiedBy = c.ModifiedBy,
                        ModifiedOn = c.ModifiedOn,
                        AdmissionFormId = c.AdmissionFormId
                    }).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<FeeData> GetAllFeeData()
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var FeeReceipt = (from receipt in db.FeeReceipts
                                      join admission in db.AddmissionForms on receipt.AdmissionFormId equals admission.AddmissionFormId
                                      join student in db.StudentInfoes on admission.StudentInfoId equals student.StudentInfoId
                                      select new FeeData
                                      {
                                          FeeReceiptId = receipt.FeeReceiptId,
                                          AdmissionFormId = admission.AddmissionFormId,
                                          StudentInfoId = student.StudentInfoId,
                                          FullName = student.FullName,
                                          ClassName = admission.ClassName,
                                          RollNumber = admission.RollNumber
                                      }).ToList();

                    return FeeReceipt;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<CollegeInfo> GetAllColleges()
        {
            using (var context = new JaiHindEduEntitiesNew())
            {
                var result = context.CollegeInfoes.Where(c => c.IsActive == true).ToList();
                return result;
            }
        }
        private void TrackActivity(string activityType, string entityName, string entityId, string description, object beforeValues = null, object afterValues = null)
        {
            ActivityLogHelper.Log(activityType, entityName, entityId, description, beforeValues, afterValues);
        }
    }
}
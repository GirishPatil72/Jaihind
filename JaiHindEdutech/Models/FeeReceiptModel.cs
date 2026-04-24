using JaiHindEdutech.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class FeeReceiptModel
    {
        public int FeeReceiptId { get; set; }
        public int AdmissionFormId { get; set; }

        [Display(Name = "Admission Fee")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter a valid decimal amount")]
        public Nullable<decimal> AdmissionFee { get; set; }

        [Display(Name = "1St Installment")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter a valid decimal amount")]
        public Nullable<decimal> C1StInstallment { get; set; }

        [Display(Name = "2nd Installment")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter a valid decimal amount")]
        public Nullable<decimal> C2ndInstallment { get; set; }

        [Display(Name = "3rd Installment")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter a valid decimal amount")]
        public Nullable<decimal> C3rdInstallment { get; set; }

        [Display(Name = "4th Installment")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter a valid decimal amount")]
        public Nullable<decimal> C4thInstallment { get; set; }

        [Display(Name = "Balance Amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter a valid decimal amount")]
        public Nullable<decimal> BalanceAmount { get; set; }

        [Display(Name = "Total Amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter a valid decimal amount")]
        public Nullable<decimal> TotalAmount { get; set; }

        [Display(Name = "Exam Fee")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter a valid decimal amount")]
        public Nullable<decimal> ExamFee { get; set; }

        [Display(Name = "Misclellaneous")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter a valid decimal amount")]
        public Nullable<decimal> Misclellaneous { get; set; }

        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }


        public string FullName { get; set; }
        public string ClassName { get; set; }
        public string Stream { get; set; }
        public int StudentInfoId { get; set; }
        public string RollNumber { get; set; }
        public string CollegeName { get; set; }
        public string CollegeAddress1 { get; set; }
        public string CollegeAddress2 { get; set; }
        public string CollegeCity { get; set; }
        public string CollegeContact { get; set; }
        public string CollegeType { get; set; }
        public string CollegeLogo { get; set; }

        public List<FeeReceipt> FeeReceipts { get; set; }
    }

    public class FeeData
    {
        public int FeeReceiptId { get; set; }
        public int AdmissionFormId { get; set; }
        public int StudentInfoId { get; set; }
        public string FullName { get; set; }
        public string RollNumber { get; set; }
        public string ClassName { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class StudentInfoModel
    {
        public int StudentInfoId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Middle name is required.")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [Display(Name = "Surname")]
        public string LastName { get; set; }
        public string FullName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [Display(Name = "Date Of Birth")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Aadhar Number is required.")]
        [Display(Name = "Aadhar Number")]
        public string AadharNo { get; set; }

        [Display(Name = "Is Single Girl Child")]
        public bool IsSingleGirlChild { get; set; }

        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        [Required(ErrorMessage = "Nationality is required.")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Mobile No is required.")]
        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Email Id is required.")]
        [Display(Name = "Email Id")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Domicile is required.")]
        public string Domicile { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public int ModelStepName { get; set; }
        public string encType { get; set; }
        public int CollegeInfoId { get; set; }
        public string CollegeName { get; set; }

        [Required(ErrorMessage = "Class selection is required.")]
        public string ClassName { get; set; }
        [Required(ErrorMessage = "Stream selection is required.")]
        public string Stream { get; set; }
        public int CoachingCentreId { get; set; }
        public int AddmissionFormId { get; set; }
        public string AdmissionFormNo { get; set; }

        //[Required(ErrorMessage = "Remark is required.")]
        public string Remark { get; set; }

        [Display(Name = "Total Fees")]
        [Range(0, double.MaxValue, ErrorMessage = "Enter a valid amount.")]
        public Nullable<decimal> TotalFees { get; set; }

        [Display(Name = "Scholarship")]
        [Range(0, double.MaxValue, ErrorMessage = "Enter a valid amount.")]
        public Nullable<decimal> Scholarship { get; set; }

        [Display(Name = "Fees Payable")]
        [Range(0, double.MaxValue, ErrorMessage = "Enter a valid amount.")]
        public Nullable<decimal> FeesPayable { get; set; }
    }
}
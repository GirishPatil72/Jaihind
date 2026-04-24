using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class ParentInfoModel
    {
        public int ParentInfoId { get; set; }
        public ParentType ParentType { get; set; }

        //[Required(ErrorMessage = "Full Name is required.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        //[Required(ErrorMessage = "Occupation is required.")]
        public string Occupation { get; set; }

        //[Required(ErrorMessage = "Aadhar No is required.")]
        [Display(Name = "Aadhar No")]
        public string AadharNo { get; set; }

        //[Required(ErrorMessage = "Annual Income is required.")]
        [Display(Name = "Annual Income")]
        public string AnnualIncome { get; set; }

        //[Required(ErrorMessage = "Mobile No is required.")]
        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        //[Required(ErrorMessage = "Email Id is required.")]
        [Display(Name = "Email Id")]
        public string EmailId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }

        public int StudentInfoId { get; set; }
    }

    public class ParentInfoFormModel
    {
        public int StudentInfoId { get; set; }
        public string encType { get; set; }

        // A list for two parents
        public List<ParentInfoModel> Parents { get; set; } = new List<ParentInfoModel>
        {
            new ParentInfoModel{ ParentType = ParentType.Father }, 
            new ParentInfoModel{ ParentType = ParentType.Mother }
        };
    }
    public enum ParentType
    {
        Father = 1,
        Mother = 2,
        Guardian = 3
    }
}
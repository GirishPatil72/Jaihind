using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class AdmissionInfoByAddmissionModel
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ClassName { get; set; }
        public string Stream { get; set; }
        public int AddmissionFormId { get; set; }
        public int StudentInfoId { get; set; }
        public string RollNumber { get; set; }
        public string CollegeName { get; set; }
        public string CollegeAddress1 { get; set; }
        public string CollegeAddress2 { get; set; }
        public string CollegeCity { get; set; }
        public string CollegeContact { get; set; }
        public string CoachingCentreName { get; set; }
        public string CoachingCentreContactNo { get; set; }
        public string CollegeType { get; set; }
        public string CollegeLogo { get; set; }

    }
}
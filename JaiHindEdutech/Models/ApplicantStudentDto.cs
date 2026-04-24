using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class ApplicantStudentDto
    {
        public int StudentInfoId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get; set; } // because it's formatted as string in SQL
        public string MobileNo { get; set; }
        public string AadharNo { get; set; }
        public string EmailId { get; set; }
        public int AddmissionFormId { get; set; }
        public int CollegeInfoId { get; set; }
        public string RollNumber { get; set; }
        public string CollegeName { get; set; }
    }
}
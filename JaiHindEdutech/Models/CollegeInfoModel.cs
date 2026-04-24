using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class CollegeInfoModel
    {
        public int CollegeInfoId { get; set; }
        public string CollegeName { get; set; }
        public string CollegeType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string LogoURL { get; set; }
        public string CollegeNameShort { get; set; }
    }
}
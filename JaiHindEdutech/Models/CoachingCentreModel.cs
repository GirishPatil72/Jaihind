using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class CoachingCentreModel
    {
        public int CoachingCentreId { get; set; }

        [Required(ErrorMessage = "Coaching Centre Name is required.")]
        [Display(Name = "Coaching Centre Name")]
        public string CoachingCentreName { get; set; }

        [Display(Name = "Address 1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address 2")]
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        [Display(Name = "Contact No")]
        public string ContactNo1 { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}
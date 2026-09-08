using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class AddressInfoModel
    {
        public int StudentAddressInfoId { get; set; }
        public bool SameAsPermanent { get; set; }
        public AddressType AddressType { get; set; }
        [Display(Name = "House No")]
        //[Required(ErrorMessage = "House No. is required.")]
        public string HouseNo { get; set; }
        //[Required(ErrorMessage = "Area is required.")]
        public string Area { get; set; }

        //[Required(ErrorMessage = "Village is required.")]
        public string Village { get; set; }

        //[Required(ErrorMessage = "PO is required.")]
        public string PO { get; set; }

        //[Required(ErrorMessage = "District is required.")]
        public string District { get; set; }

        //[Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        //[Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        //[Required(ErrorMessage = "Pincode is required.")]
        [Display(Name = "Pincode")]
        public string Zip { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        public int StudentInfoId { get; set; }
    }

    public class AddressInfoFormModel
    {
        public int StudentInfoId { get; set; }
        public string encType { get; set; }

        // A list for two parents
        public List<AddressInfoModel> Address { get; set; } = new List<AddressInfoModel>
        {
            new AddressInfoModel{ AddressType = AddressType.PemanentAddress },
            new AddressInfoModel{ AddressType = AddressType.PresentAddress }
        };
    }
    public enum AddressType
    {
        PemanentAddress = 1,
        PresentAddress = 2
    }
}
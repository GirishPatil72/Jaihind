using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class StudentUploadModel
    {
        public HttpPostedFileBase File { get; set; }
        public string Name { get; set; }
        public string FileOwner { get; set; }

        //public HttpPostedFileBase StudentPhoto { get; set; }
        //public string StudentPhotoName { get; set; }
        //public HttpPostedFileBase StudentSignature { get; set; }
        //public string StudentSignatureName { get; set; }
        //public HttpPostedFileBase FatherPhoto { get; set; }
        //public string FatherPhotoName { get; set; }
        //public HttpPostedFileBase MotherPhoto { get; set; }
        //public string MotherPhotoName { get; set; }
        //public HttpPostedFileBase FatherSignature { get; set; }
        //public string FatherSignatureName { get; set; }
        //public HttpPostedFileBase MotherSignature { get; set; }
        //public string MotherSignatureName { get; set; }
    }
}
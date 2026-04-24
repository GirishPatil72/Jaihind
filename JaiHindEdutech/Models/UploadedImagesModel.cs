using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class UploadedImagesModel
    {
        public int StudentInfoId { get; set; }

        public string DocumentName { get; set; }
        public string DocumentURL { get; set; }
        public string DocumentOwner { get; set; }
        public string DocumentType { get; set; }
    }
}
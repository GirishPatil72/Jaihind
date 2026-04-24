using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class AdmissionConfirmationModel
    {
        public int StudentInfoId { get; set; }
        public StudentInfoModel studentInfoModel { get; set; }

        public ParentInfoFormModel ParentInfoFormModel { get; set; }
        public AddressInfoFormModel addressInfoFormModel { get; set; }

        public StudentAcademicViewModel previousAcademicInfoModel { get; set; }

        public SubjectSelectionModel subjectSelectionModel { get; set; }

        public DocumentSelectionModel documentSelectionModel { get; set; }

        public List<UploadedImagesModel> uploadedImagesModel { get; set; }
        public CollegeInfoModel collegeInfoModel { get; set; }
    }
}
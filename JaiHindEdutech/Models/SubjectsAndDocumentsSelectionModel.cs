using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class SubjectsAndDocumentsSelectionModel
    {

        public int StudentInfoId { get; set; }
        public SubjectSelectionModel subjectSelectionModel { get; set; }
        public DocumentSelectionModel documentSelectionModel { get; set; }
    }


    public class SubjectSelectionModel
    {
        public int SubjectsChoosenId { get; set; }
        public Nullable<int> AddmissionFormId { get; set; }
        public bool English { get; set; }
        public bool Physics { get; set; }
        public bool Chemistry { get; set; }
        public bool EVS { get; set; }
        public bool PhysicalEducation { get; set; }
        public bool Hindi { get; set; }
        public bool Marathi { get; set; }
        public bool Mathemcatics { get; set; }
        public bool Biology { get; set; }
        public bool Geography { get; set; }
        public bool ComputerScience { get; set; }
        public bool InformationTechnology { get; set; }
        public bool InformaticsPractices { get; set; }
        public bool CropScience { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }



        //[Required(ErrorMessage = "Please select one subject.")]
        //public string Group1Subject { get; set; }
    }

    public class DocumentSelectionModel
    {
        public int DocumentSubmittedId { get; set; }
        public Nullable<int> AddmissionFormId { get; set; }
        public bool ClassXMarksheet { get; set; }
        public bool ClassXIMarksheet { get; set; }
        public bool SchoolLC { get; set; }
        public bool CasteCertificate { get; set; }
        public bool AadharCard { get; set; }
        public bool GapCertificate { get; set; }
        public bool SnglieGirlChild { get; set; }
        public bool ElegibilityCert { get; set; }
        public bool PassingCert { get; set; }
        public bool EBCCertificate { get; set; }
        public bool PassportSizePhoto { get; set; }
        public bool CopyOfLOC { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool AadharFather { get; set; }
        public bool AadharMother { get; set; }
    }
}
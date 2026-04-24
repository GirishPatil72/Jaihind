using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class PreviousAcademicInfoModel
    {
        public int StudentPreviousAcademicInfoId { get; set; }
        public Nullable<int> StudentInfoId { get; set; }

        [Display(Name = "Exam Name")]
        //[Required(ErrorMessage = "Exam Name is required.")]
        public string ExamName { get; set; }

        //[Required(ErrorMessage = "Board is required.")]
        public string Board { get; set; }

        [Display(Name = "School Or College")]
        //[Required(ErrorMessage = "School Or College is required.")]
        public string SchoolOrCollege { get; set; }

        [Display(Name = "Exam Seat No")]
        public string ExamSeatNo { get; set; }

        [Display(Name = "Eligibility Certificate No")]
        public string EligibilityCertNo { get; set; }

        [Display(Name = "Month And Year Of Passing")]
        //[Required(ErrorMessage = "Month And Year Of Passing is required.")]
        public string MonthYearOfPassing { get; set; }

        [Display(Name = "Total Marks Obtained")]
        //[Required(ErrorMessage = "Total Marks Obtained is required.")]
        public string TotalMarks { get; set; }

        [Display(Name = "Percentage")]
        //[Required(ErrorMessage = "Percentage is required.")]
        public string Percentage { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }

    }

    public class StudentAcademicViewModel
    {
        public Nullable<int> StudentInfoId { get; set; }
        public string SelectedClass { get; set; }

        // Always include 10th
        public PreviousAcademicInfoModel TenthClass { get; set; }

        // Only required if SelectedClass is 12th
        public PreviousAcademicInfoModel EleventhClass { get; set; }

        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}
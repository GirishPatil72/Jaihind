using System;
using System.Collections.Generic;

namespace JaiHindEdutech.Models
{
    public class HomeDashboardViewModel
    {
        public int TotalAdmissions { get; set; }
        public int CurrentAcademicYearAdmissions { get; set; }
        public int TotalColleges { get; set; }
        public int TotalAcademicYears { get; set; }
        public string CurrentAcademicYearName { get; set; }

        public List<NamedCountItem> CollegeAdmissions { get; set; } = new List<NamedCountItem>();
        public List<NamedCountItem> AcademicYearAdmissions { get; set; } = new List<NamedCountItem>();
        public List<NamedCountItem> ClassAdmissions { get; set; } = new List<NamedCountItem>();
        public List<NamedCountItem> TopColleges { get; set; } = new List<NamedCountItem>();
    }

    public class NamedCountItem
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
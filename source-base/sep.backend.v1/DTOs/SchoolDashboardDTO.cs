namespace sep.backend.v1.DTOs
{
    public class SchoolDashboardDTO
    {
        public int CountActiveClass { get; set; }
        public int CountActiveClub { get; set; }
        public int CountActiveBusRoute { get; set; }
        public int CountActiveBus { get; set; } 
        public int CountActivePupil { get; set; }
        public int CountActiveTeacher { get; set; }
        public int CountActiveSupervisor { get; set; }
        public int TotalActiveAccount { get; set; }
        public String? SemesterName { get; set; }
        public String? SchoolYearName { get; set; }





    }
}

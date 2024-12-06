namespace sep.backend.v1.Data.Entities
{
    public class School : BaseUserEntity
    {
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PrincipalName { get; set; }
        public string? PrincipalPhone { get; set; }
        public string? WebsiteLink { get; set; }
        public string? StandardCode { get; set; }
        public DateTime? DateOfEstablishment { get; set; }
        public string? FAX { get; set; }
        public virtual ICollection<SchoolSubscriptionPlan>? SchoolSubscriptionPlans { get; set; }
        public virtual ICollection<TimeSlot>? TimeSlots { get; set; }
        public virtual ICollection<Subject>? Subjects { get; set; }
        public virtual ICollection<Class>? Classes { get; set; }
        public virtual ICollection<BusSupervisor>? BusSupervisors { get; set; }
        public virtual ICollection<SchoolYear>? SchoolYears { get; set; }
        public virtual ICollection<BusRoute>? BusRoutes { get; set; }
        public virtual ICollection<Pupil>? Pupils { get; set; }
        public virtual ICollection<Teacher>? Teachers { get; set; }
        public virtual ICollection<Club>? Clubs { get; set; }

    }
}

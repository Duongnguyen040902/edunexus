namespace sep.backend.v1.Data.Entities
{
    public class BusEnrollment : BaseEntity
    {
      
        public int Id { get; set; }
        public int BusId { get; set; }
        public int? PupilId { get; set; }
        public int? BusSupervisorId { get; set; }
        public int SemesterId { get; set; }
        public int? BusStopId { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual Bus? Bus { get; set; }
        public virtual BusSupervisor? BusSupervisor { get; set; }
        public virtual Pupil? Pupil { get; set; }
        public virtual BusStop? BusStop { get; set; }

    }
}

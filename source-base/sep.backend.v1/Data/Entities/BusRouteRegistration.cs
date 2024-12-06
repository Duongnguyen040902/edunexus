namespace sep.backend.v1.Data.Entities
{
    public class BusRouteRegistration : BaseEntity
    {
        
        public int PupilId { get; set; }
        public int BusStopId { get; set; }
        public int SemesterId { get; set; }
        public int Status { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual Pupil? Pupil { get; set; }
        public virtual BusStop? BusStop { get; set; }
    }
}

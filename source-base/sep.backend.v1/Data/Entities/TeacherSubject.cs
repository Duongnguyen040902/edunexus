﻿namespace sep.backend.v1.Data.Entities
{
    public class TeacherSubject : BaseEntity
    {
        
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual Subject? Subject { get; set; }
    }
}

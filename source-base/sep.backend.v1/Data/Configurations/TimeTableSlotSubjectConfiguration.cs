using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Data.Configurations
{
    public class TimeTableSlotSubjectConfiguration: IEntityTypeConfiguration<TimeTableSlotSubject>
    {
       
            public void Configure(EntityTypeBuilder<TimeTableSlotSubject> builder)
            {
                builder.ToTable("TimeTableSlotSubjects");
                builder.HasKey(x => new { x.TimeSlotId , x.SubjectId, x.DayOfWeek });
                
            }
        }
    }

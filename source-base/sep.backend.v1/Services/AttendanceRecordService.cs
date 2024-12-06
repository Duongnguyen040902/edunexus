using EFCore.BulkExtensions;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services
{
    public class AttendanceRecordService : BaseService<AttendanceRecordDTO, AttendanceRecord>, IAttendanceRecordService
    {
        ApplicationContext context;
        public AttendanceRecordService(IUnitOfWork unitOfWork, IAutoMapper mapper, ApplicationContext context) : base(unitOfWork, mapper)
        {
            this.context = context;
        }

        public async Task<bool> CreateAttendanceRecords(List<AttendanceRecordDTO> attendanceRecords)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var attendanceRecordEntities = _mapper.Map<List<AttendanceRecordDTO>, List<AttendanceRecord>>(attendanceRecords);
                if (!attendanceRecordEntities.Any())
                {
                    throw new NotFoundException("Không tìm thấy bản ghi nào");
                }
                await _unitOfWork.GetRepository<AttendanceRecord>().BulkInsert(attendanceRecordEntities);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }       
        }

        public async Task<List<AttendanceRecordViewDTO>> GetAttendanceRecords(int entityId, int session, int type, int semesterId, DateTime date)
        {
            var pupils = await GetPupilsByType(entityId, session, semesterId);

            if (!pupils.Any())
            {
                throw new NotFoundException("Lớp chưa có học sinh nào tại kỳ này");
            }

            var attendanceRecords = pupils
                .Where(x => x.AttendanceRecords != null)
                .SelectMany(x => x.AttendanceRecords.Where(ar => 
                ar.AttendanceSession == session 
                && ar.AttendanceType == type
                && ar.CreatedDate.Date == date.Date
                && ((session == (int)AttendanceType.CLASSATTENDANCE && ar.ClassId == entityId) ||
                (session == (int)AttendanceType.CLUBATTENDANCE && ar.ClubId == entityId) ||
                (session == (int)AttendanceType.BUSATTENDANCE && ar.BusId == entityId)
                )))
                .OrderBy(x=>x.Pupil.Id).ToList();

            return attendanceRecords.Any() ? _mapper.Map<List<AttendanceRecord>, List<AttendanceRecordViewDTO>>(attendanceRecords) : null;
        }

        public async Task<bool> UpdateAttendanceRecords(List<AttendanceRecordDTO> updatedRecords)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var attendanceRecordEntities = _mapper.Map<List<AttendanceRecordDTO>, List<AttendanceRecord>>(updatedRecords);
                if (!attendanceRecordEntities.Any())
                {
                    throw new NotFoundException("Không tìm thấy bản ghi nào");
                }
                await _unitOfWork.GetRepository<AttendanceRecord>().BulkUpdate(attendanceRecordEntities);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<AttendanceRecordViewDTO>> GetPupilForAttendance(int entityId, int session, int type, int semesterId)
        {
            IEnumerable<dynamic> listPupil = session switch
            {
                (int)AttendanceType.CLASSATTENDANCE => await _unitOfWork.GetRepository<ClassEnrollment>()
                    .GetMulti(x => x.PupilId != null && x.SemesterId == semesterId && x.ClassId == entityId, new[] { "Pupil" }),
                (int)AttendanceType.CLUBATTENDANCE => await _unitOfWork.GetRepository<ClubEnrollment>()
                    .GetMulti(x => x.PupilId != null && x.SemesterId == semesterId && x.ClubId == entityId, new[] { "Pupil" }),
                _ => await _unitOfWork.GetRepository<BusEnrollment>()
                    .GetMulti(x => x.PupilId != null && x.SemesterId == semesterId && x.BusId == entityId, new[] { "Pupil" })
            };

            if (listPupil == null || !listPupil.Any())
            {
                throw new NotFoundException("Lớp chưa có học sinh nào trong kỳ này");
            }

            listPupil = listPupil.OrderBy(x => x.Pupil.Id);

            var attendanceRecords = listPupil.Select(pupil => new AttendanceRecordViewDTO
            {
                PupilId = pupil.Pupil.Id,
                PupilName = $"{pupil.Pupil.FirstName} {pupil.Pupil.LastName}",
                Image = pupil.Pupil.Image,
                AttendanceSession = session,
                AttendanceType = type,
                IsAttend = true,
                CreatedDate = DateTime.Now,
                ClassId = session == (int)AttendanceType.CLASSATTENDANCE ? entityId : null,
                ClubId = session == (int)AttendanceType.CLUBATTENDANCE ? entityId : null,
                BusId = session == (int)AttendanceType.BUSATTENDANCE ? entityId : null
            }).ToList();

            return attendanceRecords;
        }


        public async Task<List<AttendanceListDTO>> GetAttendanceListDTOs(int entityId, int session, int type, int semesterId, DateTime date)
        {
            var pupils = await GetPupilsByType(entityId, session, semesterId);

            var semester = await _unitOfWork.GetRepository<Semester>().GetById(semesterId);
            if (semester == null)
            {
                throw new NotFoundException("Không tìm thấy kỳ học");
            }

            var attendanceRecords = pupils
                                    .Where(x => x.AttendanceRecords != null)
                                    .SelectMany(x => x.AttendanceRecords.Where(ar =>
                                        ar.AttendanceSession == session &&
                                        ar.CreatedDate.Date <= date.Date &&
                                        ar.CreatedDate.Date >= semester.StartDate.Date &&
                                        ar.CreatedDate.Date <= semester.EndDate.Date &&
                                        ((session == (int)AttendanceType.CLASSATTENDANCE && ar.ClassId == entityId) ||
                                        (session == (int)AttendanceType.CLUBATTENDANCE && ar.ClubId == entityId) ||
                                        (session == (int)AttendanceType.BUSATTENDANCE && ar.BusId == entityId))))
                                    .ToList();

            if (!attendanceRecords.Any())
            {
                return null;
            }

            var attendanceListDTOs = attendanceRecords
                .GroupBy(ar => new { ar.CreatedDate.Date, ar.AttendanceType })
                .Select(g =>
                {
                    var totalPupils = pupils.Count();
                    var absentees = g.Count(ar => ar.IsAttend.HasValue && !ar.IsAttend.Value);

                    return new AttendanceListDTO
                    {
                        Name = type switch
                        {
                            (int)AttendanceType.CLASSATTENDANCE => "Lớp",
                            (int)AttendanceType.CLUBATTENDANCE => "Câu lạc bộ",
                            (int)AttendanceType.BUSATTENDANCE => "Xe tuyến",
                            _ => "Unknown"
                        },
                        Date = g.Key.Date,
                        Absentees = absentees,
                        Classize = totalPupils,
                        SemesterId = semesterId,
                        Session = type,
                        Type = g.Key.AttendanceType,
                        EntityId = entityId
                    };
                })
                .OrderByDescending(x => x.Date).ThenByDescending(x => x.Type)
                .Take(7)
                .ToList();

            return attendanceListDTOs;
        }


        private async Task<IEnumerable<Pupil>> GetPupilsByType(int entityId, int type, int semesterId)
        {
            var repository = _unitOfWork.GetRepository<Pupil>();
            return type switch
            {
                (int)AttendanceType.CLASSATTENDANCE => await repository.GetMulti(
                    x => x.PupilClasses != null && x.PupilClasses.Any(y => y.ClassId == entityId && y.SemesterId == semesterId),
                    new[] { "PupilClasses", "AttendanceRecords" }),
                (int)AttendanceType.CLUBATTENDANCE => await repository.GetMulti(
                    x => x.ClubEnrollments != null && x.ClubEnrollments.Any(y => y.ClubId == entityId && y.SemesterId == semesterId),
                    new[] { "ClubEnrollments", "AttendanceRecords" }),
                (int)AttendanceType.BUSATTENDANCE => await repository.GetMulti(
                    x => x.BusEnrollments != null && x.BusEnrollments.Any(y => y.BusId == entityId && y.SemesterId == semesterId),
                    new[] { "BusEnrollments", "AttendanceRecords" }),
                _ => throw new ArgumentException("Invalid attendance type")
            };
        }

        private bool IsMatchingEntity(AttendanceRecord ar, ClassEnrollment? classEnrollment, IEnumerable<ClubEnrollment> clubEnrollments, BusEnrollment? busEnrollment)
        {
            return (ar.ClassId.HasValue && ar.ClassId == classEnrollment?.Class.Id) ||
                   (ar.ClubId.HasValue && clubEnrollments.Any(ce => ce.ClubId == ar.ClubId)) ||
                   (ar.BusId.HasValue && ar.BusId == busEnrollment?.Bus.Id);
        }

        public async Task<PupilAttendance> GetPupilAttedanceList(int pupilId, int semesterId, DateTime date)
        {
            var pupil = await _unitOfWork.GetRepository<Pupil>()
                .GetSingleByCondition(x => x.Id == pupilId, new[] { "PupilClasses", "ClubEnrollments", "BusEnrollments", "AttendanceRecords" });
            if (pupil == null)
            {
                throw new NotFoundException("Không tìm thấy học sinh");
            }

            var semester = await _unitOfWork.GetRepository<Semester>().GetById(semesterId);
            if (semester == null)
            {
                throw new NotFoundException("Không tìm thấy kỳ học");
            }
            var classEnrollment = await _unitOfWork.GetRepository<ClassEnrollment>()
                .GetSingleByCondition(x => x.PupilId == pupilId && x.SemesterId == semesterId, new[] { "Class" });
            var clubEnrollments = await _unitOfWork.GetRepository<ClubEnrollment>()
                .GetMulti(x => x.PupilId == pupilId && x.SemesterId == semesterId, new[] { "Club" });
            var busEnrollment = await _unitOfWork.GetRepository<BusEnrollment>()
                .GetSingleByCondition(x => x.PupilId == pupilId && x.SemesterId == semesterId, new[] { "Bus" });

            var attendanceRecords = pupil.AttendanceRecords
                .Where(ar => (ar.CreatedDate.Date <= semester.EndDate.Date && ar.CreatedDate.Date >= semester.StartDate.Date) 
                && ar.CreatedDate.Date <= date.Date
                   && IsMatchingEntity(ar, classEnrollment, clubEnrollments, busEnrollment))
                .GroupBy(ar => new { ar.CreatedDate.Date, ar.AttendanceType })
                .Select(g => new PupilViewAttendanceDTO
                {
                    CreateDate = g.Key.Date,
                    Type = g.Key.AttendanceType,
                    IsAttendClass = g.Where(ar => ar.AttendanceSession == (int)AttendanceType.CLASSATTENDANCE)
                                     .Select(ar => new ViewClassAttendanceDTO
                                     {
                                         ClassId = ar.ClassId,
                                         IsAttend = ar.IsAttend,
                                         Feedback = ar.Feedback
                                     }).FirstOrDefault(),
                    IsAttendClub = g.Where(ar => ar.AttendanceSession == (int)AttendanceType.CLUBATTENDANCE)
                                    .Select(ar => new ViewClubAttendanceDTO
                                    {
                                        ClubId = ar.ClubId,
                                        IsAttend = ar.IsAttend,
                                        Feedback = ar.Feedback
                                    }).ToList(),
                    IsAttendBus = g.Where(ar => ar.AttendanceSession == (int)AttendanceType.BUSATTENDANCE)
                                   .Select(ar => new ViewBusAttendanceDTO
                                   {
                                       BusId = ar.BusId,
                                       IsAttend = ar.IsAttend,
                                       Feedback = ar.Feedback
                                   }).FirstOrDefault()
                })
                .OrderByDescending(x => x.CreateDate).ThenByDescending(x => x.Type)
                .Take(10)
                .ToList();

            var pupilAttendance = new PupilAttendance
            {
                PupilAttendanceMaterial = new PupilAttendanceMaterial
                {
                    ClassName = classEnrollment?.Class.Name,
                    ClubName = clubEnrollments.Select(ce => ce.Club.Name).ToArray(),
                    BusName = busEnrollment?.Bus.Name
                },
                PupilViewAttendanceDTO = attendanceRecords
            };

            return pupilAttendance;
        }

    }
}

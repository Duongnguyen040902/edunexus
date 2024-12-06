using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services
{
    public class PupilScoreService : BaseService<PupilScoreDTO, PupilScore>, IPupilScoreService
    {
        public PupilScoreService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<bool> CreatePupilScores(List<PupilScoreDTO> pupilScores)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = _mapper.Map<List<PupilScoreDTO>, List<PupilScore>>(pupilScores);
                if (!entity.Any())
                {
                    throw new NotFoundException("Không tìm thấy bản ghi nào");
                }
                await _unitOfWork.GetRepository<PupilScore>().BulkInsert(entity);
                await _unitOfWork.CommitTransactionAsync();
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                return false;
            }
        }

        public async Task<List<PupilScoreViewDTO>> GetPupilForCreate(int entityId, int semesterId, int subjectId)
        {
            var pupils = await _unitOfWork.GetRepository<ClassEnrollment>().GetMulti(
                x => x.ClassId == entityId && semesterId == x.SemesterId && x.PupilId != null,
                new string[] { "Pupil" }
            );
            if (!pupils.Any())
            {
                throw new NotFoundException("Lớp chưa có học sinh nào tại kỳ này");
            }

            var pupilScores = new List<PupilScoreViewDTO>();

            foreach (var pupil in pupils)
            {
                var pupilScore = new PupilScoreViewDTO
                {
                    PupilId = (int)pupil.PupilId,
                    PupilName = pupil.Pupil.FirstName + pupil.Pupil.LastName,
                    Image = pupil.Pupil.Image,
                    Score = 0,
                    SubjectId = subjectId,
                    SemesterId = semesterId,
                    Status = (int)ScoreStatus.IsPublic
                };

                pupilScores.Add(pupilScore);
            }

            return pupilScores;
        }

        public async Task<ClassScoreListDTO> GetPupilScoreListDTOs(int classId, int semesterId)
        {
            // Retrieve the school year based on the given semester
            var semester = await _unitOfWork.GetRepository<Semester>().GetById(semesterId);
            if (semester == null)
            {
                throw new NotFoundException("Không tìm thấy kỳ học");
            }

            var schoolYear = await _unitOfWork.GetRepository<SchoolYear>().GetById(semester.SchoolYearId);
            if (schoolYear == null)
            {
                throw new NotFoundException("Không tìm thấy năm học");
            }

            // Retrieve both semesters for the school year and sort by start date
            var semesters = (await _unitOfWork.GetRepository<Semester>().GetMulti(
                x => x.SchoolYearId == schoolYear.Id
            )).OrderBy(s => s.StartDate).ToList();

          

            var pupils = await _unitOfWork.GetRepository<ClassEnrollment>().GetMulti(
                 x => x.ClassId == classId && semesterId == x.SemesterId && x.PupilId != null,
                 new string[] { "Pupil" }
             );

            var subjects = await _unitOfWork.GetRepository<Subject>().GetMulti(
                x => x.PupilScores.Any(ps => ps.SemesterId == semesters.First().Id)
            );


            var sortedSubjects = subjects.OrderBy(subject => subject.Name).ToList();

            var pupilScoreList = new ClassScoreListDTO
            {
                Subjects = sortedSubjects.Select(subject => new SubjectScoreDTO
                {
                    Id = subject.Id,
                    Name = subject.Name
                }).ToList(),
                Pupils = pupils.Select(pupil => new PupilScoreSummaryDTO
                {
                    PupilId = (int)pupil.PupilId,
                    PupilName = pupil.Pupil.FirstName + " " + pupil.Pupil.LastName,
                    Image = pupil.Pupil.Image,
                    SubjectScores = new List<PupilSubjectScoreDTO>()
                }).ToList()
            };

            foreach (var pupil in pupils)
            {
                var pupilScores = await _unitOfWork.GetRepository<PupilScore>().GetMulti(
                    x => x.PupilId == pupil.PupilId,
                    new string[] { "Subject" }
                );

                var pupilSummary = pupilScoreList.Pupils.First(p => p.PupilId == pupil.PupilId);

                foreach (var subject in sortedSubjects)
                {
                    var subjectScores = new PupilSubjectScoreDTO
                    {
                        SubjectId = subject.Id,
                        SubjectName = subject.Name,
                        Scores = new List<float?>()
                    };

                    foreach (var sem in semesters)
                    {
                        var score = pupilScores.FirstOrDefault(ps => ps.SubjectId == subject.Id && ps.SemesterId == sem.Id);
                        subjectScores.Scores.Add(score?.Score);
                    }

                    pupilSummary.SubjectScores.Add(subjectScores);
                }
            }

            return pupilScoreList;
        }



        public async Task<List<PupilScoreViewDTO>> GetPupilScores(int entityId, int semesterId, int subjectId)
        {
            var pupils = await _unitOfWork.GetRepository<Pupil>().GetMulti(
               x => x.PupilClasses != null && x.PupilClasses.Any(x=>x.SemesterId == semesterId && x.ClassId == entityId),
               new string[] { "PupilScores" });

            if (!pupils.Any())
            {
                throw new NotFoundException("Lớp chưa có học sinh nào tại kỳ này");
            }

            var pupilScores = pupils
                .Where(x=>x.PupilScores != null)
                .SelectMany(x=>x.PupilScores.Where(y=>
                y.SemesterId == semesterId
                && y.SubjectId == subjectId)).ToList();

            return pupilScores.Any() ? _mapper.Map<List<PupilScore>, List<PupilScoreViewDTO>>(pupilScores) : null;
        }

        public async Task<bool> UpdatePupilScores(List<PupilScoreDTO> updatedScores)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entities = _mapper.Map<List<PupilScoreDTO>, List<PupilScore>>(updatedScores);
                if (!entities.Any())
                {
                    throw new NotFoundException("Không tìm thấy bản ghi nào");
                }
                await _unitOfWork.GetRepository<PupilScore>().BulkUpdate(entities);
                await _unitOfWork.CommitTransactionAsync();
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                return false;
            }
        }
        public async Task<List<PupilIndividualScoreDTO>> GetIndividualPupilScores(int pupilId, int semesterId)
        {
            // Retrieve the semester and school year
            var semester = await _unitOfWork.GetRepository<Semester>().GetById(semesterId);
            if (semester == null)
            {
                throw new NotFoundException("Không tìm thấy kỳ học");
            }

            var schoolYear = await _unitOfWork.GetRepository<SchoolYear>().GetById(semester.SchoolYearId);
            if (schoolYear == null)
            {
                throw new NotFoundException("Không tìm thấy năm học");
            }

            // Retrieve both semesters for the school year and sort by start date
            var semesters = (await _unitOfWork.GetRepository<Semester>().GetMulti(
                x => x.SchoolYearId == schoolYear.Id
            )).OrderBy(s => s.StartDate).ToList();

            // Retrieve pupil scores for both semesters
            var pupilScores = await _unitOfWork.GetRepository<PupilScore>().GetMulti(
                x => x.PupilId == pupilId,
                new string[] { "Subject" }
            );

            if (!pupilScores.Any())
            {
                return null;
            }

            // Group scores by subject and sort by subject name
            var individualScores = pupilScores
                .GroupBy(score => score.Subject.Name)
                .OrderBy(group => group.Key) // Sort by subject name
                .Select(group => new PupilIndividualScoreDTO
                {
                    SubjectName = group.Key,
                    Scores = new List<float?>()
                }).ToList();

            // Add scores for each semester
            foreach (var subjectScore in individualScores)
            {
                foreach (var sem in semesters)
                {
                    var score = pupilScores.FirstOrDefault(ps => ps.Subject.Name == subjectScore.SubjectName && ps.SemesterId == sem.Id);
                    subjectScore.Scores.Add(score?.Score);
                }
            }

            return individualScores;
        }

    }
}
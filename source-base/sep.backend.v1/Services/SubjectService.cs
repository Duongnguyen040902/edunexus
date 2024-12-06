using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;
using System.Xml;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Helpers;

namespace sep.backend.v1.Services
{
    public class SubjectService : BaseService<SubjectDTO, Subject>, ISubjectService
    {
        public SubjectService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<List<SubjectDTO>> GetAll(int schoolId)
        {
            var subjects = await _unitOfWork.GetRepository<Subject>()
                .GetMulti(x => x.SchoolId == schoolId);
            if (!subjects.Any())
            {
                return null;
            }

            return subjects.Select(t => _mapper.Map<Subject, SubjectDTO>(t)).ToList();
        }

        public Task<SubjectDTO> GetSubjectById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<SubjectDTO> CreateSubject(SubjectDTO subjectDTO, int schoolId)
        {
            subjectDTO.SchoolId = schoolId;
            var newSubject = _mapper.Map<SubjectDTO, Subject>(subjectDTO);
            await _unitOfWork.SubjectRepository.Add(newSubject);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<Subject, SubjectDTO>(newSubject);
        }

        public async Task<SubjectDTO> UpdateSubject(int id, SubjectDTO subjectDTO)
        {
            var subject = await _unitOfWork.GetRepository<Subject>().GetById(id);
            if (subject == null)
            {
                throw new NotFoundException(
                    StringHelper.FormatMessage(Messages.NOT_FOUND, "Môn học"));
            }

            subject.Name = subjectDTO.Name;
            subject.Code = subjectDTO.Code;
            await _unitOfWork.GetRepository<Subject>().Update(subject);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<Subject, SubjectDTO>(subject);
        }

        public async Task<bool> DeleteSubject(int id, int schoolId)
        {
            var subject = _unitOfWork.GetRepository<Subject>()
                .Where(s => s.Id == id && s.SchoolId == schoolId).FirstOrDefault();

            if (subject is null)
            {
                throw new NotFoundException();
            }

            bool hasRelatedEntities = (subject.PupilFeedbacks != null && subject.PupilFeedbacks.Any()) ||
                                      (subject.PupilScores != null && subject.PupilScores.Any()) ||
                                      (subject.TimeTables != null && subject.TimeTables.Any()) ||
                                      (subject.TeacherSubjects != null && subject.TeacherSubjects.Any());

            if (hasRelatedEntities)
            {
                throw new ConflictException(StringHelper.FormatMessage(Messages.CONFLICT,
                    "Không thể xóa hóa đơn vì đã có liên kết"));
            }

            await _unitOfWork.GetRepository<Subject>().Delete(id);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services
{
    public class PupilFeedbackService : BaseService<PupilFeedbackDTO, PupilFeedback>, IPupilFeedbackService
    {
        public PupilFeedbackService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<bool> CreatePupilFeedback(PupilFeedbackDTO models)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var pupilFeedback = _mapper.Map<PupilFeedbackDTO, PupilFeedback>(models);
                pupilFeedback.Status = (int)FeedbackStatus.Active;
                pupilFeedback.CreatedDate = DateTime.Now;
                await _unitOfWork.GetRepository<PupilFeedback>().Add(pupilFeedback);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeletePupilFeedback(int pupilId, int semesterId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.GetRepository<PupilFeedback>()
                    .DeleteMulti(x => x.PupilId == pupilId && x.SemesterId == semesterId);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
                throw new Exception(ex.Message);
            }
        }

        public Task<PupilFeedbackDTO> GetPupilFeedback(int pupilId, int semesterId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<PupilFeedbackDetailDTO>> GetPupilFeedbacks(int pupilId)
        {
            var pupilFeedback = await _unitOfWork.FeedbackRepository.GetAllFeedbacksOfPupil(pupilId);

            if (pupilFeedback.Count == 0)
            {
                return null;
            }

            var data = _mapper.Map<List<PupilFeedback>, List<PupilFeedbackDetailDTO>>(pupilFeedback);

            return data;
        }

        public async Task<List<ListPupilFeedbackDTO>> GetPupilFeedbackOfClass(int classId, int semesterId)
        {
            var pupils = await _unitOfWork.GetRepository<Pupil>()
                .GetMulti(
                    p => p.PupilClasses.Any(pc => pc.ClassId == classId && pc.SemesterId == semesterId),
                    new string[] { "PupilFeedbacks" }
                );
            if (pupils == null || !pupils.Any())
            {
                throw new NotFoundException("Lớp chưa có học sinh nào trong kỳ này");
            }
            var feedbackList = new List<ListPupilFeedbackDTO>();

            foreach (var pupil in pupils)
            {
                var feedbacks = pupil.PupilFeedbacks.Where(fb => fb.SemesterId == semesterId).ToList();

                if (feedbacks.Any())
                {
                    feedbackList.AddRange(feedbacks.Select(feedback => new ListPupilFeedbackDTO
                    {
                        PupilId = feedback.PupilId,
                        PupilName = $"{pupil.FirstName} {pupil.LastName}",
                        DonorName = pupil.DonorName,
                        Image = pupil.Image,
                        SemesterId = feedback.SemesterId,
                        Description = feedback.Description,
                        Status = feedback.Status,
                        CreatedDate = feedback.CreatedDate
                    }));
                }
                else
                {
                    feedbackList.Add(new ListPupilFeedbackDTO
                    {
                        PupilId = pupil.Id,
                        PupilName = $"{pupil.FirstName} {pupil.LastName}",
                        DonorName = pupil.DonorName,
                        Image = pupil.Image,
                        SemesterId = semesterId,
                        Description = null,
                        Status = 0
                    });
                }
            }

            return feedbackList;
        }

        async Task<bool> IPupilFeedbackService.UpdatePupilFeedback(PupilFeedbackDTO models)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var pupilFeedback = _mapper.Map<PupilFeedbackDTO, PupilFeedback>(models);
                await _unitOfWork.GetRepository<PupilFeedback>().Update(pupilFeedback);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
                throw new Exception(ex.Message);
            }
        }
    }
}
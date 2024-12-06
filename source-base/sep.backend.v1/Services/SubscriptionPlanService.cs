using sep.backend.v1.Common.Const;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapper _mapper;

        public SubscriptionPlanService(IUnitOfWork unitOfWork,
            IAutoMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SubscriptionPlanDTO>> GetAllSubscriptionsAsync()
        {
            var subscriptionPlan = await _unitOfWork.SubscriptionPlanRepository.GetAllSubscriptionAsync();
            var subscriptionPlanDTOs = subscriptionPlan
                .Select(subscription => _mapper.Map<SubscriptionPlan, SubscriptionPlanDTO>(subscription)).ToList();

            return subscriptionPlanDTOs;
        }

        public async Task<SubscriptionPlanDetailDTO> GetSubscriptionsDetailAsync(int subscriptionId)
        {
            var subscriptionPlan = await _unitOfWork.GetRepository<SubscriptionPlan>().GetById(subscriptionId);
            if (subscriptionPlan is null)
            {
                throw new NotFoundException("The SubscriptionPlan is not found.");
            }

            var subscriptionPlanDTOs = _mapper.Map<SubscriptionPlan, SubscriptionPlanDetailDTO>(subscriptionPlan);

            return subscriptionPlanDTOs;
        }

        public async Task<List<SubscriptionPlanDTO>> GetAllSubscriptionsByAdminAsync()
        {
            var subscriptionPlan = await _unitOfWork.SubscriptionPlanRepository
                .All();
            if (subscriptionPlan is null)
            {
                throw new NotFoundException("The SubscriptionPlan list is empty.");
            }

            var subscriptionPlanDTOs = subscriptionPlan
                .Select(subscription => _mapper.Map<SubscriptionPlan, SubscriptionPlanDTO>(subscription))
                .OrderByDescending(sp => sp.Id)
                .ToList();


            return subscriptionPlanDTOs;
        }

        public async Task<SubscriptionPlanDTO> GetSubscriptionByIdAsync(int id)
        {
            var subscriptionPlan = await _unitOfWork.SubscriptionPlanRepository.GetById(id);

            if (subscriptionPlan == null)
            {
                throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Gói dịch vụ"));
            }

            var subscriptionPlanDTO = _mapper.Map<SubscriptionPlan, SubscriptionPlanDTO>(subscriptionPlan);

            return subscriptionPlanDTO;
        }

        public async Task<SubscriptionPlanDTO> CreateSubscriptionAsync(SubscriptionPlanDTO subscriptionPlanDTO)
        {
            var subscriptionPlan = _mapper.Map<SubscriptionPlanDTO, SubscriptionPlan>(subscriptionPlanDTO);
            await _unitOfWork.SubscriptionPlanRepository.Add(subscriptionPlan);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<SubscriptionPlan, SubscriptionPlanDTO>(subscriptionPlan);
        }

        public async Task<SubscriptionPlanDTO> UpdateSubscriptionAsync(int id, SubscriptionPlanDTO subscriptionPlanDTO)
        {
            var subscriptionPlan = await _unitOfWork.SubscriptionPlanRepository.GetById(id);
            if (subscriptionPlan == null)
            {
                throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Gói dịch vụ"));
            }

            _mapper.Map(subscriptionPlanDTO, subscriptionPlan);
            await _unitOfWork.SubscriptionPlanRepository.Update(subscriptionPlan);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<SubscriptionPlan, SubscriptionPlanDTO>(subscriptionPlan);
        }
    }
}
using AutoMapper;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, InvoiceDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.schoolId, opt => opt.MapFrom(src => src.SchoolSubscriptionPlans.SchoolId))
                .ForMember(dest => dest.SubscriptionPlanId, opt => opt.MapFrom(src => src.SchoolSubscriptionPlans.SubscriptionPlanId))
                .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => GetStatusName((InvoiceStatuses)src.Status)))
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.SchoolSubscriptionPlans.School.Name))
                .ForMember(dest => dest.SubscriptionPlanName, opt => opt.MapFrom(src => src.SchoolSubscriptionPlans.SubscriptionPlan.Name))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.SchoolSubscriptionPlans.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.SchoolSubscriptionPlans.EndDate))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.SchoolSubscriptionPlans.SubscriptionPlan.Price));

            CreateMap<CreateInvoiceDTO, Invoice>()
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.SchoolSubscriptionPlanId, opt => opt.MapFrom(src => src.SubscriptionPlanId));
                
        }

        private string GetStatusName(InvoiceStatuses status)
        {
            return status switch
            {
                InvoiceStatuses.Pending => "Chờ",
                InvoiceStatuses.Paid => "Đã thanh toán",
                InvoiceStatuses.Cancel => "Hủy",
                InvoiceStatuses.Sent => "Đã gửi",
                InvoiceStatuses.Change => "Thay đổi",
                _ => "Chưa xác định"
            };
        }
    }
}
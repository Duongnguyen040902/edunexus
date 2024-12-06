using AutoMapper;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile() {
            CreateMap<Payment, PaymentDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            
            CreateMap<PaymentDTO, Payment>()
                .ForMember(dest => dest.InvoiceId , opt => opt.MapFrom(src => src.InvoiceId))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));


            CreateMap<Payment, PaymentDetailDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => GetPaymentStatusName((PaymentStatuses)src.Status)));
        }


        private string GetPaymentStatusName(PaymentStatuses status)
        {
            return status switch
            {
                PaymentStatuses.Success => "Thành công",
                PaymentStatuses.Error => "Thất bại",
                _ => "Unknown"
            };
        }
    }
}

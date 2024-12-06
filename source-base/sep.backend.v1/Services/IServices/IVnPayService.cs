using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestDTO model);
        Task<VnPaymentResponseDTO> PaymentExecute(IQueryCollection collections);
    }
}

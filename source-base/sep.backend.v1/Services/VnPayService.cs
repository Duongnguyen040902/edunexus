using sep.backend.v1.DTOs;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IServices;
using System.Transactions;

namespace sep.backend.v1.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;
        private readonly VNPayLibrary _vnPayLibrary;

        public VnPayService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _vnPayLibrary = new VNPayLibrary(httpContextAccessor, config);
        }

        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestDTO model)
        {
            var tick = DateTime.Now.Ticks.ToString();
            string applicationUrl = Environment.GetEnvironmentVariable("APPLICATION_URL");
            string paymentBackReturnPath = _config["VnPay:PaymentBackReturnUrl"];
            string fullReturnUrl = $"{applicationUrl}{paymentBackReturnPath}";

            _vnPayLibrary.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            _vnPayLibrary.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            _vnPayLibrary.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            _vnPayLibrary.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            _vnPayLibrary.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            _vnPayLibrary.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            _vnPayLibrary.AddRequestData("vnp_IpAddr", _vnPayLibrary.GetIpAddress());
            _vnPayLibrary.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
            _vnPayLibrary.AddRequestData("vnp_OrderInfo", $"{model.InvoiceId}_{model.SubscriptionPlan}");
            _vnPayLibrary.AddRequestData("vnp_OrderType", "other"); 
            _vnPayLibrary.AddRequestData("vnp_ReturnUrl", fullReturnUrl);
            _vnPayLibrary.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = _vnPayLibrary.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);
            return paymentUrl;
        }

        public async Task<VnPaymentResponseDTO> PaymentExecute(IQueryCollection collections)
        {
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    _vnPayLibrary.AddResponseData(key, value.ToString());
                }
            }

            var vnpOrderId = _vnPayLibrary.GetResponseData("vnp_TxnRef");
            var vnpTransactionId = _vnPayLibrary.GetResponseData("vnp_TransactionNo");
            var vnp_TransactionStatus = _vnPayLibrary.GetResponseData("vnp_TransactionStatus");
            var vnpSecureHash = collections["vnp_SecureHash"].ToString();
            var vnpResponseCode = _vnPayLibrary.GetResponseData("vnp_ResponseCode");
            var vnpOrderInfo = _vnPayLibrary.GetResponseData("vnp_OrderInfo");

            var invoiceId = vnpOrderInfo?.Split('_')[0];

            bool checkSignature = _vnPayLibrary.ValidateSignature(vnpSecureHash, _config["VnPay:HashSecret"]);
            if (!checkSignature)
            {
                return new VnPaymentResponseDTO
                {
                    IsSuccess = false,
                };
            }

            return new VnPaymentResponseDTO
            {
                IsSuccess = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnpOrderInfo,
                OrderId = vnpOrderId,
                InvoiceId = invoiceId,
                TransactionId = vnpTransactionId,
                TransactionStatus = vnp_TransactionStatus,
                Token = vnpSecureHash,
                VnPayResponseCode = vnpResponseCode
            };
        }
    }
}

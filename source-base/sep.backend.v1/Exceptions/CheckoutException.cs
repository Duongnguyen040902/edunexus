using sep.backend.v1.Common.Const;

namespace sep.backend.v1.Exceptions
{
    public class CheckoutException : CustomException
    {
        public CheckoutException() : base(Responses.CheckoutErrorMessage, Responses.CheckoutErrorCode)
        {
        }

        public CheckoutException(string message) : base(message, Responses.CheckoutErrorCode)
        {
        }

        public CheckoutException(string message, Exception innerException) : base(message, Responses.CheckoutErrorCode, innerException)
        {
        }

        public CheckoutException(string message, string errorCode) : base(message, errorCode)
        {
        }

        public CheckoutException(string message, string errorCode, Exception innerException) : base(message, errorCode, innerException)
        {
        }
    }
}
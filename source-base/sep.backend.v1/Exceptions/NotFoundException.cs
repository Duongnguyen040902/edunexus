
namespace sep.backend.v1.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NotFoundException(string message = "NotFound", string errorCode = "404") : base(message, errorCode)
        {
        }

        public NotFoundException(string message, string errorCode, Exception innerException) : base(message, errorCode, innerException)
        {
        }
    }
}

using sep.backend.v1.Common.Const;

namespace sep.backend.v1.Exceptions
{
    public class ConflictException : CustomException
    {
        public ConflictException() : base(Responses.ConflictErrorMessage, Responses.ConflictErrorCode)
        {
        }

        public ConflictException(string message) : base(message, Responses.ConflictErrorCode)
        {
        }

        public ConflictException(string message, Exception innerException) : base(message, Responses.ConflictErrorCode, innerException)
        {
        }

        public ConflictException(string message, string errorCode) : base(message, errorCode)
        {
        }

        public ConflictException(string message, string errorCode, Exception innerException) : base(message, errorCode, innerException)
        {
        }
    }
}
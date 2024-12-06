namespace sep.backend.v1.Exceptions
{
    public class CustomException : Exception
    {
        public string ErrorCode { get; }

        public CustomException(string message) : base(message)
        {
        }

        public CustomException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CustomException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public CustomException(string message, string errorCode, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
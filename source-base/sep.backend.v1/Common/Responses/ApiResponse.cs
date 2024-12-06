namespace sep.backend.v1.Common.Responses
{
    public class ApiResponse<T>
    {
        public ApiResponse()
        {
        }

        public ApiResponse(T data, string message = "Success")
        {
            Succeeded = true;
            Message = message;
            Errors = null;
            Data = data;
        }

        public ApiResponse(string message, Dictionary<string, string[]>? errors = null)
        {
            Succeeded = false;
            Message = message;
            Errors = errors;
            Data = default!;
        }

        public T Data { get; set; } = default!;
        public bool Succeeded { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ApiResponse<T> Success(T data, string message = "Success")
        {
            return new ApiResponse<T>(data, message);
        }

        public static ApiResponse<T> Error(string message, Dictionary<string, string[]>? errors = null)
        {
            return new ApiResponse<T>(message, errors);
        }
    }
}

using System.Text.Json.Serialization;

namespace sep.backend.v1.Common.Responses
{
    public class ErrorResponse
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; }
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        public ErrorResponse(int statusCode, string message, List<string> errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors ?? new List<string>();
            Success = false;
        }
    }

    public class ErrorResponseWithData<T> : ErrorResponse
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }

        public ErrorResponseWithData(int statusCode, string message, T data, List<string> errors = null)
            : base(statusCode, message, errors)
        {
            Data = data;
        }
    }
}

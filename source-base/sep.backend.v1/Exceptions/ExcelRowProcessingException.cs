using sep.backend.v1.Common.Responses;

namespace sep.backend.v1.Exceptions
{
    public class ExcelRowProcessingException : CustomException
    {
        public int RowNumber { get; }
        public string ErrorFilePath { get; }

        public ExcelRowProcessingException(
            int rowNumber,
            string message,
            string errorFilePath = null,
            string errorCode = "422")
            : base(message, errorCode)
        {
            RowNumber = rowNumber;
            ErrorFilePath = errorFilePath;
        }

        public async Task<ErrorResponseWithData<object>> GetErrorDetailsAsync()
        {
            if (!string.IsNullOrEmpty(ErrorFilePath))
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(ErrorFilePath);
                var timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var errorData = new
                {   
                    fileName = $"error_{timeStamp}.txt",
                    fileContent = Convert.ToBase64String(fileBytes)
                };

                return new ErrorResponseWithData<object>(
                    statusCode: 422,
                    message: Message,
                    data: errorData
                );
            }

            return new ErrorResponseWithData<object>(
                statusCode: 422,
                message: Message,
                data: null
            );
        }
    }
}

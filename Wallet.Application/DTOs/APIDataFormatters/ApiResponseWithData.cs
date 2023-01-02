namespace Wallet.Application.DTOs.APIDataFormatters
{
    public partial class ApiResponse<T> where T : class
    {
        public ApiResponse(T data, string code, string status, string message)
        {
            Data = data;
            StatusCode = code;
            Status = status;
            Message = message;
        }

        public T Data { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}

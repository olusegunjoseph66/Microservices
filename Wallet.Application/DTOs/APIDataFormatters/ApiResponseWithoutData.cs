namespace Wallet.Application.DTOs.APIDataFormatters
{
    public partial class ApiResponse
    {
        public ApiResponse(string code, string status, string message)
        {
            Status = status;
            StatusCode = code;
            Message = message;
        }

        public object Data { get; set; } = null;
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
    }
}

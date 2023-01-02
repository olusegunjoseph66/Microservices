namespace Wallet.Application.DTOs.APIDataFormatters
{
    public class ErrorDto
    {
        public ErrorDto(string code, string message)
        {
            Code = code;
            Message = message;
        }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}

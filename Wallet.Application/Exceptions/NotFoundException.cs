using System.Globalization;
using Wallet.Application.CodeFactories;
using Wallet.Application.Constants;
using Wallet.Application.DTOs.APIDataFormatters;

namespace Wallet.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        {
            Response = ResponseHandler.FailureResponse(ErrorCodes.NOTFOUND_ERROR_CODE, ErrorMessages.NOT_FOUND_ERROR);
        }

        public NotFoundException(string message) : base(message)
        {
            Response = ResponseHandler.FailureResponse(ErrorCodes.NOTFOUND_ERROR_CODE, message);
        }

        public ApiResponse Response { get; private set; }

        public NotFoundException(string message, string code, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            Response = ResponseHandler.FailureResponse(code, message);
        }
    }
}

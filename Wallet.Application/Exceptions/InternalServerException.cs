using System.Globalization;
using Wallet.Application.CodeFactories;
using Wallet.Application.Constants;
using Wallet.Application.DTOs.APIDataFormatters;

namespace Wallet.Application.Exceptions
{
    public class InternalServerException : Exception
    {
        public InternalServerException() : base()
        {
            Response = ResponseHandler.FailureResponse(ErrorCodes.SERVER_ERROR_CODE, ErrorMessages.SERVER_ERROR);
        }

        public InternalServerException(string message) : base(message)
        {
            Response = ResponseHandler.FailureResponse(ErrorCodes.SERVER_ERROR_CODE, message);
        }

        public ApiResponse Response { get; private set; }

        public InternalServerException(string message, string code, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            Response = ResponseHandler.FailureResponse(code, message);
        }
    }
}

using System.Globalization;
using Wallet.Application.CodeFactories;
using Wallet.Application.Constants;
using Wallet.Application.DTOs.APIDataFormatters;

namespace Wallet.Application.Exceptions
{
    public class UnauthorizedUserException : Exception
    {
        public UnauthorizedUserException() : base()
        {
            Response = ResponseHandler.FailureResponse(ErrorCodes.DEFAULT_AUTHORIZATION_CODE, ErrorMessages.DEFAULT_AUTHORIZATION_MESSAGE);
        }

        public UnauthorizedUserException(string message) : base(message)
        {
            Response = ResponseHandler.FailureResponse(ErrorCodes.DEFAULT_AUTHORIZATION_CODE, message);
        }

        public ApiResponse Response { get; private set; }

        public UnauthorizedUserException(string message, string code, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            Response = ResponseHandler.FailureResponse(code, message);
        }
    }
}

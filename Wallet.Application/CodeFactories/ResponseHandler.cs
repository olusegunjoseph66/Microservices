using Shared.Utilities.Helpers;
using Wallet.Application.Constants;
using Wallet.Application.DTOs.APIDataFormatters;
using Wallet.Application.Enums;

namespace Wallet.Application.CodeFactories
{
    public static class ResponseHandler<T> where T : class
    {
        public static ApiResponse<T> SuccessResponse(string message, string code = SuccessCodes.DEFAULT_SUCCESS_CODE, T? data = null)
        {
            return new ApiResponse<T>(data, code, ResponseStatusEnum.Successful.ToDescription(), message);
        }

        public static ApiResponse<T> FailureResponse(string message, string code, T? data = null)
        {
            return new ApiResponse<T>(data, code, ResponseStatusEnum.Failed.ToDescription(), message);
        }


        public static ApiResponse<T> SuccessResponse(string message)
        {
            T? data = null;
            return new ApiResponse<T>(data, SuccessCodes.DEFAULT_SUCCESS_CODE, ResponseStatusEnum.Successful.ToDescription(), message);
        }

        public static ApiResponse<T> SuccessResponse(string message, T? data = null)
        {
            return new ApiResponse<T>(data, SuccessCodes.DEFAULT_SUCCESS_CODE, ResponseStatusEnum.Successful.ToDescription(), message);
        }
    }

    public static class ResponseHandler
    {
        public static ApiResponse FailureResponse(string code, string message)
        {
            return new ApiResponse(code, ResponseStatusEnum.Failed.ToDescription(), message);
        }
    }
}

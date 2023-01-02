using Microsoft.EntityFrameworkCore;
using Shared.Utilities.Handlers;
using Shared.Utilities.Helpers;
using System.Data.SqlClient;
using System.Text.Json;
using Wallet.Application.Exceptions;
using Wallet.Application.Constants;
using Wallet.Application.Enums;
using Wallet.Application.DTOs.APIDataFormatters;

namespace Wallet.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                ApiResponse? responseModel = null;
                var response = context.Response;
                response.ContentType = "application/json";
                switch (ex)
                {
                    case UnauthorizedAccessException exception:
                        _logger.LogError(ex, "An Unauthorized Exception Occurred - 401");
                        response.StatusCode = StatusCodes.Status401Unauthorized;
                        responseModel = new ApiResponse(ErrorCodes.DEFAULT_AUTHORIZATION_CODE, ResponseStatusEnum.Failed.ToDescription(), exception.Message);
                        break;
                    case UnauthorizedUserException exception:
                        _logger.LogError(ex, "An Unauthorized Exception Occurred - 401");
                        response.StatusCode = StatusCodes.Status401Unauthorized;
                        responseModel = new ApiResponse(exception.Response.StatusCode, ResponseStatusEnum.Failed.ToDescription(), exception.Response.Message);
                        break;
                    case ValidationException exception:
                        _logger.LogError(ex, "An Error Occurred due to Bad Request - 400");
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        responseModel = new ApiResponse(exception.Response.Code, ResponseStatusEnum.Failed.ToDescription(), exception.Response.Message);
                        break;
                    case InternalServerException exception:
                        _logger.LogError(ex, "A Server Error Occurred - 500");
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        responseModel = new ApiResponse(exception.Response.StatusCode, ResponseStatusEnum.Failed.ToDescription(), exception.Response.Message);
                        break;
                    case NotFoundException exception:
                        _logger.LogError(ex, "An Error Occurred due to resource Not Found - 404");
                        response.StatusCode = StatusCodes.Status404NotFound;
                        responseModel = new ApiResponse(exception.Response.StatusCode, ResponseStatusEnum.Failed.ToDescription(), exception.Response.Message);
                        break;
                    default:
                        // unhandled error
                        if (ex is DbUpdateException databaseInsertException)
                        {
                            if (databaseInsertException.InnerException is SqlException sqlException)
                            {
                                if (sqlException.Number == ErrorCodes.SqlServerViolationOfUniqueConstraint || sqlException.Number == ErrorCodes.SqlServerViolationOfUniqueIndex)
                                {
                                    var validationResponse = SqlHandlers.UniqueErrorFormatter(sqlException);
                                    if (validationResponse != null)
                                        _logger.LogError(ex, $"A Database Error Occurred - 409:{ErrorCodes.DATABASE_INSERT_CONFLICT_CODE}", new List<DatabaseErrorDto>
                                    {
                                        new DatabaseErrorDto{ TableName = validationResponse.TableName, ColumnName = validationResponse.ColumnName, MaskedColumnValue = validationResponse.MaskedColumnValue }
                                    });

                                    response.StatusCode = StatusCodes.Status409Conflict;
                                    responseModel = new ApiResponse(ErrorCodes.DATABASE_INSERT_CONFLICT_CODE, ResponseStatusEnum.Failed.ToDescription(), ErrorMessages.DATABASE_CONFLICT_ERROR);

                                }
                            }
                        }
                        else
                        {
                            _logger.LogError(ex, "An Unknown Error Occurred - 500");
                            response.StatusCode = StatusCodes.Status500InternalServerError;
                            responseModel = new ApiResponse(ErrorCodes.SERVER_ERROR_CODE, ResponseStatusEnum.Failed.ToDescription(), ErrorMessages.SERVER_ERROR);
                        }
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                await response.WriteAsync(result);
            }


        }
    }
}

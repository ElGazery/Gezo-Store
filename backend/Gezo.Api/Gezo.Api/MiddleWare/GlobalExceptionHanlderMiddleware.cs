using Gezo.Api.ResponseBase.Response;
using Gezo.Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace Gezo.Api.MiddleWares
{
    public class GlobalExceptionHanlderMiddleware
        (RequestDelegate NextMd,ILogger<GlobalExceptionHanlderMiddleware> logger,IWebHostEnvironment env)
    {

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await NextMd(context);

                if(context.Response.StatusCode==404 && !context.Response.HasStarted)
                {
                    await HandleResponseExeptionAsync(context, 404, "The Requested resource not found");

                }

                // function handle response


            }
            catch(Exception error)
            {
                logger.LogError(error, $" Error : {error.Message}");
                await HandleExeptionAsync(context, error);

            }

        }
      
        public async Task HandleResponseExeptionAsync(HttpContext context,int statusCode,string msg)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var response =new ApiResponse<string>(msg, statusCode);
            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions{ PropertyNamingPolicy= JsonNamingPolicy.CamelCase});
            await context.Response.WriteAsync(jsonResponse);

        }


        public async Task HandleExeptionAsync(HttpContext context,Exception ex)
        {
            context.Response.ContentType = "application/json";

            string ErrorMessage = env.IsDevelopment() ? $"Error : {ex.Message} \n ,stackTrace : {ex.StackTrace}" : "An unexpected  Error occurred try again later";

            var response = ex switch
            {
                NotFoundCustomException => new ApiResponse<string>(ex.Message, StatusCodes.Status404NotFound),
                BadRequestCustomException BR=> new ApiResponse<string>(ex.Message, StatusCodes.Status400BadRequest,errors:BR.Errors?.ToList()),
                UnauthorizedCustomException=>new ApiResponse<string>(ex.Message,StatusCodes.Status401Unauthorized),
                _ => new ApiResponse<string>( "An unexpected error occurred.", StatusCodes.Status500InternalServerError)

            };
            context.Response.StatusCode = response.StatusCode;
            response.IsSuccess = false;

            var jsonResponse = JsonSerializer.Serialize(response, 
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

           await context.Response.WriteAsync(jsonResponse);
        }

    }
}

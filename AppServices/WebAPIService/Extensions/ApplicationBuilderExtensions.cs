using System.Linq;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebAPIService
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(x=> {
                x.Run(async context => {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;

                    if(!(exception is ValidationException validationException)) throw exception;

                    var errors = validationException.Errors.Select(e=>new {
                        e.PropertyName,
                        e.ErrorMessage
                    });                    

                    var errorText = JsonConvert.SerializeObject(errors);
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(errorText, Encoding.UTF8);
                });
            });
        }        
    }
}
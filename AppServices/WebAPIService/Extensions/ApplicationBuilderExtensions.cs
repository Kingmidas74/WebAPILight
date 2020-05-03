using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebAPIService.Exceptions;

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
                    string content= exception switch {
                        ClientValidationException e when exception is ClientValidationException => JsonConvert.SerializeObject(e.Messages),
                        _ => JsonConvert.SerializeObject(new Dictionary<string,object> {
                                {nameof(Exception),exception.Message}
                            })
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(content, Encoding.UTF8);
                });
            });
        }        
    }
}
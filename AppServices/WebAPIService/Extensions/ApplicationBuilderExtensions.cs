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
                    (string content, int code) = exception switch {
                        ClientValidationException e when exception is ClientValidationException => (JsonConvert.SerializeObject(e.Messages), StatusCodes.Status400BadRequest),
                        _ => (JsonConvert.SerializeObject(new Dictionary<string,object> {
                                {"Processing error","Contact to tech support"}
                            }), StatusCodes.Status500InternalServerError)
                    };
                    context.Response.StatusCode=code;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(content, Encoding.UTF8);
                });
            });
        }        
    }
}
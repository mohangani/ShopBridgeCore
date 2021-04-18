using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShopBridge.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Api.MiddleWares
{
    public class ExceptionMiddleWare : IMiddleware
    {

        public ExceptionMiddleWare(ILogger<ExceptionMiddleWare> loger)
        {
            Loger = loger;
        }

        public ILogger Loger { get; }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                Loger.LogError(exception.ToString());

                //exception.Serialize() => My Own Extention Method
                await response.WriteAsync(exception.Serialize());
            }
        }
    }

}

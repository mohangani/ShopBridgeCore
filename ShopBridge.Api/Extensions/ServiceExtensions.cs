using Microsoft.Extensions.DependencyInjection;
using ShopBridge.Api.Controllers;
using ShopBridge.Api.Core;
using ShopBridge.Api.Data;
using ShopBridge.Api.Helpers;
using ShopBridge.Api.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddRequiredServices(this IServiceCollection services) {
            
            services.AddScoped<IProduct, Product>();
            services.AddScoped<IProductDataAccess, ProductDataAccess>();
            services.AddSingleton<IDbHelper, DbHelper>();


            services.AddTransient<ProductValidator, ProductValidator>();
            services.AddTransient<ProductModifyValidator, ProductModifyValidator>();

            services.AddTransient<ProductController, ProductController>();

            

        }


    }
}

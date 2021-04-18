using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopBridge.Api.Extensions
{
    public static class ExceptionExtension
    {
        public static string Serialize(this Exception ex)
        {
            return JsonSerializer.Serialize(new { ExcepptionMessage = ex?.Message }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using ShopBridge.Api.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Tests
{
    public static class ActionResultExtension
    {
        public static string GetErrorMessage(this ActionResult action)
        {
            var result = action as ObjectResult;
            return (string)((IList)result.Value)[0].GetType().GetProperty("ErrorMessage").GetValue(((IList)result.Value)[0]);
        }
    }
}

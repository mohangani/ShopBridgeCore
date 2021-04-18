using ShopBridge.Api.Data;
using ShopBridge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Api.Core
{
    public class Product :IProduct
    {
        private readonly IProductDataAccess _productDataAccess;

        public Product(IProductDataAccess productDataAccess)
        {
            _productDataAccess = productDataAccess;
        }

        public async Task<int?> AddItem(ProductInfoModel productInfoModel)
        {
            return await _productDataAccess.AddItem(productInfoModel);
        }

    }
}

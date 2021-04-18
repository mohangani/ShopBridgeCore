using ShopBridge.Api.Helpers;
using ShopBridge.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridge.Api.Data
{
    public interface IProductDataAccess
    {
        IDbHelper _dbHelper { get; }

        Task<int?> AddItem(ProductInfoModel productInfoModel);
        Task<int?> DeleteItem(int productId);
        Task<int?> ModifyItem(ProductInfoModel productInfoModel);
        Task<List<ProductInfoModel>> ProductItemsList();
        Task<bool> ValidateName(string productName);
        Task<bool> IsProductIdExists(int productId);
    }
}
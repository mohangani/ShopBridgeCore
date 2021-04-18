using ShopBridge.Api.Helpers;
using ShopBridge.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Api.Data
{
    public class ProductDataAccess : IProductDataAccess
    {
        public ProductDataAccess(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IDbHelper _dbHelper { get; }

        public async Task<int?> AddItem(ProductInfoModel productInfoModel)
        {

            var sqlQuery = "insert into tbl_products (name,description,price) output INSERTED.Id values (@name,@description,@price)";

            using var sqlCon = new SqlConnection(_dbHelper.GetConnectionString());

            using var command = new SqlCommand(sqlQuery, sqlCon);

            command.Parameters.AddWithValue("@name", productInfoModel.Name);
            command.Parameters.AddWithValue("@description", productInfoModel.Description);
            command.Parameters.AddWithValue("@price", productInfoModel.Price);

            await sqlCon.OpenAsync();
            var productId = (int?)await command.ExecuteScalarAsync();
            await sqlCon.CloseAsync();

            return productId;
        }

        public async Task<int?> ModifyItem(ProductInfoModel productInfoModel)
        {
            var sqlQuery = @$"update tbl_products 
                              set
                                    [Name] = @name,
	                                [Description] = @description,
	                                [price] = @price
                              where id = @productId";

            using var sqlCon = new SqlConnection(_dbHelper.GetConnectionString());

            using var command = new SqlCommand(sqlQuery, sqlCon);

            command.Parameters.AddWithValue("@name", productInfoModel.Name);
            command.Parameters.AddWithValue("@description", productInfoModel.Description);
            command.Parameters.AddWithValue("@price", productInfoModel.Price);
            command.Parameters.AddWithValue("@productId", productInfoModel.Id);


            await sqlCon.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await sqlCon.CloseAsync();

            return productInfoModel.Id;
        }

        public async Task<int?> DeleteItem(int productId)
        {
            var sqlQuery = @$"update tbl_products 
                              set
                                    [Active] = 0
                              where id = @productId";

            using var sqlCon = new SqlConnection(_dbHelper.GetConnectionString());

            using var command = new SqlCommand(sqlQuery, sqlCon);
            command.Parameters.AddWithValue("@productId", productId);

            await sqlCon.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await sqlCon.CloseAsync();

            return productId;
        }

        public async Task<List<ProductInfoModel>> ProductItemsList()
        {
            var productsList = new List<ProductInfoModel>();

            var sqlQuery = @$"select * from tbl_products 
	                            where Active = 1";

            using var sqlCon = new SqlConnection(_dbHelper.GetConnectionString());

            using var command = new SqlCommand(sqlQuery, sqlCon);

            await sqlCon.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var product = new ProductInfoModel()
                {
                    Id = (int)reader["id"],
                    Name = reader["name"].ToString(),
                    Description = reader["description"].ToString(),
                    Price = (decimal)reader["price"],
                };

                productsList.Add(product);
            }

            await sqlCon.CloseAsync();

            return productsList;
        }

        public async Task<bool> ValidateName(string productName)
        {
            var sqlQuery = @$"SELECT
                                CASE WHEN EXISTS 
                                (
                                     select 1 from tbl_products where name = @name and Active = 1
                                )
                                THEN 0
                                ELSE 1
                            END";

            using var sqlCon = new SqlConnection(_dbHelper.GetConnectionString());

            using var command = new SqlCommand(sqlQuery, sqlCon);

            command.Parameters.AddWithValue("@name", productName);

            await sqlCon.OpenAsync();
            var result = Convert.ToBoolean(await command.ExecuteScalarAsync());

            await sqlCon.CloseAsync();

            return result;
        }

        public async Task<bool> IsProductIdExists(int productId)
        {
            var sqlQuery = @$"SELECT
                                CASE WHEN EXISTS 
                                (
                                     select 1 from tbl_products where id = @id and Active = 1
                                )
                                THEN 1
                                ELSE 0
                            END";

            using var sqlCon = new SqlConnection(_dbHelper.GetConnectionString());

            using var command = new SqlCommand(sqlQuery, sqlCon);

            command.Parameters.AddWithValue("@id", productId);

            await sqlCon.OpenAsync();
            var result = Convert.ToBoolean(await command.ExecuteScalarAsync());

            await sqlCon.CloseAsync();

            return result;
        }
    }
}

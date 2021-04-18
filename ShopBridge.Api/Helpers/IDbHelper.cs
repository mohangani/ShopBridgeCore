using Microsoft.Extensions.Configuration;

namespace ShopBridge.Api.Helpers
{
    public interface IDbHelper
    {
        IConfiguration _config { get; }

        DbHelper CreateDatabaseIfNotExists();
        void CreateTableIfNotExists();
        string GetConnectionString();
    }
}
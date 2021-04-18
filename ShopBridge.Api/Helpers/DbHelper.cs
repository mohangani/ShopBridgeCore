using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Api.Helpers
{
    public class DbHelper : IDbHelper
    {
        public DbHelper(IConfiguration config)
        {
            _config = config;
        }

        public IConfiguration _config { get; }

        public string GetConnectionString()
        {
            return $"{_config.GetConnectionString("ServerConnection")} database={_config.GetValue<string>("DataBaseName")};";
           // return _config.GetConnectionString("ServerConnection");
        }

        private string GetServerConnectionString()
        {
            return _config.GetConnectionString("ServerConnection");
        }


        public DbHelper CreateDatabaseIfNotExists()
        {

            var dbName = _config.GetValue<string>("DataBaseName");

            var query = @$" IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{dbName}')
                              BEGIN
                                CREATE DATABASE [{dbName}]
                              END";


            using var con = new SqlConnection(GetServerConnectionString());
            using var command = new SqlCommand(query, con);

            con.Open();
            command.ExecuteNonQuery();
            con.Close();
            return this;
        }

        public void CreateTableIfNotExists()
        {
            var tableName = _config.GetValue<string>("TableName");

            var query = @$" IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{tableName}' and xtype='U')
                            BEGIN
                                CREATE TABLE {tableName} (
                                    Id INT PRIMARY KEY IDENTITY (1, 1),
                                    Name VARCHAR(100) NOT NULL,
                                    Description VARCHAR(1024),
                                    Price DECIMAL(10,2) NOT NULL,
                                    Active bit NOT NULL
                                )
                            END";

            using var con = new SqlConnection(GetConnectionString());
            using var command = new SqlCommand(query, con);

            con.Open();
            command.ExecuteNonQuery();
            con.Close();
        }
    }
}

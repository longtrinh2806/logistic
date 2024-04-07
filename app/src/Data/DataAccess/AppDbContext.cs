using Data.Configurations;
using Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess
{
    public class AppDbContext
    {
        private IMongoDatabase _database;
        private IMongoClient _client;

        public AppDbContext(IOptions<AppDatabaseSetting> options)
        {
            _client = new MongoClient(options.Value.ConnectionString);
            _database = _client.GetDatabase(options.Value.DatabaseName);
        }
        public IMongoCollection<CangNhapKhau> CangNhapKhau => _database.GetCollection<CangNhapKhau>("cang-nhap-khau");
        public IMongoCollection<KhaiThacCang> KhaiThacCang => _database.GetCollection<KhaiThacCang>("khai-thac-cang");
        public IClientSessionHandle StartSession()
        {
            var session = _client.StartSession();
            return session;
        }
        public void CreateCollectionsIfNotExisted()
        {
            var collectionNames = _database.ListCollectionNames().ToList();

            if (!collectionNames.Any(name => name.Equals("cang-nhap-khau")))
            {
                _database.CreateCollection("cang-nhap-khau");
            }
            if (!collectionNames.Any(name => name.Equals("khai-thac-cang")))
            {
                _database.CreateCollection("khai-thac-cang");
            }
        }
    }
}

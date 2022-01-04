using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Seller.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.API.Data
{
    public class ProductContext:IProductContext
    {
        public ProductContext(IConfiguration configuration)
        {

            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName_Product"));
            ProductContextSeed.SeedData(Products);
            Bids= database.GetCollection<Bid>(configuration.GetValue<string>("DatabaseSettings:CollectionName_Bid"));
        }
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<Bid> Bids { get; }
    }
}

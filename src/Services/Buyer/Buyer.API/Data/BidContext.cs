using Buyer.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.API.Data
{
    public class BidContext : IBidContext
    {
        public BidContext(IConfiguration configuration)
        {

            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            Bids = database.GetCollection<Bid>(configuration.GetValue<string>("DatabaseSettings:CollectionName_Bid"));
            BidContextSeed.SeedData(Bids);
            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName_Product"));
            
        }
        public IMongoCollection<Bid> Bids { get; }
        public IMongoCollection<Product> Products { get; }
    }
}

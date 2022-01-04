using Buyer.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.API.Data
{
    public class BidContextSeed
    {
        public static void SeedData(IMongoCollection<Bid> bidCollection)
        {
            bool existBid = bidCollection.Find(p => true).Any();
            if (!existBid)
            {
                bidCollection.InsertManyAsync(GetPreconfiguredBids());
            }
        }
        private static IEnumerable<Bid> GetPreconfiguredBids()
        {
            return new List<Bid>()
            {
                new Bid()
                {
                    Id="602d2149e773f2a3990b47f1",
                    FirstName="FirstName",
                    LastName="LastName",
                    Address="Address",
                    City="City",
                    State="State",
                    Pin="Pin",
                    Phone="Phone",
                    Email="Email1@email.com",
                    ProductId="602d2149e773f2a3990b47f5",
                    BidAmount=200
                },
                new Bid()
                {
                    Id="602d2149e773f2a3990b47f2",
                    FirstName="FirstName",
                    LastName="LastName",
                    Address="Address",
                    City="City",
                    State="State",
                    Pin="Pin",
                    Phone="Phone",
                    Email="Email1@email.com",
                    ProductId="602d2149e773f2a3990b47f6",
                    BidAmount=300
                },
                new Bid()
                {
                    Id="602d2149e773f2a3990b47f3",
                    FirstName="FirstName",
                    LastName="LastName",
                    Address="Address",
                    City="City",
                    State="State",
                    Pin="Pin",
                    Phone="Phone",
                    Email="Email1@email.com",
                    ProductId="602d2149e773f2a3990b47f7",
                    BidAmount=400
                }
                };
        }
    }
}

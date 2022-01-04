using MongoDB.Driver;
using Seller.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.API.Data
{
    public class ProductContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetPreconfiguredProducts());
            }
        }
        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    ProductName= "Piccaso",
                    ShortDescription="Short Description",
                    DetailedDescription="Detailed Description",
                    Category=CategoryType.Painting,
                    StartingPrice= 100,
                    BidEndDate=DateTime.Today,
                    FirstName ="FirstName",
                    LastName ="LastName",
                    Address ="Address",
                    City ="City",
                    State ="State",
                    Pin ="Pin",
                    Phone ="1234567890",
                    Email ="Email@email.com"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    ProductName= "Socrates",
                    ShortDescription="Short Description",
                    DetailedDescription="Detailed Description",
                    Category=CategoryType.Sculptor,
                    StartingPrice= 100,
                    BidEndDate=DateTime.Today,
                    FirstName ="FirstName",
                    LastName ="LastName",
                    Address ="Address",
                    City ="City",
                    State ="State",
                    Pin ="Pin",
                    Phone ="1234567890",
                    Email ="Email@email.com"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f7",
                    ProductName= "Bangle",
                    ShortDescription="Short Description",
                    DetailedDescription="Detailed Description",
                    Category=CategoryType.Ornament,
                    StartingPrice= 100,
                    BidEndDate= DateTime.Today,
                    FirstName ="FirstName",
                    LastName ="LastName",
                    Address ="Address",
                    City ="City",
                    State ="State",
                    Pin ="Pin",
                    Phone ="1234567890",
                    Email ="Email@email.com"
                }
                };
            }
    }
}

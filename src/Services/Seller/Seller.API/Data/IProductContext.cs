using MongoDB.Driver;
using Seller.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.API.Data
{
    public interface IProductContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<Bid> Bids { get; }
    }
}

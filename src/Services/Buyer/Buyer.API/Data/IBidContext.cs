using Buyer.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.API.Data
{
    public interface IBidContext
    {
        IMongoCollection<Bid> Bids { get; }
        IMongoCollection<Product> Products { get; }
    }
}

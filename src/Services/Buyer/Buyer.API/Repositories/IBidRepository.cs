using Buyer.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.API.Repositories
{
    public interface IBidRepository
    {
        Task CreateBid(Bid bid);
        Task<bool> UpdateBid(string productId,string buyerEmailId,decimal newBidAmount);
        Bid GetBidDetails(string productId, string buyerEmailId);
        Task AddProduct(Product product);
        Task<bool> DeleteProduct(Product product);
        public string ValidateUpdateBid(string productId, string buyerEmailId, decimal newBidAmount, bool isupdate);
        //public Product GetProduct(string Id);
    }
}

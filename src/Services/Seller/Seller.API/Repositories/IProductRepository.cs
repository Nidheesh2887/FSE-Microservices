using Seller.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsforSeller(string sellerEmail);
        Task<IEnumerable<Bid>> GetBids(string productId);
        Task CreateProduct(Product product);
        Task<bool> DeleteProduct(string id);

        Task AddBid(Bid bid);

        Task<bool> UpdateBid(Bid bid);
        Task<IEnumerable<ProductwithBidInfo>> GetProductsnBids();
        public Product GetProduct(string Id);
    }
}

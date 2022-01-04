using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Seller.API.Entities;
using Seller.API.Data;

namespace Seller.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IProductContext _context;

        public ProductRepository(IProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetProductsforSeller(string sellerEmail)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Email, sellerEmail);

            return await _context
                            .Products
                            .Find(filter)
                            .ToListAsync();
        }
        public async Task<IEnumerable<Bid>> GetBids(string productId)
        {
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(b => b.ProductId, productId);

            return await _context
                            .Bids
                            .Find(filter)
                            .ToListAsync();
        }
        public async  Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }
        public async Task<IEnumerable<ProductwithBidInfo>> GetProductsnBids()
        {
            
            var bids = _context.Bids;
            List<ProductwithBidInfo> ls_productwithBidInfos = new List<ProductwithBidInfo>();
            foreach (var p in _context.Products.AsQueryable())
            {
                ProductwithBidInfo productwithBidInfo = new ProductwithBidInfo();
                productwithBidInfo.Id = p.Id;
                productwithBidInfo.ProductName = p.ProductName;
                productwithBidInfo.ShortDescription = p.ShortDescription;
                productwithBidInfo.DetailedDescription = p.DetailedDescription;
                productwithBidInfo.Category = p.Category;
                productwithBidInfo.StartingPrice = p.StartingPrice;
                productwithBidInfo.BidEndDate = p.BidEndDate;
                productwithBidInfo.FirstName = p.FirstName;
                productwithBidInfo.LastName = p.LastName;
                productwithBidInfo.Address = p.Address;
                productwithBidInfo.City = p.City;
                productwithBidInfo.State = p.State;
                productwithBidInfo.Pin = p.Pin;
                productwithBidInfo.Phone = p.Phone;
                productwithBidInfo.Email = p.Email;
                productwithBidInfo.BidsforProd = bids.Find(x => x.ProductId == p.Id).ToList();
                ls_productwithBidInfos.Add(productwithBidInfo);
            }
            return  ls_productwithBidInfos;
                            
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Products
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
        public async Task AddBid(Bid bid)
        {
            await _context.Bids.InsertOneAsync(bid);
        }

        public async Task<bool> UpdateBid(Bid bid)
        {
            var updateResult = await _context.Bids
                                             .UpdateOneAsync(filter: g => g.ProductId == bid.ProductId && g.Email == bid.Email,
                                              Builders<Bid>.Update.Set(rec => rec.BidAmount, bid.BidAmount))
                                             .ConfigureAwait(false);


            return updateResult.IsAcknowledged &&
                    updateResult.ModifiedCount > 0;
        }
        public Product GetProduct(string Id)
        {
            Product product = _context.Products.Find(x => x.Id == Id).FirstOrDefault();
            FilterDefinition<Product> filter = Builders<Product>.Filter.Where(p => p.Id == Id);

            return product;
        }
    }
}

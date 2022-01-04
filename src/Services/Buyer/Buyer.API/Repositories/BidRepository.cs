using Buyer.API.Data;
using Buyer.API.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.API.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly IBidContext _context;

        public BidRepository(IBidContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task CreateBid(Bid bid)
        {
            await _context.Bids.InsertOneAsync(bid);
        }

        public async Task<bool> UpdateBid(string productId, string buyerEmailId, decimal newBidAmount)
        {
            var updateResult = await _context.Bids
                                             .UpdateOneAsync(filter: g => g.ProductId == productId && g.Email == buyerEmailId,
                                              Builders<Bid>.Update.Set(rec => rec.BidAmount, newBidAmount))
                                             .ConfigureAwait(false);

            
            return updateResult.IsAcknowledged &&
                    updateResult.ModifiedCount > 0;
        }
        public Bid GetBidDetails(string productId, string buyerEmailId)
        {
            Bid b = _context.Bids.Find(x => x.ProductId == productId && x.Email == buyerEmailId).FirstOrDefault();
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Where(p => p.ProductId== productId && p.Email==buyerEmailId);

            return b;


        }
        public async Task AddProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }
        public async Task<bool> DeleteProduct(Product product)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);

            DeleteResult deleteResult = await _context
                                                .Products
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public string ValidateUpdateBid(string productId, string buyerEmailId, decimal newBidAmount,bool isupdate)
        {
            Product p = _context.Products.Find(x => x.Id == productId).FirstOrDefault();
            Bid b = _context.Bids.Find(x => x.ProductId == productId && x.Email == buyerEmailId).FirstOrDefault();

            if (p == null)
            {
                return "No such product exeist.";
            }
            else if (p.BidEndDate.Date < DateTime.Today.Date)
            {
                return "Bidding Over";
            }
            else if (p.StartingPrice > newBidAmount)
            {
                return "Bidding amount should be greater than starting price.";
            }
            else if (isupdate)
            {
             if (b == null && isupdate)
                {
                    return "No Such Bid exist";
                }
                else if (b.BidAmount >= newBidAmount && isupdate)
                {
                    return "Bidding amount should be greater than previous amount.";
                }
                else
                    return string.Empty;
            }

            else
                return string.Empty;
        }
    }
}

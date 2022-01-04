using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.API.AzureMessaging.Dto
{
    public class Bid_Azure_Dto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pin { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ProductId { get; set; }
        public decimal BidAmount { get; set; }
        public string Operation { get; set; }
    }
}

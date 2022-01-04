using FluentValidation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.API.Entities
{
    public class Bid
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
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
        
        
    }
    public class BidValidator : AbstractValidator<Bid>
    {
        public BidValidator()
        {
            RuleFor(t => t.FirstName).NotEmpty()
            .MinimumLength(5).WithMessage("FirstName should of minimum length of 5.")
            .MaximumLength(30).WithMessage("FirstName should of maximum length of 30.");
            RuleFor(t => t.LastName).NotEmpty()
            .MinimumLength(5).WithMessage("LastName should of minimum length of 5.")
            .MaximumLength(30).WithMessage("LastName should of maximum length of 30.");
            RuleFor(t => t.Email).EmailAddress();
            RuleFor(t => t.Phone).NotNull().Matches("^[0-9]*$").Length(10).
                WithMessage("Phone number should be numeric of length 10.");
        }
    }
}

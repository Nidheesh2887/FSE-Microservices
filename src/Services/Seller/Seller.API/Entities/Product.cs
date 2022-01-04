using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using FluentValidation;
using System.Runtime.Serialization;

namespace Seller.API.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string DetailedDescription { get; set; }
        public CategoryType Category { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime BidEndDate { get; set; }
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pin { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(t => t.ProductName).NotEmpty()
            .MinimumLength(5).WithMessage("ProductName should of minimum length of 5.")
            .MaximumLength(30).WithMessage("ProductName should of maximum length of 30.");
            RuleFor(t => t.BidEndDate).GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Bidding date must be in future.");
            RuleFor(t => t.StartingPrice).GreaterThan(0).WithMessage("Starting Price should be greater than Zero.");
            RuleFor(x => x.Category).NotNull().IsInEnum();
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
    public enum CategoryType
    {
        [EnumMember(Value = "Painting")]
        Painting,
        [EnumMember(Value = "Sculptor")]
        Sculptor,
        [EnumMember(Value = "Ornament")]
        Ornament
    }
}

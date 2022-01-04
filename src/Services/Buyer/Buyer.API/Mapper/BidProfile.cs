using AutoMapper;
using Buyer.API.AzureMessaging.Dto;
using Buyer.API.Entities;
//using EventBus.Messages.Events;
using Messaging.AzureBus;

namespace Buyer.API.Mapper
{
    public class BidProfile:Profile
    {
        public BidProfile()
        {
            //for rabitMq messaging --start
            //CreateMap<Bid, BiddingEvent>().ReverseMap();
            //CreateMap<Product, ProductAddDelEvent>().ReverseMap();
            //for rabitMq messaging --end

            //for azure ServiceBus --start
            CreateMap<Bid, BiddingMessage>().ReverseMap();
            CreateMap<Product, Product_Azure_Dto>().ReverseMap();
            //for azure ServiceBus --end
        }
    }
}

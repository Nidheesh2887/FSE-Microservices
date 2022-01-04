using AutoMapper;
//using EventBus.Messages.Events;
using Messaging.AzureBus;
using Seller.API.AzureMessaging.Dto;
using Seller.API.Entities;

namespace Seller.API.Mapping
{
    public class BiddingProfile:Profile
    {
        public BiddingProfile()
        {
            //for rabitMq messaging --start
            //CreateMap<Bid, BiddingEvent>().ReverseMap();
            //CreateMap<Product, ProductAddDelEvent>().ReverseMap();
            //for rabitMq messaging --end

            //for azure ServiceBus --start
            CreateMap<Bid, Bid_Azure_Dto>().ReverseMap();
            CreateMap<Product, ProductMessage>().ReverseMap();
            //for azure ServiceBus --end
        }
    }
}

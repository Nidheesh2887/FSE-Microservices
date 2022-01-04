using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Seller.API.Entities;
using Seller.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.API.EventBusConsumer
{
    public class BiddingConsumer : IConsumer<BiddingEvent>
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<BiddingConsumer> _logger;
        private readonly IMapper _mapper;

        public BiddingConsumer(IProductRepository repository, ILogger<BiddingConsumer> logger, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Consume(ConsumeContext<BiddingEvent> context)
        {
            var command = _mapper.Map<Bid>(context.Message);
            if (context.Message.Operation == "INSERT")
            {
                await _repository.AddBid(command);
                _logger.LogInformation("Bid added successfully.Id:" + command.Email + ":" + command.ProductId + ":" + command.BidAmount);
            }
            if (context.Message.Operation == "UPDATE")
            {
                await _repository.UpdateBid(command);
                _logger.LogInformation("Bid updated successfully.Id:" + command.Email + ":" + command.ProductId + ":" + command.BidAmount);
            }
        }
    }
}

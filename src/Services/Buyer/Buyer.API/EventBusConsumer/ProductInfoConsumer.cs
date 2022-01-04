using AutoMapper;
using Buyer.API.Entities;
using Buyer.API.Repositories;
//using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.API.EventBusConsumer
{
    public class ProductInfoConsumer : IConsumer<ProductAddDelEvent>
    {
        private readonly IBidRepository _repository;
        private readonly ILogger<ProductInfoConsumer> _logger;
        private readonly IMapper _mapper;

        public ProductInfoConsumer(IBidRepository repository, ILogger<ProductInfoConsumer> logger, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Consume(ConsumeContext<ProductAddDelEvent> context)
        {
            var command = _mapper.Map<Product>(context.Message);
            if (context.Message.Opertaion == "INSERT")
            {
                await _repository.AddProduct(command);
                _logger.LogInformation("Product added successfully.Id:" + command.Id + ":" + command.ProductName + ":" + command.StartingPrice);
            }
            if (context.Message.Opertaion == "DELETE")
            {
                await _repository.DeleteProduct(command);
                _logger.LogInformation("Product deleted successfully.Id:" + command.Id + ":" + command.ProductName + ":" + command.StartingPrice);
            }
        }
    }
}

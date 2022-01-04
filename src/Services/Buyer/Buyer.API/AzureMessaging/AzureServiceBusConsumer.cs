using AutoMapper;
using Azure.Messaging.ServiceBus;
using Buyer.API.AzureMessaging.Dto;
using Buyer.API.Entities;
using Buyer.API.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buyer.API.AzureMessaging
{
    public class AzureServiceBusConsumer: IAzureServiceBusConsumer
    {
        private readonly IBidRepository _repository;
        private readonly ILogger<AzureServiceBusConsumer> _logger;
        private readonly IMapper _mapper;
        private readonly string servicebusconnectionstring;
        private readonly string productSubscriptionName;
        private readonly string productTopic;
        private readonly IConfiguration _configuration;

        private ServiceBusProcessor productProcessor;

        public AzureServiceBusConsumer(IBidRepository repository, ILogger<AzureServiceBusConsumer> logger, IMapper mapper, IConfiguration configuration)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration=configuration?? throw new ArgumentNullException(nameof(configuration));
            servicebusconnectionstring = _configuration.GetValue<string>("AzureBus:ConnectionString");
            productSubscriptionName = _configuration.GetValue<string>("AzureBus:ProductSubscription");
            productTopic = _configuration.GetValue<string>("AzureBus:ProductTopic");

            var client = new ServiceBusClient(servicebusconnectionstring);
            productProcessor = client.CreateProcessor(productTopic, productSubscriptionName);

        }

        public async Task Start()
        {
            productProcessor.ProcessMessageAsync += OnProductRecieved;
            productProcessor.ProcessErrorAsync += ErrorHandler;
            await productProcessor.StartProcessingAsync();
        }
         Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception.ToString());
            return Task.CompletedTask;
        }
        public async Task Stop()
        {
            await productProcessor.StopProcessingAsync();
            await productProcessor.DisposeAsync();
        }

        private async Task OnProductRecieved(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            Product_Azure_Dto product_dto = JsonConvert.DeserializeObject<Product_Azure_Dto>(body);

            var product = _mapper.Map<Product>(product_dto);
            if (product_dto.Opertaion == "INSERT")
            {
                await _repository.AddProduct(product);
                _logger.LogInformation("Product added successfully.Id:" + product.Id + ":" + product.ProductName + ":" + product.StartingPrice);
            }
            if (product_dto.Opertaion == "DELETE")
            {
                await _repository.DeleteProduct(product);
                _logger.LogInformation("Product deleted successfully.Id:" + product.Id + ":" + product.ProductName + ":" + product.StartingPrice);
            }
        }

    }
}

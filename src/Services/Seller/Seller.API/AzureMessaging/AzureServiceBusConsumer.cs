using AutoMapper;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Seller.API.AzureMessaging.Dto;
using Seller.API.Entities;
using Seller.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seller.API.AzureMessaging
{
    
        public class AzureServiceBusConsumer : IAzureServiceBusConsumer
        {
        private readonly IProductRepository _repository;
        private readonly ILogger<AzureServiceBusConsumer> _logger;
            private readonly IMapper _mapper;
            private readonly string servicebusconnectionstring;
            private readonly string bidSubscriptionName;
            private readonly string bidTopic;
            private readonly IConfiguration _configuration;

            private ServiceBusProcessor bidProcessor;

            public AzureServiceBusConsumer(IProductRepository repository, ILogger<AzureServiceBusConsumer> logger, IMapper mapper, IConfiguration configuration)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
                servicebusconnectionstring = _configuration.GetValue<string>("AzureBus:ConnectionString");
            bidSubscriptionName = _configuration.GetValue<string>("AzureBus:BidSubscription");
            bidTopic = _configuration.GetValue<string>("AzureBus:BidTopic");

                var client = new ServiceBusClient(servicebusconnectionstring);
            bidProcessor = client.CreateProcessor(bidTopic, bidSubscriptionName);

            }

            public async Task Start()
            {
            bidProcessor.ProcessMessageAsync += OnBidRecieved;
            bidProcessor.ProcessErrorAsync += ErrorHandler;
                await bidProcessor.StartProcessingAsync();
            }
            Task ErrorHandler(ProcessErrorEventArgs args)
            {
                _logger.LogError(args.Exception.ToString());
                return Task.CompletedTask;
            }
            public async Task Stop()
            {
                await bidProcessor.StopProcessingAsync();
                await bidProcessor.DisposeAsync();
            }

            private async Task OnBidRecieved(ProcessMessageEventArgs args)
            {
                var message = args.Message;
                var body = Encoding.UTF8.GetString(message.Body);
                Bid_Azure_Dto bid_dto = JsonConvert.DeserializeObject<Bid_Azure_Dto>(body);

                var bid = _mapper.Map<Bid>(bid_dto);
                if (bid_dto.Operation == "INSERT")
                {
                    await _repository.AddBid(bid);
                    _logger.LogInformation("Bid added successfully.Id:" + bid.Email + ":" + bid.ProductId + ":" + bid.BidAmount);
            }
                if (bid_dto.Operation == "UPDATE")
                {
                await _repository.UpdateBid(bid);
                _logger.LogInformation("Bid updated successfully.Id:" + bid.Email + ":" + bid.ProductId + ":" + bid.BidAmount);
            }
            }

        }
    }


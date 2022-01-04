using AutoMapper;
using Buyer.API.Entities;
using Buyer.API.Repositories;
//using EventBus.Messages.Events;
using MassTransit;
using Messaging.AzureBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.API.Controllers
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        private readonly IBidRepository _repository;
        private readonly ILogger<BuyerController> _logger;        
        private readonly IMapper _mapper;
        //for Azure ServiceBus-start
        private readonly IMessageBus _messageBus;
        //for Azure ServiceBus-end


        //for rabitMq messaging --start        
        //private readonly IPublishEndpoint _publishEndpoint;
        //public BuyerController(IBidRepository repository, ILogger<BuyerController> logger, IPublishEndpoint publishEndpoint, IMapper mapper)
        //{
        //    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        //    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //    _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        //    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        //}
        //for rabitMq messaging --end  

        //for Azure ServiceBus-start
        public BuyerController(IBidRepository repository, ILogger<BuyerController> logger, IMessageBus messageBus, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageBus = messageBus ?? throw new ArgumentNullException(nameof(_messageBus));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        //for Azure ServiceBus-end


        [Route("[action]")]
        [HttpPost]
        [ActionName("place-bid")]        
        [ProducesResponseType(typeof(Bid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateBid([FromBody] Bid bid)
        {
            string validationmsgs = _repository.ValidateUpdateBid(bid.ProductId, bid.Email, bid.BidAmount, false);
            if (validationmsgs != string.Empty)
                return BadRequest(validationmsgs);
            await _repository.CreateBid(bid);

            //for rabitMq messaging --start  
            //var eventMessage = _mapper.Map<BiddingEvent>(bid);
            //eventMessage.Operation = "INSERT";
            //await _publishEndpoint.Publish(eventMessage);
            //for rabitMq messaging --end

            //for azure ServiceBus --start
            var eventMessage = _mapper.Map<BiddingMessage>(bid);
            eventMessage.Operation = "INSERT";
            await _messageBus.PublishMessage(eventMessage, "bidtopic");
            //for azure ServiceBus --end

            return Ok(bid);            
        }
        [Route("[action]/{productId}/{buyerEmailId}/{newBidAmount}")]
        [HttpPut]
        [ActionName("update-bid")]
        [ProducesResponseType(typeof(Bid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBid(string productId, string buyerEmailId, decimal newBidAmount)
        {
            string validationmsgs = _repository.ValidateUpdateBid(productId, buyerEmailId, newBidAmount,true);
            if(validationmsgs!=string.Empty)
                return BadRequest(validationmsgs);
            await _repository.UpdateBid(productId,buyerEmailId,newBidAmount);
            var updatedbid = _repository.GetBidDetails(productId, buyerEmailId);

            //for rabitMq messaging --start
            //var eventMessage = _mapper.Map<BiddingEvent>(updatedbid);
            //eventMessage.Operation = "UPDATE";
            //await _publishEndpoint.Publish(eventMessage);
            //for rabitMq messaging --end

            //for azure ServiceBus --start
            var eventMessage = _mapper.Map<BiddingMessage>(updatedbid);
            eventMessage.Operation = "UPDATE";
            await _messageBus.PublishMessage(eventMessage, "bidtopic");
            //for azure ServiceBus --end

            return Ok();
        }
    }
}

using AutoMapper;
//using EventBus.Messages.Events;
using MassTransit;
using Messaging.AzureBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Seller.API.Entities;
using Seller.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<SellerController> _logger;
        private readonly IMapper _mapper;

        //for Azure ServiceBus-start
        private readonly IMessageBus _messageBus;
        //for Azure ServiceBus-end

        //for rabitMq messaging --start
        //private readonly IPublishEndpoint _publishEndpoint;        

        //public SellerController(IProductRepository repository, ILogger<SellerController> logger, IPublishEndpoint publishEndpoint, IMapper mapper)
        //{
        //    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        //    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //    _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        //    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        //}
        //for rabitMq messaging --end
        //for Azure ServiceBus-start
        public SellerController(IProductRepository repository, ILogger<SellerController> logger, IMessageBus messageBus, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageBus = messageBus ?? throw new ArgumentNullException(nameof(_messageBus));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        //for Azure ServiceBus-end
        [Route("[action]/{sellerEmail}", Name = "GetProductsforSeller")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsforSeller(string sellerEmail)
        {
            var products = await _repository.GetProductsforSeller(sellerEmail);
            return Ok(products);
        }
        [Route("[action]/{productId}")]
        [HttpGet]
        [ActionName("show-bids")]
        [ProducesResponseType(typeof(IEnumerable<Bid>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidsforProduct(string productId)
        {
            var products = await _repository.GetProductsforSeller(productId);
            return Ok(products);
        }

        [Route("[action]", Name = "GetProductsnBids")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ProductwithBidInfo>>> GetProductsnBids()
        {
            var productwithBidInfo = await _repository.GetProductsnBids();
            return Ok(productwithBidInfo);
        }

        [Route("[action]")]
        [HttpPost]
        [ActionName("add-Product")]        
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _repository.CreateProduct(product);
            //for rabitMq messaging --start
            //var eventMessage = _mapper.Map<ProductAddDelEvent>(product);
            //eventMessage.Opertaion = "INSERT";
            //await _publishEndpoint.Publish(eventMessage);
            //for rabitMq messaging --end


            
            //for azure ServiceBus --start
            var eventMessage = _mapper.Map<ProductMessage>(product);
            eventMessage.Opertaion = "INSERT";
            await _messageBus.PublishMessage(eventMessage, "sellertopic");
            //for azure ServiceBus --end
            return CreatedAtRoute("GetProductsforSeller", new { sellerEmail = product.Email }, product);
        }

        
        [HttpDelete("{id:length(24)}")]        
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string id)
        {
            var product = _repository.GetProduct(id);
            //for rabitMq messaging --start
            //var eventMessage = _mapper.Map<ProductAddDelEvent>(product);
            //eventMessage.Opertaion = "DELETE";
            //await _publishEndpoint.Publish(eventMessage);
            //for rabitMq messaging --end



            //for azure ServiceBus --start
            var eventMessage = _mapper.Map<ProductMessage>(product);
            eventMessage.Opertaion = "DELETE";
            await _messageBus.PublishMessage(eventMessage, "sellertopic");
            //for azure ServiceBus --end

            return Ok(await _repository.DeleteProduct(id));
        }
    }
}

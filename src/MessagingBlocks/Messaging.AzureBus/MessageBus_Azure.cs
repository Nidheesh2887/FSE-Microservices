using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Messaging.AzureBus
{
    public class MessageBus_Azure : IMessageBus
    {
        private string connectionstring = "Endpoint=sb://biddingservicenamespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xqK+XTkgfhsqH0DgFO74DNqzsyWJSS8/kJHR6oF8fNY=";
        public async Task PublishMessage(BaseMessage message, string Topic)
        {
            await using var client = new ServiceBusClient(connectionstring);
            ServiceBusSender sender = client.CreateSender(Topic);
            
            var jsonmessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalmessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonmessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };
            await sender.SendMessageAsync(finalmessage);
            await client.DisposeAsync();
        }
    }
}

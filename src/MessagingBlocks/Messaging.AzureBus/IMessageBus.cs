using System;
using System.Threading.Tasks;

namespace Messaging.AzureBus
{
    public interface IMessageBus
    {
        Task PublishMessage(BaseMessage message, string Topic);
    }
}

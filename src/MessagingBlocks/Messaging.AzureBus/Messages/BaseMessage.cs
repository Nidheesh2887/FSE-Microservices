using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.AzureBus
{
    public class BaseMessage
    {
        public int EventId { get; set; }
        public DateTime MessageCreated { get; set; }
    }
}

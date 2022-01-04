using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
	public class IntegrationBaseEvent
	{
		public IntegrationBaseEvent()
		{
			EventId = Guid.NewGuid();
			CreationDate = DateTime.UtcNow;
		}

		public IntegrationBaseEvent(Guid eventId, DateTime createDate)
		{
			EventId = eventId;
			CreationDate = createDate;
		}

		public Guid EventId { get; private set; }

		public DateTime CreationDate { get; private set; }
	}
}

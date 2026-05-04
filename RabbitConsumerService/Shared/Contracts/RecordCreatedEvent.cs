using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumerService.Shared.Contracts
{
    public class RecordCreatedEvent
    {
        public Guid CorrelationId { get; set; }
        public int Version { get; set; } = 1;
        public RecordDtoResponse Data { get; set; } = default!;
    }
}

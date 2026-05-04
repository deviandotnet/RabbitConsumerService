using RabbitConsumerService.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumerService.Models
{
    public class ProcessedMessages
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public DateTime ProcessedAt { get; set; }
        public Status Status { get; set; }

    }
}

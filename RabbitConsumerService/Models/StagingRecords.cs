using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumerService.Models
{
    public class StagingRecords
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RecordNumber { get; set; }
        public Guid CorrelationId { get; set; }
        public DateTime RecievedAt { get; set; }
        public Status Status { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

}

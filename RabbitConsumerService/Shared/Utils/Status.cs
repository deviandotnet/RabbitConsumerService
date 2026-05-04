using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumerService.Shared.Utils
{
    public enum Status
    {
        Pending,
        InProgress,
        Processed,
        Failed,
        Success
    }
}

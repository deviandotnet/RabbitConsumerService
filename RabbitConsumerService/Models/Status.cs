using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumerService.Models
{
    public enum Status
    {
        Pending,
        Processed,
        Failed,
        Success
    }
}

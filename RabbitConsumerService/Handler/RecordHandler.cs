using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RabbitConsumerService.Models;
using RabbitConsumerService.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumerService.Handler
{
    public class RecordHandler
    {
        private readonly InternalDatabaseContext _context;
        private readonly ILogger<RecordHandler> _logger;

        public RecordHandler(InternalDatabaseContext context, ILogger<RecordHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task HandleAsync(RecordCreatedEvent recordEvent)
        {
            if(await _context.ProcessedMessages.AnyAsync(x => x.CorrelationId == recordEvent.CorrelationId))
            {
                _logger.LogInformation($"{recordEvent.CorrelationId}: Message already processed, skip");
                return;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //var entity = new StagingRecords
                //{
                //    Id = recordEvent.Data.Id,
                //    Name = recordEvent.Data.Name,
                //    Description = recordEvent.Data.Description,
                //    RecordNumber = recordEvent.Data.RecordNumber,
                //    CorrelationId = recordEvent.CorrelationId,
                //    RecievedAt = DateTime.UtcNow,
                //    Status = Shared.Utils.Status.Processed
                //};

                var newRecord = new Records
                {
                    Id = recordEvent.Data.Id,
                    Name = recordEvent.Data.Name,
                    Description = recordEvent.Data.Description,
                    RecordNumber = recordEvent.Data.RecordNumber,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,

                };

                await _context.Records.AddAsync(newRecord);

                await _context.ProcessedMessages.AddAsync(new ProcessedMessages
                {
                    CorrelationId = recordEvent.CorrelationId,
                    ProcessedAt = DateTime.UtcNow,
                    Status = Shared.Utils.Status.Processed
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        
    }
}

using Microsoft.EntityFrameworkCore;
using RabbitConsumerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumerService
{
    public class InternalDatabaseContext(DbContextOptions<InternalDatabaseContext> options) : DbContext(options)
    {
        public DbSet<Records> Records => Set<Records>();
        public DbSet<ProcessedMessages> ProcessedMessages => Set<ProcessedMessages>();

        public DbSet<StagingRecords> StagingRecords => Set<StagingRecords>();

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Records>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.RecordNumber).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
            });


            modelBuilder.Entity<ProcessedMessages>(entity =>
            {
                entity.HasIndex(x => x.CorrelationId).IsUnique();
            });
        }
    }
}

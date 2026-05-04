
using Microsoft.EntityFrameworkCore;

namespace RabbitConsumerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddDbContext<InternalDatabaseContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("InternalDatabase"));
            });

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}
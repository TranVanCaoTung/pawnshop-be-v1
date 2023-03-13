using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BackGroundWorkerService : BackgroundService
    {
        private readonly ILogger<BackGroundWorkerService> logger;
        public BackGroundWorkerService(ILogger<BackGroundWorkerService> logger)
        {
            this.logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Hosted service starting");

            return Task.Factory.StartNew(async () =>
            {
                // loop until a cancalation is requested
                while (!cancellationToken.IsCancellationRequested)
                {
                    logger.LogInformation("Hosted service executing - {0}", DateTime.Now);
                    try
                    {
                        // wait for 3 seconds
                        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
                    }
                    catch (OperationCanceledException) { }
                }
            }, cancellationToken);
        }
    }
}

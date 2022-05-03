﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unite.Specimens.Feed.Web.Configuration.Options;
using Unite.Specimens.Feed.Web.Handlers;

namespace Unite.Specimens.Feed.Web.HostedServices
{
    public class SpecimensIndexingHostedService : BackgroundService
    {
        private readonly SpecimensIndexingOptions _options;
        private readonly SpecimensIndexingHandler _handler;
        private readonly ILogger _logger;

        public SpecimensIndexingHostedService(
            SpecimensIndexingOptions options,
            SpecimensIndexingHandler handler,
            ILogger<SpecimensIndexingHostedService> logger)
        {
            _options = options;
            _handler = handler;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Indexing service started");

            cancellationToken.Register(() => _logger.LogInformation("Indexing service stopped"));

            // Delay 5 seconds to let the web api start working
            await Task.Delay(5000, cancellationToken);

            try
            {
                _handler.Prepare();
            }
            catch (Exception exception)
            {
                LogError(exception);
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _handler.Handle(_options.BucketSize);
                }
                catch (Exception exception)
                {
                    LogError(exception);
                }
                finally
                {
                    await Task.Delay(_options.Interval, cancellationToken);
                }
            }
        }

        private void LogError(Exception exception)
        {
            _logger.LogError(exception.Message);

            if (exception.InnerException != null)
            {
                _logger.LogError(exception.InnerException.Message);
            }
        }
    }
}

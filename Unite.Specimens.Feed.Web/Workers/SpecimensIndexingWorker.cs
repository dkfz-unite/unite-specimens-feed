﻿using Unite.Essentials.Extensions;
using Unite.Specimens.Feed.Web.Configuration.Options;
using Unite.Specimens.Feed.Web.Handlers;

namespace Unite.Specimens.Feed.Web.Workers;

public class SpecimensIndexingWorker : BackgroundService
{
    private readonly SpecimensIndexingOptions _options;
    private readonly SpecimensIndexingHandler _handler;
    private readonly ILogger _logger;

    public SpecimensIndexingWorker(
        SpecimensIndexingOptions options,
        SpecimensIndexingHandler handler,
        ILogger<SpecimensIndexingWorker> logger)
    {
        _options = options;
        _handler = handler;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Indexing worker started");

        cancellationToken.Register(() => _logger.LogInformation("Indexing worker stopped"));

        // Delay 5 seconds to let the web api start working
        await Task.Delay(5000, cancellationToken);

        try
        {
            await _handler.Prepare();
        }
        catch (Exception exception)
        {
            _logger.LogError("{error}", exception.GetShortMessage());
        }

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _handler.Handle(_options.BucketSize);
            }
            catch (Exception exception)
            {
                _logger.LogError("{error}", exception.GetShortMessage());
            }
            finally
            {
                await Task.Delay(10000, cancellationToken);
            }
        }
    }
}

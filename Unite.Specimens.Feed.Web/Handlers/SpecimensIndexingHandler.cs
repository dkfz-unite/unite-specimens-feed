﻿using System.Diagnostics;
using Unite.Data.Context.Services.Tasks;
using Unite.Data.Entities.Tasks.Enums;
using Unite.Indices.Context;
using Unite.Indices.Entities.Specimens;
using Unite.Specimens.Indices.Services;

namespace Unite.Specimens.Feed.Web.Handlers;

public class SpecimensIndexingHandler
{
    private readonly TasksProcessingService _taskProcessingService;
    private readonly SpecimenIndexCreationService _indexCreationService;
    private readonly IIndexService<SpecimenIndex> _indexingService;
    private readonly ILogger _logger;


    public SpecimensIndexingHandler(
        TasksProcessingService taskProcessingService,
        SpecimenIndexCreationService indexCreationService,
        IIndexService<SpecimenIndex> indexingService,
        ILogger<SpecimensIndexingHandler> logger)
    {
        _taskProcessingService = taskProcessingService;
        _indexCreationService = indexCreationService;
        _indexingService = indexingService;
        _logger = logger;
    }

    public void Prepare()
    {
        _indexingService.UpdateIndex().GetAwaiter().GetResult();
    }

    public void Handle(int bucketSize)
    {
        ProcessSpecimenIndexingTasks(bucketSize);
    }


    private void ProcessSpecimenIndexingTasks(int bucketSize)
    {
        var stopwatch = new Stopwatch();

        _taskProcessingService.Process(IndexingTaskType.Specimen, bucketSize, (tasks) =>
        {
            if (_taskProcessingService.HasSubmissionTasks() || _taskProcessingService.HasAnnotationTasks())
            {
                return false;
            }

            _logger.LogInformation("Indexing {number} specimens", tasks.Length);

            stopwatch.Restart();

            var grouped = tasks.DistinctBy(task => task.Target);

            var indices = grouped.Select(task =>
            {
                var id = int.Parse(task.Target);

                var index = _indexCreationService.CreateIndex(id);

                return index;

            }).ToArray();

            _indexingService.AddRange(indices);

            stopwatch.Stop();

            _logger.LogInformation("Indexing of {number} specimens completed in {time}s", tasks.Length, Math.Round(stopwatch.Elapsed.TotalSeconds, 2));

            return true;
        });
    }
}


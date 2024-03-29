using Microsoft.AspNetCore.Mvc;
using Unite.Specimens.Feed.Data;
using Unite.Specimens.Feed.Data.Exceptions;
using Unite.Specimens.Feed.Web.Models.Converters;
using Unite.Specimens.Feed.Web.Services;

namespace Unite.Specimens.Feed.Web.Controllers;

public abstract class SpecimensControllerBase : Controller
{
    protected readonly SpecimensDataWriter _dataWriter;
    protected readonly SpecimenIndexingTasksService _indexingTaskService;
    protected readonly ILogger _logger;

    protected readonly SpecimenDataModelsConverter _converter;


    public SpecimensControllerBase(
        SpecimensDataWriter dataWriter,
        SpecimenIndexingTasksService indexingTaskService,
        ILogger<SpecimensControllerBase> logger)
    {
        _dataWriter = dataWriter;
        _indexingTaskService = indexingTaskService;
        _logger = logger;

        _converter = new SpecimenDataModelsConverter();
    }


    protected virtual IActionResult PostData(Data.Models.SpecimenModel[] models)
    {
        try
        {
            _dataWriter.SaveData(models, out var audit);

            _logger.LogInformation("{audit}", audit.ToString());

            _indexingTaskService.PopulateTasks(audit.Specimens);

            return Ok();
        }
        catch (NotFoundException exception)
        {
            _logger.LogWarning("{error}", exception.Message);

            return BadRequest(exception.Message);
        }
    }
}

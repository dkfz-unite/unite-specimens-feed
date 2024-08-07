using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Specimens.Feed.Data;
using Unite.Specimens.Feed.Web.Configuration.Constants;
using Unite.Specimens.Feed.Web.Services;
using Unite.Specimens.Indices.Services;

namespace Unite.Specimens.Feed.Web.Controllers;

[Route("api/entry")]
[Authorize(Policy = Policies.Data.Writer)]
public class SpecimenController : Controller
{
    protected readonly SpecimensRemover _dataRemover;
    protected readonly SpecimenIndexRemover _indexRemover;
    protected readonly SpecimenIndexingTasksService _tasksService;
    protected readonly ILogger _logger;


    public SpecimenController(
        SpecimensRemover dataRemover,
        SpecimenIndexRemover indexRemover,
        SpecimenIndexingTasksService tasksService,
        ILogger<SpecimenController> logger)
    {
        _dataRemover = dataRemover;
        _indexRemover = indexRemover;
        _tasksService = tasksService;
        _logger = logger;
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var image = _dataRemover.Find(id);

        if (image != null)
        {
            _tasksService.ChangeStatus(false);
            _tasksService.PopulateTasks([id]);
            _indexRemover.DeleteIndex(id);
            _dataRemover.SaveData(image);
            _tasksService.ChangeStatus(true);

            _logger.LogInformation("Specimen `{id}` has been deleted", id);

            return Ok();
        }
        else
        {
            _logger.LogWarning("Wrong attempt to delete specimen `{id}`", id);

            return NotFound();
        }
    }
}

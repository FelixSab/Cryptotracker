using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace Cryptotracker.API.Controllers;

[ApiController]
[Route("[controller]")]
public class JobsController(ISchedulerFactory _schedulerFactory) : ControllerBase
{
    [HttpPost("trigger/{jobKey}")]
    public async Task<IActionResult> TriggerJob(string jobKey)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.TriggerJob(new JobKey(jobKey));
        return Ok($"Job {jobKey} triggered");
    }
}

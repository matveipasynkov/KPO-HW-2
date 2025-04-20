using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZooManagement.Application.DTOs;
using ZooManagement.Application.Services;

namespace ZooManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IZooStatisticsService _statsService;

    public StatisticsController(IZooStatisticsService statsService)
    {
        _statsService = statsService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ZooStatisticsDto), 200)]
    public async Task<ActionResult<ZooStatisticsDto>> GetStatistics()
    {
        var stats = await _statsService.GetStatisticsAsync();
        return Ok(stats);
    }
}

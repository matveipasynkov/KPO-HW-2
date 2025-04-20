using ZooManagement.Application.DTOs;

namespace ZooManagement.Application.Services;

public interface IZooStatisticsService
{
    Task<ZooStatisticsDto> GetStatisticsAsync();
}

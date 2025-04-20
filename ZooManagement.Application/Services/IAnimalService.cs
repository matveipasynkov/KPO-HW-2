using ZooManagement.Domain.Entities;

namespace ZooManagement.Application.Services;

public interface IAnimalService
{
    Task<Animal?> GetAnimalByIdAsync(Guid id);
    Task<IEnumerable<Animal>> GetAllAnimalsAsync();
    Task AddAnimalAsync(Animal animal);
    Task UpdateAnimalAsync(Animal animal);
    Task DeleteAnimalAsync(Guid id);
}

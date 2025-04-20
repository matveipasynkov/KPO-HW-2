using ZooManagement.Domain.Entities;

namespace ZooManagement.Domain.Interfaces;

public interface IEnclosureRepository
{
    Task<Enclosure?> GetByIdAsync(Guid id);
    Task<IEnumerable<Enclosure>> GetAllAsync();
    Task AddAsync(Enclosure enclosure);
    Task UpdateAsync(Enclosure enclosure);
    Task DeleteAsync(Guid id);
}

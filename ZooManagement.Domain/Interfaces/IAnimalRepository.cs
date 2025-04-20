using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZooManagement.Domain.Entities;

namespace ZooManagement.Domain.Interfaces;

public interface IAnimalRepository
{
    Task<Animal?> GetByIdAsync(Guid id);
    Task<IEnumerable<Animal>> GetAllAsync();
    Task AddAsync(Animal animal);
    Task UpdateAsync(Animal animal);
    Task DeleteAsync(Guid id);
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZooManagement.Domain.Entities;

namespace ZooManagement.Application.Services;

public interface IEnclosureService
{
    Task<Enclosure?> GetEnclosureByIdAsync(Guid id);
    Task<IEnumerable<Enclosure>> GetAllEnclosuresAsync();
    Task AddEnclosureAsync(Enclosure enclosure);
    Task UpdateEnclosureAsync(Enclosure enclosure);
    Task DeleteEnclosureAsync(Guid id);
}

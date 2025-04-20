using System;
using System.Threading.Tasks;

namespace ZooManagement.Application.Services;

public interface IAnimalTransferService
{
    Task TransferAnimalAsync(Guid animalId, Guid targetEnclosureId);
}

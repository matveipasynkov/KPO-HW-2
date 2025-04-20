using System;

namespace ZooManagement.Domain.Events;

public class AnimalMovedEvent
{
    public Guid AnimalId { get; }
    public Guid? SourceEnclosureId { get; }
    public Guid TargetEnclosureId { get; }
    public DateTime Timestamp { get; }

    public AnimalMovedEvent(Guid animalId, Guid? sourceEnclosureId, Guid targetEnclosureId)
    {
        if (animalId == Guid.Empty)
            throw new ArgumentException("AnimalId cannot be empty.", nameof(animalId));
        if (targetEnclosureId == Guid.Empty)
            throw new ArgumentException("TargetEnclosureId cannot be empty.", nameof(targetEnclosureId));

        AnimalId = animalId;
        SourceEnclosureId = sourceEnclosureId;
        TargetEnclosureId = targetEnclosureId;
        Timestamp = DateTime.UtcNow;
    }
}

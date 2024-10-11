using CaseLocaliza.Models.Abstractions;
using static CaseLocaliza.Models.Vehicle;

namespace CaseLocaliza.Models;

public class VehicleAudit : Entity
{
    public SituationType Situation { get; private set; }
    public Guid VehicleId { get; private set; }
    public bool IsUsed { get; private set; } = true;

    public VehicleAudit() { }

    public VehicleAudit(SituationType situation, Guid vehicleId)
    {
        Situation = situation;
        VehicleId = vehicleId;
    }

    public void UpdateUse()
    {
        IsUsed = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public static VehicleAudit Create(SituationType situation, Guid vehicleId)
            => new(situation, vehicleId);
}

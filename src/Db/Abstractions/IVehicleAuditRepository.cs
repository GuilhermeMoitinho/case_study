using CaseLocaliza.Models;
using static CaseLocaliza.Models.Vehicle;

namespace CaseLocaliza.Db.Abstractions;

public interface IVehicleAuditRepository
{
    Task AddAsync(VehicleAudit vehicleAudit);

    Task<List<VehicleAudit>> GetAuditsByVehicleIdAsync(Guid vehicleId);

    Task RemoveLastAuditVehicle(Guid vehicleId);

    Task<SituationType> LastSituationAuditVehicle(Guid vehicleId);

    Task<VehicleAudit> GetLastAuditByVehicleIdAsync(Guid vehicleId);

    Task UpdateAsync(VehicleAudit vehicleAudit);
}

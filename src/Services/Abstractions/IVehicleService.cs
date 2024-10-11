using CaseLocaliza.Models;
using System.Collections.Generic;
using static CaseLocaliza.Models.Vehicle;

namespace CaseLocaliza.Services.Abstractions;

public interface IVehicleService
{
    Task<Guid> AddVehicleAsync(Vehicle vehicle);

    Task<Vehicle> GetVehicleByIdAsync(Guid id);

    Task MovementVehicleAsync(Guid id, SituationType situationType);

    Task ReturnMovementVehicleAsync(Guid id);

    Task<IEnumerable<Vehicle>> GetAllVehicles(int offset, int limit);
}

using CaseLocaliza.Models;

namespace CaseLocaliza.Db.Abstractions;

public interface IVehicleRepository
{
    Task<Guid> AddVehicleAsync(Vehicle vehicle);

    Task<Vehicle> GetVehicleByIdAsync(Guid id);

    Task MovementVehicleAsync(Vehicle vehicle);

    Task<IEnumerable<Vehicle>> GetAllVehicles(int offset, int limit);
}

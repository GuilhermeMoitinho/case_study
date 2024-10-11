using CaseLocaliza.Db.Abstractions;
using CaseLocaliza.Db.Context;
using CaseLocaliza.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseLocaliza.Db.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _context;

    public VehicleRepository(AppDbContext context) { _context = context; }

    public async Task<Guid> AddVehicleAsync(Vehicle vehicle) { await _context.Vehicles.AddAsync(vehicle); return vehicle.Id; }

    public async Task<Vehicle> GetVehicleByIdAsync(Guid id) => await _context.Vehicles.FirstOrDefaultAsync(x => x.Id == id);

    public async Task MovementVehicleAsync(Vehicle vehicle) => _context.Vehicles.Update(vehicle);

    public async Task<IEnumerable<Vehicle>> GetAllVehicles(int offset, int limit) => await _context.Vehicles.Skip(offset).Take(limit).ToListAsync();
}

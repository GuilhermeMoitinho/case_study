using CaseLocaliza.Db.Abstractions;
using CaseLocaliza.Db.Context;
using CaseLocaliza.Models;
using Microsoft.EntityFrameworkCore;
using static CaseLocaliza.Models.Vehicle;

namespace CaseLocaliza.Db.Repositories;

public class VehicleAuditRepository : IVehicleAuditRepository
{
    private readonly AppDbContext _context;

    public VehicleAuditRepository(AppDbContext context) => _context = context;
        
    public async Task AddAsync(VehicleAudit vehicleAudit) => await _context.VehicleAudits.AddAsync(vehicleAudit);
        
    public async Task<List<VehicleAudit>> GetAuditsByVehicleIdAsync(Guid vehicleId) => await _context.VehicleAudits.ToListAsync();    

    public async Task RemoveLastAuditVehicle(Guid vehicleId)
    {
        var lastVehicleAudit = await _context.VehicleAudits.Where(x => x.VehicleId == vehicleId).OrderBy(x => x.CreatedAt).LastAsync();

        lastVehicleAudit.UpdateUse();

        _context.VehicleAudits.Update(lastVehicleAudit);
    }

    public async Task<VehicleAudit> GetLastAuditByVehicleIdAsync(Guid vehicleId)
    {
        return await _context.VehicleAudits.Where(x => x.VehicleId == vehicleId && x.IsUsed == true).OrderBy(x => x.CreatedAt).LastAsync();
    }

    public async Task UpdateAsync(VehicleAudit vehicleAudit)
    {
        _context.VehicleAudits.Update(vehicleAudit);
        await _context.SaveChangesAsync();
    }

    public async Task<SituationType> LastSituationAuditVehicle(Guid vehicleId)
    {
        var vehicle = await GetLastAuditByVehicleIdAsync(vehicleId);

        return vehicle.Situation;
    }
}


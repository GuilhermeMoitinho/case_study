using CaseLocaliza.Db.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseLocaliza.Controllers;

[Route("api/v1/audits")]
[ApiController]
public class VehicleAuditController(AppDbContext context) : ControllerBase
{
    [HttpGet("vehicle/{id:Guid}")]
    public async Task<IActionResult> GetAuditsVehicle(Guid id, int offset = 0, int limit = 15)
    {
        var audits = await context.VehicleAudits.Skip(offset).Take(15).Where(x => x.VehicleId == id).ToListAsync();
        if (audits is null) return NotFound("Audits Not found!");
        return Ok(audits);
    }
}

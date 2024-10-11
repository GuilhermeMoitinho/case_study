using CaseLocaliza.DTOs;
using CaseLocaliza.Models;
using CaseLocaliza.Notifications.Abstractions;
using CaseLocaliza.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using static CaseLocaliza.Models.Vehicle;

namespace CaseLocaliza.Controllers;

/// <summary>
/// Manages vehicle-related operations.
/// </summary>
[Route("api/v1/vehicles")]
[Produces("application/json")]
[Tags("Vehicles")]
[ApiController]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    private readonly INotifier _notifier;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehiclesController"/> class.
    /// </summary>
    /// <param name="vehicleService">The vehicle service used to manage vehicles.</param>
    public VehiclesController(IVehicleService vehicleService, INotifier notifier)
    {
        _vehicleService = vehicleService;
        _notifier = notifier;
    }

    /// <summary>
    /// [DONE] Adds a new vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle to be added.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddVehicleAsync(VehicleRequest vehicle)
    {
        if (vehicle == null)
            return BadRequest("Vehicle cannot be null.");

       var vehicleId = await _vehicleService.AddVehicleAsync(vehicle.IntoEntity());

        if (vehicleId == Guid.Empty) return BadRequest();

        if (_notifier.HasNotification())
            return BadRequest(new Response(_notifier.GetNotification().Select(x => x.Message), false));

        var response = new Response(vehicleId, true);

       return CreatedAtAction(nameof(GetVehicleByIdAsync), new { id = vehicleId }, response);
    }

    /// <summary>
    /// [DONE] Updates the situation of a vehicle.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle.</param>
    /// <param name="situationType">The new situation type for the vehicle.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [HttpPatch("movement/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MovementVehicleAsync(Guid id, SituationType situationType)
    {
        await _vehicleService.MovementVehicleAsync(id, situationType);

        if (_notifier.HasNotification())
            return BadRequest(new Response(_notifier.GetNotification().Select(x => x.Message), false));

        return NoContent();
    }

    /// <summary>
    /// [DONE] Returns a vehicle to movement.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [HttpPost("return")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ReturnMovementVehicleAsync(Guid id)
    {
        await _vehicleService.ReturnMovementVehicleAsync(id);

        var messages = _notifier.GetNotification().ToList();

        if (_notifier.HasNotification())
            return BadRequest(new Response(_notifier.GetNotification().Select(x => x.Message), false));

        return NoContent();
    }

    /// <summary>
    /// [DONE] Gets a vehicle by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [HttpGet("{id}")]
    [ActionName("GetVehicleByIdAsync")]
    [ProducesResponseType(typeof(Vehicle), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVehicleByIdAsync(Guid id)
    {
        var vehicle = await _vehicleService.GetVehicleByIdAsync(id);

        if (vehicle == null)
            return NotFound($"Vehicle with id {id} not found.");

        if (_notifier.HasNotification())
            return BadRequest(new Response(_notifier.GetNotification().Select(x => x.Message), false));

        var response = new Response(new VehicleDTO(vehicle.Mark, vehicle.Situation), true);

        return Ok(response);
    }

    /// <summary>
    /// [DONE] Gets a vehicles.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [HttpGet]
    [ActionName("GetVehicleByIdAsync")]
    [ProducesResponseType(typeof(Vehicle), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllVehicles(int offset = 0, int limit = 15)
    {
        var vehicles = await _vehicleService.GetAllVehicles(offset, limit);

        if (_notifier.HasNotification())
            return BadRequest(new Response(_notifier.GetNotification().Select(x => x.Message), false));

        var response = new Response(vehicles.Select(x => new VehicleDTO(x.Mark, x.Situation)), true);

        return Ok(response);
    }
}

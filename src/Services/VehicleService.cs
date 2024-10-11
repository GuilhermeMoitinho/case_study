using CaseLocaliza.Db.Abstractions;
using CaseLocaliza.Models;
using CaseLocaliza.Notifications;
using CaseLocaliza.Notifications.Abstractions;
using CaseLocaliza.Services.Abstractions;
using static CaseLocaliza.Models.Vehicle;

namespace CaseLocaliza.Services;

public class VehicleService : BaseService, IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IVehicleAuditRepository _vehicleAuditRepository;
    private readonly IUnityOfWork _unityOfWork;

    public VehicleService(IVehicleRepository vehicleRepository, 
                          IUnityOfWork unityOfWork, 
                          IVehicleAuditRepository vehicleAuditRepository,
                          INotifier notifier) : base(notifier)
    {
        _vehicleRepository = vehicleRepository;
        _unityOfWork = unityOfWork;
        _vehicleAuditRepository = vehicleAuditRepository;
    }

    public async Task<Guid> AddVehicleAsync(Vehicle vehicle)
    {
        if (vehicle is null)
        {
            Notify("Add your vehicle, please.");
            return Guid.Empty;
        }

        var vehicleId = await _vehicleRepository.AddVehicleAsync(vehicle);

        await _vehicleAuditRepository.AddAsync(new VehicleAudit(vehicle.Situation, vehicleId ));

        await _unityOfWork.CommitAsync();

        return vehicleId;
    }

    public Task<IEnumerable<Vehicle>> GetAllVehicles(int offset, int limit)
    {
        var vehicles = _vehicleRepository.GetAllVehicles(offset, limit);

        if (vehicles is null)
            Notify("Vehicles not found");

        return vehicles;
    }

    public async Task<Vehicle> GetVehicleByIdAsync(Guid id)
    {
        return await _vehicleRepository.GetVehicleByIdAsync(id);
    }

    public async Task MovementVehicleAsync(Guid id, SituationType situationType)
    {
        if(await IsSituationValid(id, situationType) is false)
        {
            var currentSituation = await _vehicleRepository.GetVehicleByIdAsync(id);
            Notify($"The situation your vehicle is {currentSituation.Situation}, he cant't go to {situationType}.");
            return;
        }

        var vehicleDb = await _vehicleRepository.GetVehicleByIdAsync(id);

        vehicleDb.UpdateSituation(situationType);

        await _vehicleRepository.MovementVehicleAsync(vehicleDb);

        await _vehicleAuditRepository.AddAsync(new VehicleAudit( situationType, id));

        await _unityOfWork.CommitAsync();
    }


    public async Task ReturnMovementVehicleAsync(Guid id)
    {
        var vehicle = await _vehicleRepository.GetVehicleByIdAsync(id);

        if (await IsNotReturnValid(vehicle.CreatedAt, vehicle.DateSituationChanged) is false)
        {
            var timeInvalid = vehicle.CreatedAt.Minute - vehicle.DateSituationChanged.Minute;
            Notify($"Your vehicle has more {timeInvalid} been edited");
            return;
        }

        await _vehicleAuditRepository.RemoveLastAuditVehicle(id);

        await _unityOfWork.CommitAsync();

        if (vehicle is null)
        {
            Notify("Vehicle not found.");
            await _unityOfWork.RollBackAsync();
            return;
        }

        var situation = await _vehicleAuditRepository.LastSituationAuditVehicle(id);

        vehicle.UpdateSituation(situation);

        await _vehicleRepository.MovementVehicleAsync(vehicle);

        await _unityOfWork.CommitAsync();
    }

    private async Task<bool> IsNotReturnValid(DateTime CreatedAt, DateTime DateChanged)
    {
        var dateValid = DateChanged - CreatedAt;

        if (dateValid.Minutes >= 5)
            return false;

        return true;
    }

    private async Task<bool> IsSituationValid(Guid id, SituationType newSituation)
    {
        var vehicle = await _vehicleRepository.GetVehicleByIdAsync(id);
        var currentSituation = vehicle.Situation;

        switch (currentSituation)
        {
            case Vehicle.SituationType.Available:
                return newSituation == Vehicle.SituationType.Rented ||
                       newSituation == Vehicle.SituationType.Maintenance ||
                       newSituation == Vehicle.SituationType.Disabled;

            case Vehicle.SituationType.Maintenance:
                return newSituation == Vehicle.SituationType.Available ||
                       newSituation == Vehicle.SituationType.Disabled;

            case Vehicle.SituationType.Rented:
                return newSituation == Vehicle.SituationType.Available ||
                       newSituation == Vehicle.SituationType.Maintenance;

            case Vehicle.SituationType.Disabled:
                return false;

            default:
                return false;
        }
    }
}

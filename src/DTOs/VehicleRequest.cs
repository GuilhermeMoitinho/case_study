using CaseLocaliza.Models;
using static CaseLocaliza.Models.Vehicle;

namespace CaseLocaliza.DTOs;

public record VehicleRequest(string Mark, SituationType SituationType);

public static class VehicleRequestExtensions
{
    public static Vehicle IntoEntity(this VehicleRequest dto) => new Vehicle(dto.Mark, dto.SituationType);
}

using FluentValidation;

namespace CaseLocaliza.Models.Validations;

public class VehicleValidator : AbstractValidator<Vehicle>
{
    public VehicleValidator()
    {
        RuleFor(vehicle => vehicle.Mark)
            .NotEmpty().WithMessage("The vehicle mark is required.")
            .Length(2, 50).WithMessage("The vehicle mark must be between 2 and 50 characters.");

        RuleFor(vehicle => vehicle.Situation)
            .IsInEnum().WithMessage("Invalid situation type.");

        RuleFor(vehicle => vehicle.Situation)
            .Must(situation => situation != Vehicle.SituationType.Disabled)
            .When(vehicle => vehicle.Situation == Vehicle.SituationType.Rented ||
                             vehicle.Situation == Vehicle.SituationType.Maintenance)
            .WithMessage("A vehicle cannot be rented or in maintenance if it is disabled.");
    }
}

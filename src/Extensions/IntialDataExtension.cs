using CaseLocaliza.Db.Context;
using CaseLocaliza.Models;

namespace CaseLocaliza.Extensions;

public static class IntialDataExtension
{
    public static void AddInitialData(this IApplicationBuilder app)
    {
        using (var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var vehicles = new List<Vehicle>
            {
                Vehicle.Create("Volkswagen", Vehicle.SituationType.Available),
                Vehicle.Create("Ford Escort XR3", Vehicle.SituationType.Available),
                Vehicle.Create("Fiat Uno College", Vehicle.SituationType.Maintenance),
                Vehicle.Create("Citroën C3", Vehicle.SituationType.Rented),
                Vehicle.Create("Chevrolet Chevette Jeans", Vehicle.SituationType.Disabled),
                Vehicle.Create("Proton", Vehicle.SituationType.Available),
                Vehicle.Create("Venirauto", Vehicle.SituationType.Rented),
                Vehicle.Create("Innoson", Vehicle.SituationType.Available),
                Vehicle.Create("Wuling", Vehicle.SituationType.Maintenance)
            };

            context.Vehicles.AddRange(vehicles);
            context.SaveChanges(); 

            var vehicleAudits = vehicles.Select(vehicle =>
                VehicleAudit.Create(vehicle.Situation, vehicle.Id)).ToList();

            context.VehicleAudits.AddRange(vehicleAudits);
            context.SaveChanges(); 
        }
    }
}
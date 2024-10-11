using CaseLocaliza.Db;
using CaseLocaliza.Db.Abstractions;
using CaseLocaliza.Db.Context;
using CaseLocaliza.Db.Repositories;
using CaseLocaliza.Models.Validations;
using CaseLocaliza.Notifications;
using CaseLocaliza.Notifications.Abstractions;
using CaseLocaliza.Services;
using CaseLocaliza.Services.Abstractions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace CaseLocaliza.Configurations;

internal static class DependencyInjectionConfig
{
    internal static WebApplicationBuilder AddDependency(this WebApplicationBuilder builder)
    {
        #region Config AppDbContext
        var ConnectionStrings = builder.Configuration.GetConnectionString("DefaultConnection"); 
        builder.Services.AddDbContext<AppDbContext>(x => x.UseNpgsql(ConnectionStrings));
        #endregion

        #region Config Dependency Injection
        builder.Services.AddTransient<IVehicleRepository, VehicleRepository>();
        builder.Services.AddTransient<IVehicleAuditRepository, VehicleAuditRepository>();
        builder.Services.AddScoped<INotifier, Notifier>();
        builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();

        builder.Services.AddTransient<IVehicleService, VehicleService>();
        #endregion

        #region Config Fluent Validation
        builder.Services.AddValidatorsFromAssemblyContaining<VehicleValidator>();
        builder.Services.AddFluentValidationAutoValidation();
        #endregion

        return builder;
    }
}

using CaseLocaliza.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CaseLocaliza.Db.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id); 

        builder.Property(v => v.Mark)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Situation)
            .IsRequired(); 

        builder.Property(v => v.DateSituationChanged)
            .IsRequired(); 
    }
}

namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProcedureConfiguration : IEntityTypeConfiguration<Procedure>
    {
        public void Configure(EntityTypeBuilder<Procedure> procedure)
        {
            procedure
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Procedure)
                .HasForeignKey(a => a.ProcedureId);

            procedure
               .HasMany(p => p.Reviews)
               .WithOne(r => r.Procedure)
               .HasForeignKey(r => r.ProcedureId);
        }
    }
}

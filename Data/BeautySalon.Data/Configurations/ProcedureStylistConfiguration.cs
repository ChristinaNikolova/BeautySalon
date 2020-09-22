namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProcedureStylistConfiguration : IEntityTypeConfiguration<ProcedureStylist>
    {
        public void Configure(EntityTypeBuilder<ProcedureStylist> procedureStylist)
        {
            procedureStylist
                .HasKey(ps => new { ps.ProcedureId, ps.StylistId });
        }
    }
}

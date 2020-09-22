namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SkinProblemProcedureConfiguration : IEntityTypeConfiguration<SkinProblemProcedure>
    {
        public void Configure(EntityTypeBuilder<SkinProblemProcedure> skinProblemProcedure)
        {
            skinProblemProcedure
                .HasKey(spp => new { spp.ProcedureId, spp.SkinProblemId });
        }
    }
}

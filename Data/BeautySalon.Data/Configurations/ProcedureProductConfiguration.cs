namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProcedureProductConfiguration : IEntityTypeConfiguration<ProcedureProduct>
    {
        public void Configure(EntityTypeBuilder<ProcedureProduct> procedureProduct)
        {
            procedureProduct
                .HasKey(pp => new { pp.ProcedureId, pp.ProductId });
        }
    }
}

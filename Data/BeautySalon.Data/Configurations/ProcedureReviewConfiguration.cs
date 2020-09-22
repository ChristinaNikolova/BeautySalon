namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProcedureReviewConfiguration : IEntityTypeConfiguration<ProcedureReview>
    {
        public void Configure(EntityTypeBuilder<ProcedureReview> procedureReview)
        {
            procedureReview
                .HasKey(pr => new { pr.ProcedureId, pr.ClientId });
        }
    }
}

namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> productReview)
        {
            productReview
                 .HasKey(pr => new { pr.ProductId, pr.ClientId });
        }
    }
}

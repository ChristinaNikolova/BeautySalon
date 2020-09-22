namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ClientProductLikeConfiguration : IEntityTypeConfiguration<ClientProductLike>
    {
        public void Configure(EntityTypeBuilder<ClientProductLike> clientProductLike)
        {
            clientProductLike
                .HasKey(cpl => new { cpl.ClientId, cpl.ProductId });
        }
    }
}

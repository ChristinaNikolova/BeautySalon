namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ClientArticleLikeConfiguration : IEntityTypeConfiguration<ClientArticleLike>
    {
        public void Configure(EntityTypeBuilder<ClientArticleLike> clientArticleLike)
        {
            clientArticleLike
                .HasKey(cal => new { cal.ArticleId, cal.ClientId });
        }
    }
}

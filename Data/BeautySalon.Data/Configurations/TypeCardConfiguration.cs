namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TypeCardConfiguration : IEntityTypeConfiguration<TypeCard>
    {
        public void Configure(EntityTypeBuilder<TypeCard> typeCard)
        {
            typeCard
                .HasMany(tc => tc.Cards)
                .WithOne(c => c.TypeCard)
                .HasForeignKey(c => c.TypeCardId);
        }
    }
}

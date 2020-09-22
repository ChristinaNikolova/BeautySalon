namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SkinTypeConfiguration : IEntityTypeConfiguration<SkinType>
    {
        public void Configure(EntityTypeBuilder<SkinType> skinType)
        {
            skinType
                .HasMany(sp => sp.Clients)
                .WithOne(c => c.SkinType)
                .HasForeignKey(c => c.SkinTypeId);

            skinType
                .HasMany(sp => sp.Procedures)
                .WithOne(p => p.SkinType)
                .HasForeignKey(p => p.SkinTypeId);
        }
    }
}

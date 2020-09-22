namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> category)
        {
            category
                .HasMany(c => c.Articles)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId);

            category
               .HasMany(c => c.Procedures)
               .WithOne(p => p.Category)
               .HasForeignKey(p => p.CategoryId);

            category
              .HasMany(c => c.Procedures)
              .WithOne(p => p.Category)
              .HasForeignKey(p => p.CategoryId);

            category
              .HasMany(c => c.Stylists)
              .WithOne(s => s.Category)
              .HasForeignKey(s => s.CategoryId);
        }
    }
}

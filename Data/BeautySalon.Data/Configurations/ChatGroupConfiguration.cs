namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ChatGroupConfiguration : IEntityTypeConfiguration<ChatGroup>
    {
        public void Configure(EntityTypeBuilder<ChatGroup> chatGroup)
        {
            chatGroup
                .HasMany(cg => cg.ChatMessages)
                .WithOne(cm => cm.ChatGroup)
                .HasForeignKey(cm => cm.ChatGroupId);
        }
    }
}

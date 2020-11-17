namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> appUser)
        {
            appUser
                 .HasMany(c => c.Questions)
                 .WithOne(q => q.Client)
                 .HasForeignKey(q => q.ClientId);

            appUser
                 .HasMany(c => c.Comments)
                 .WithOne(com => com.Client)
                 .HasForeignKey(com => com.ClientId);

            appUser
                .HasMany(c => c.Orders)
                .WithOne(o => o.Client)
                .HasForeignKey(o => o.ClientId);

            appUser
                .HasMany(c => c.ClientAppointments)
                .WithOne(a => a.Client)
                .HasForeignKey(a => a.ClientId);

            appUser
                .HasMany(s => s.Answers)
                .WithOne(a => a.Stylist)
                .HasForeignKey(a => a.StylistId);

            appUser
                .HasMany(s => s.Articles)
                .WithOne(a => a.Stylist)
                .HasForeignKey(a => a.StylistId);

            appUser
                .HasMany(s => s.StylistAppointments)
                .WithOne(a => a.Stylist)
                .HasForeignKey(a => a.StylistId);

            appUser
                .HasMany(u => u.SendMessages)
                .WithOne(m => m.Sender)
                .HasForeignKey(m => m.SenderId);

            appUser
                .HasMany(u => u.ReceivedMessages)
                .WithOne(m => m.Receiver)
                .HasForeignKey(m => m.ReceiverId);

            appUser
               .HasMany(u => u.Reviews)
               .WithOne(r => r.Client)
               .HasForeignKey(r => r.ClientId);

            appUser
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            appUser
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            appUser
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}

namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> appointment)
        {
            appointment
                .HasOne(a => a.Review)
                .WithOne(r => r.Appointment)
                .HasForeignKey<Appointment>(a => a.ReviewId);
        }
    }
}

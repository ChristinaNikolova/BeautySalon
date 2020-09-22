namespace BeautySalon.Data.Configurations
{
    using System;

    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class JobTypeConfiguration : IEntityTypeConfiguration<JobType>
    {
        public void Configure(EntityTypeBuilder<JobType> jobType)
        {
            jobType
                .HasMany(jt => jt.Stylists)
                .WithOne(s => s.JobType)
                .HasForeignKey(s => s.JobTypeId);
        }
    }
}

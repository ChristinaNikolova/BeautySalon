namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ClientSkinProblemConfiguration : IEntityTypeConfiguration<ClientSkinProblem>
    {
        public void Configure(EntityTypeBuilder<ClientSkinProblem> clientSkinProblem)
        {
            clientSkinProblem
                .HasKey(csp => new { csp.SkinProblemId, csp.ClientId });
        }
    }
}

using MasterServer.Domain.Entities.BattlePasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterServer.Infrastructure.Persistence.Configurations
{
    public class BattlePassDefinitionConfiguration : IEntityTypeConfiguration<BattlePassDefinition>
    {
        public void Configure(EntityTypeBuilder<BattlePassDefinition> builder)
        {
            builder.Property(e => e.Type).HasConversion<string>();
            builder.HasOne(e => e.PurchaseDefinition).WithMany().HasForeignKey(e => e.PurchaseDefinitionId);
        }
    }
}

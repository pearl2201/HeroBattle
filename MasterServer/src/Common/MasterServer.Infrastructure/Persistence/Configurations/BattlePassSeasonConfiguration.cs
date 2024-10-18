using MasterServer.Domain.Entities.BattlePasses;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Infrastructure.Persistence.Configurations
{
    public class BattlePassSeasonConfiguration : IEntityTypeConfiguration<BattlePassSeason>
    {
        public void Configure(EntityTypeBuilder<BattlePassSeason> builder)
        {
           builder.HasKey(x => new {x.DefinitionId, x.Id});
        }
    }
}

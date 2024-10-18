using MasterServer.Domain.Entities.Socials;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Infrastructure.Persistence.Configurations
{
    public class ConversationParticipantConfiguration : IEntityTypeConfiguration<ConversationParticipant>
    {
        public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
        {
            builder.HasIndex(e => new { e.PlayerId, e.ConversationId }).IsUnique();

            builder.HasOne(e => e.Player).WithMany(e => e.ConversationParticipants).HasForeignKey(e => e.PlayerId);
            builder.HasOne(e => e.Conversation).WithMany(e => e.ConversationParticipants).HasForeignKey(e => e.ConversationId);
        }
    }
}

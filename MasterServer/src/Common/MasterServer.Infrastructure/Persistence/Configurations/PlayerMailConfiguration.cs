using MasterServer.Domain.Entities.Mails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Infrastructure.Persistence.Configurations
{
    public class PlayerMailConfiguration : IEntityTypeConfiguration<PlayerMail>
    {
        public void Configure(EntityTypeBuilder<PlayerMail> builder)
        {
            builder.HasKey(x => new { x.PlayerId, x.MailId });

            builder.HasOne(x => x.Player).WithMany(x => x.MailBox).HasForeignKey(x => x.PlayerId);
            builder.HasOne(x => x.Mail).WithMany(x => x.PlayerMails).HasForeignKey(x => x.MailId);

        }
    }


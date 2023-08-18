using Kodigos.Dominio.ModelsData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodigos.Dominio.ModelsData.Administration.Account;

namespace Kodigos.Infra.Data.Configuration.Administration.Account
{
    public class AccountConfiguration
    {
        public AccountConfiguration(EntityTypeBuilder<AccountDTO> entity)
        {
            if (entity != null)
            {
                entity.ToTable("Administration.Account").HasKey(t => t.Id);
                entity.Property(t => t.name);
                entity.Property(t => t.agency).IsRequired(false);
                entity.Property(t => t.accountNumber).IsRequired(false);
                entity.Property(t => t.isActive);
                entity.Property(t => t.dateCreated);
                entity.Property(t => t.dateUpdate).IsRequired(false);

            }
        }
    }
}

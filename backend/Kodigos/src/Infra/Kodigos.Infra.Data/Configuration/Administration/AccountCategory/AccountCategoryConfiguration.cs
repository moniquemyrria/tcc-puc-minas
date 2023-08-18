using Kodigos.Dominio.ModelsData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodigos.Dominio.ModelsData.Administration.AccountCategory;

namespace Kodigos.Infra.Data.Configuration.Administration.AccountCategory
{
    public class AccountCategoryConfiguration
    {
        public AccountCategoryConfiguration(EntityTypeBuilder<AccountCategoryDTO> entity)
        {
            if (entity != null)
            {
                entity.ToTable("Administration.AccountCategory").HasKey(t => t.Id);
                entity.Property(t => t.initials);
                entity.Property(t => t.description);
                entity.Property(t => t.isActive);
                entity.Property(t => t.dateCreated);
                entity.Property(t => t.dateUpdate).IsRequired(false);

            }
        }
    }
}

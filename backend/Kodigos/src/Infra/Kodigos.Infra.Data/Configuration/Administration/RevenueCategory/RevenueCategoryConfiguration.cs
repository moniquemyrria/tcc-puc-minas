using Kodigos.Dominio.ModelsData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Kodigos.Dominio.ModelsData.Administration.RevenueCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodigos.Infra.Data.Configuration.Administration.RevenueCategory
{
    public class RevenueCategoryConfiguration
    {
        public RevenueCategoryConfiguration(EntityTypeBuilder<RevenueCategoryDTO> entity)
        {
            if (entity != null)
            {
                entity.ToTable("Administration.RevenueCategory").HasKey(t => t.Id);
                entity.Property(t => t.initials);
                entity.Property(t => t.description);
                entity.Property(t => t.color);
                entity.Property(t => t.isActive);
                entity.Property(t => t.dateCreated);
                entity.Property(t => t.dateUpdate).IsRequired(false);

            }
        }
    }
}

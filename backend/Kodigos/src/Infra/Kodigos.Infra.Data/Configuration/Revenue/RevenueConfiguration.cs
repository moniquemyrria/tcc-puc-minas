using Kodigos.Dominio.ModelsData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Kodigos.Dominio.ModelsData.Revenue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodigos.Infra.Data.Configuration.Revenue
{
    public class RevenueConfiguration
    {
        public RevenueConfiguration(EntityTypeBuilder<RevenueDTO> entity)
        {
            if (entity != null)
            {
                entity.ToTable("Revenue").HasKey(t => t.Id);
                entity.Property(t => t.value);
                entity.Property(t => t.description).IsRequired(false);
                entity.Property(t => t.obs).IsRequired(false);
                entity.Property(t => t.revenueReceipt).IsRequired(false);
                entity.Property(t => t.revenueStatus).IsRequired(false);
                entity.Property(t => t.isActive);
                entity.Property(t => t.dateCreated);
                entity.Property(t => t.dateUpdate).IsRequired(false);

                //FK - SUPPLIER
                entity.HasOne(t => t.supplier).WithMany(t => t.revenue).HasPrincipalKey(t => t.Id).OnDelete(DeleteBehavior.Restrict);


            }
        }
    }
}

using Kodigos.Dominio.ModelsData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Kodigos.Dominio.ModelsData.Administration.TypePerson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodigos.Infra.Data.Configuration.Administration.TypePerson
{
    public class SupplierConfiguration
    {
        public SupplierConfiguration(EntityTypeBuilder<SupplierDTO> entity)
        {
            if (entity != null)
            {
                entity.ToTable("Administration.Supplier").HasKey(t => t.Id);
                entity.Property(t => t.name);
                entity.Property(t => t.typePerson);
                entity.Property(t => t.isActive);
                entity.Property(t => t.dateCreated);
                entity.Property(t => t.dateUpdate).IsRequired(false);

            }
        }
    }
}

using Kodigos.Dominio.ModelsData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodigos.Dominio.ModelsData.Administration.PaymentMethods;

namespace Kodigos.Infra.Data.Configuration.Administration.PaymentMethods
{
    public class PaymentMethodsConfiguration
    {
        public PaymentMethodsConfiguration(EntityTypeBuilder<PaymentMethodsDTO> entity)
        {
            if (entity != null)
            {
                entity.ToTable("Administration.PaymentMethods").HasKey(t => t.Id);
                entity.Property(t => t.name);
                entity.Property(t => t.acceptInstallment);
                entity.Property(t => t.isActive);
                entity.Property(t => t.dateCreated);
                entity.Property(t => t.dateUpdate).IsRequired(false);

            }
        }
    }
}

using Kodigos.Dominio.ModelsData.Help;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodigos.Infra.Data.Configuration.Help
{
    public class HelpConfiguration
    {
        public HelpConfiguration(EntityTypeBuilder<HelpDTO> entity)
        {
            if (entity != null)
            {
                entity.ToTable("Help.Help").HasKey(t => t.Id);
                entity.Property(t => t.name);
                entity.Property(t => t.email);
                entity.Property(t => t.cpf);
                entity.Property(t => t.subject);
                entity.Property(t => t.message);
                entity.Property(t => t.typePerson);
                entity.Property(t => t.dateCreated);
              
            }
        }
    }
}

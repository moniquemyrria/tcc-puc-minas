using Kodigos.Dominio.ModelsData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodigos.Dominio.ModelsData.Administration.Users;

namespace Kodigos.Infra.Data.Configuration.Administration.Users
{
    public class UsersConfiguration
    {
        public UsersConfiguration(EntityTypeBuilder<UsersDTO> entity)
        {
            if (entity != null)
            {
                entity.ToTable("Administration.Users").HasKey(t => t.Id);
                entity.Property(t => t.isActive);
                entity.Property(t => t.dateCreated);
                entity.Property(t => t.dateUpdate).IsRequired(false);

                entity.Property(t => t.name);
                entity.Property(t => t.lastAccessDate).IsRequired(false);

               
            }
        }
    }
}

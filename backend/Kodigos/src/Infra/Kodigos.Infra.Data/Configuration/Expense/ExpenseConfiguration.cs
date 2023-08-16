using Kodigos.Dominio.ModelsData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Kodigos.Dominio.ModelsData.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodigos.Infra.Data.Configuration.Expense
{
    public class ExpenseConfiguration
    {
        public ExpenseConfiguration(EntityTypeBuilder<ExpenseDTO> entity)
        {
            if (entity != null)
            {
                entity.ToTable("Expense").HasKey(t => t.Id);
                entity.Property(t => t.amount);
                entity.Property(t => t.description).IsRequired(false);
                entity.Property(t => t.obs).IsRequired(false);
                entity.Property(t => t.daysBetweenInstallments).IsRequired(false);
                entity.Property(t => t.dueDate);
                entity.Property(t => t.expenseStatus).IsRequired(false);
                entity.Property(t => t.numberInstallments).IsRequired(false);
                entity.Property(t => t.typeInstallment).IsRequired(false);
                entity.Property(t => t.expenseType);
                entity.Property(t => t.isActive);
                entity.Property(t => t.dateCreated);

                //FK - ACCOUNT
                entity.HasOne(t => t.account).WithMany(t => t.expense).HasPrincipalKey(t => t.Id).OnDelete(DeleteBehavior.Restrict);


                //FK - PAYMENT METHODS
                entity.HasOne(t => t.paymentMethods).WithMany(t => t.expense).HasPrincipalKey(t => t.Id).OnDelete(DeleteBehavior.Restrict);


                //FK - SUPPLIER
                entity.HasOne(t => t.supplier).WithMany(t => t.expense).HasPrincipalKey(t => t.Id).OnDelete(DeleteBehavior.Restrict);


            }
        }
    }
}

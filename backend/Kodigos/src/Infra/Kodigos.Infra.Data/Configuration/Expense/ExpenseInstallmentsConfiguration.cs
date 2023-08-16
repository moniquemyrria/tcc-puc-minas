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
    public class ExpenseInstallmentsConfiguration
    {
        public ExpenseInstallmentsConfiguration(EntityTypeBuilder<ExpenseInstallmentsDTO> entity)
        {
            if (entity != null)
            {
                entity.ToTable("Expense.Installments").HasKey(t => t.Id);
                entity.Property(t => t.dueDate);
                entity.Property(t => t.installment);
                entity.Property(t => t.installmentDescription).IsRequired(false);
                entity.Property(t => t.value);
                entity.Property(t => t.dueDate);
                entity.Property(t => t.isPaid);
                entity.Property(t => t.paymentDate).IsRequired(false);
                
            }
        }
    }
}

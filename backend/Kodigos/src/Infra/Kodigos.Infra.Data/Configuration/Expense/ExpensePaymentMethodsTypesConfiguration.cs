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
    public class ExpensePaymentMethodsTypesConfiguration
    {
        public ExpensePaymentMethodsTypesConfiguration(EntityTypeBuilder<ExpensePaymentMethodsTypesDTO> entity)
        {
            if (entity != null)
            {
                entity.ToTable("Expense.PaymentMethods.Types").HasKey(t => t.Id);
                entity.Property(t => t.taxRate).IsRequired(false);
                entity.Property(t => t.totalPayment).IsRequired(false);
                entity.Property(t => t.trafficTicket).IsRequired(false);
                entity.Property(t => t.value);
                entity.Property(t => t.paymentDate);

                //FK - EXPENSE
                entity.HasOne(t => t.expense).WithMany(t => t.expensePaymentMethodsTypes).HasPrincipalKey(t => t.Id).OnDelete(DeleteBehavior.Restrict);

                //FK - PAYMENT METHODS
                entity.HasOne(t => t.paymentMethods).WithMany(t => t.expensePaymentMethodsTypes).HasPrincipalKey(t => t.Id).OnDelete(DeleteBehavior.Restrict);

                //FK - PAYMENT METHODS TYPES
                entity.HasOne(t => t.paymentMethodsTypes).WithMany(t => t.expensePaymentMethodsTypes).HasPrincipalKey(t => t.Id).OnDelete(DeleteBehavior.Restrict);

                //FK - INSTALLMENSTS
                entity.HasOne(t => t.expenseInstallments).WithMany(t => t.expensePaymentMethodsTypes).HasPrincipalKey(t => t.Id).OnDelete(DeleteBehavior.Restrict);

            }
        }
    }
}

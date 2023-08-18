
using Kodigos.Dominio.ModelsData;
using Kodigos.Dominio.ModelsData.Administration.ExpenseCategory;
using Kodigos.Infra.Data.Configuration;
using Kodigos.Infra.Data.Configuration.Administration.ExpenseCategory;
using Kodigos.Dominio.ModelsData.Administration.Users;
using Kodigos.Infra.Data.Configuration.Administration.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Model;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodigos.Dominio.ModelsData.Help;
using Kodigos.Infra.Data.Configuration.Help;
using Kodigos.Dominio.ModelsData.Administration.RevenueCategory;
using Kodigos.Infra.Data.Configuration.Administration.RevenueCategory;
using Kodigos.Dominio.ModelsData.Administration.AccountCategory;
using Kodigos.Infra.Data.Configuration.Administration.AccountCategory;
using Kodigos.Dominio.ModelsData.Administration.TypePerson;
using Kodigos.Infra.Data.Configuration.Administration.TypePerson;
using Kodigos.Dominio.ModelsData.Administration.Account;
using Kodigos.Infra.Data.Configuration.Administration.Account;
using Kodigos.Dominio.ModelsData.Administration.PaymentMethods;
using Kodigos.Infra.Data.Configuration.Administration.PaymentMethods;
using Kodigos.Dominio.ModelsData.Administration.PaymentMethodsTypes;
using Kodigos.Infra.Data.Configuration.Administration.PaymentMethodsTypes;
using Kodigos.Dominio.ModelsData.Revenue;
using Kodigos.Infra.Data.Configuration.Revenue;
using Kodigos.Dominio.ModelsData.Expense;
using Kodigos.Infra.Data.Configuration.Expense;

namespace Kodigos.Infra.Data.Contexto
{
    public class KodigosContext : IdentityDbContext<IdentityUser>, ISecurityKeyContext
    {
        public DbSet<KeyMaterial> SecurityKeys { get; set; }
        public DbSet<ExpenseCategoryDTO> ExpenseCategory { get; set; }
        public DbSet<RevenueCategoryDTO> RevenueCategory { get; set; }
        public DbSet<AccountCategoryDTO> AccountCategory { get; set; }
        public DbSet<SupplierDTO> Supplier { get; set; }
        public DbSet<AccountDTO> Account { get; set; }
        public DbSet<PaymentMethodsDTO> PaymentMethods { get; set; }
        public DbSet<PaymentMethodsTypesDTO> PaymentMethodsTypes { get; set; }
        public DbSet<RevenueDTO> Revenue { get; set; }

        public DbSet<UsersDTO> Users { get; set; }
        public DbSet<HelpDTO> Help { get; set; }

        public DbSet<ExpenseDTO> Expense { get; set; }
        public DbSet<ExpenseInstallmentsDTO> ExpenseInstallments { get; set; }
        public DbSet<ExpensePaymentMethodsTypesDTO> ExpensePaymentMethodsTypes { get; set; }

        public KodigosContext(DbContextOptions<KodigosContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder != null)
            {
    
                _ = new ExpenseCategoryConfiguration(modelBuilder.Entity<ExpenseCategoryDTO>());
                _ = new RevenueCategoryConfiguration(modelBuilder.Entity<RevenueCategoryDTO>());
                _ = new AccountCategoryConfiguration(modelBuilder.Entity<AccountCategoryDTO>());
                _ = new SupplierConfiguration(modelBuilder.Entity<SupplierDTO>());
                _ = new AccountConfiguration(modelBuilder.Entity<AccountDTO>());
                _ = new PaymentMethodsConfiguration(modelBuilder.Entity<PaymentMethodsDTO>());
                _ = new PaymentMethodsTypesConfiguration(modelBuilder.Entity<PaymentMethodsTypesDTO>());
                _ = new RevenueConfiguration(modelBuilder.Entity<RevenueDTO>());
                _ = new ExpenseConfiguration(modelBuilder.Entity<ExpenseDTO>());
                _ = new ExpenseInstallmentsConfiguration(modelBuilder.Entity<ExpenseInstallmentsDTO>());
                _ = new ExpensePaymentMethodsTypesConfiguration(modelBuilder.Entity<ExpensePaymentMethodsTypesDTO>());
                _ = new UsersConfiguration(modelBuilder.Entity<UsersDTO>());
                _ = new HelpConfiguration(modelBuilder.Entity<HelpDTO>());

                base.OnModelCreating(modelBuilder);

               
            }
        }
    }
}

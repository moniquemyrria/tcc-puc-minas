using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kodigos.API.Migrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administration.Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastAccessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administration.Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Help.Help",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    typePerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Help.Help", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SecurityKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KeyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Administration.AccountCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    initials = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administration.AccountCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administration.AccountCategory_Administration.Users_userId",
                        column: x => x.userId,
                        principalTable: "Administration.Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Administration.ExpenseCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    initials = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administration.ExpenseCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administration.ExpenseCategory_Administration.Users_userId",
                        column: x => x.userId,
                        principalTable: "Administration.Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Administration.RevenueCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    initials = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administration.RevenueCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administration.RevenueCategory_Administration.Users_userId",
                        column: x => x.userId,
                        principalTable: "Administration.Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Administration.Supplier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    typePerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administration.Supplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administration.Supplier_Administration.Users_userId",
                        column: x => x.userId,
                        principalTable: "Administration.Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Administration.Account",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    agency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    accountCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    typePersonId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administration.Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administration.Account_Administration.AccountCategory_accountCategoryId",
                        column: x => x.accountCategoryId,
                        principalTable: "Administration.AccountCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Administration.Account_Administration.Supplier_typePersonId",
                        column: x => x.typePersonId,
                        principalTable: "Administration.Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Administration.Account_Administration.Users_userId",
                        column: x => x.userId,
                        principalTable: "Administration.Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Administration.PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    acceptInstallment = table.Column<bool>(type: "bit", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    accountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administration.PaymentMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administration.PaymentMethods_Administration.Account_accountId",
                        column: x => x.accountId,
                        principalTable: "Administration.Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Administration.PaymentMethods_Administration.Users_userId",
                        column: x => x.userId,
                        principalTable: "Administration.Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Revenue",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value = table.Column<double>(type: "float", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    revenueReceipt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    revenueStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    revenueCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    accountId = table.Column<long>(type: "bigint", nullable: false),
                    typePersonId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revenue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Revenue_Administration.Account_accountId",
                        column: x => x.accountId,
                        principalTable: "Administration.Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Revenue_Administration.RevenueCategory_revenueCategoryId",
                        column: x => x.revenueCategoryId,
                        principalTable: "Administration.RevenueCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Revenue_Administration.Supplier_typePersonId",
                        column: x => x.typePersonId,
                        principalTable: "Administration.Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Revenue_Administration.Users_userId",
                        column: x => x.userId,
                        principalTable: "Administration.Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Administration.PaymentMethods.Types",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    paymentMethodsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administration.PaymentMethods.Types", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administration.PaymentMethods.Types_Administration.PaymentMethods_paymentMethodsId",
                        column: x => x.paymentMethodsId,
                        principalTable: "Administration.PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<double>(type: "float", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    daysBetweenInstallments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    expenseStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numberInstallments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    typeInstallment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    expenseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    accountId = table.Column<long>(type: "bigint", nullable: false),
                    expenseCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    paymentMethodsId = table.Column<long>(type: "bigint", nullable: true),
                    typePersonId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expense_Administration.Account_accountId",
                        column: x => x.accountId,
                        principalTable: "Administration.Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expense_Administration.ExpenseCategory_expenseCategoryId",
                        column: x => x.expenseCategoryId,
                        principalTable: "Administration.ExpenseCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expense_Administration.PaymentMethods_paymentMethodsId",
                        column: x => x.paymentMethodsId,
                        principalTable: "Administration.PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expense_Administration.Supplier_typePersonId",
                        column: x => x.typePersonId,
                        principalTable: "Administration.Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expense_Administration.Users_userId",
                        column: x => x.userId,
                        principalTable: "Administration.Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Expense.Installments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    installment = table.Column<long>(type: "bigint", nullable: false),
                    installmentDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    value = table.Column<double>(type: "float", nullable: false),
                    isPaid = table.Column<bool>(type: "bit", nullable: false),
                    paymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    expenseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense.Installments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expense.Installments_Expense_expenseId",
                        column: x => x.expenseId,
                        principalTable: "Expense",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expense.PaymentMethods.Types",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    taxRate = table.Column<double>(type: "float", nullable: true),
                    totalPayment = table.Column<double>(type: "float", nullable: true),
                    trafficTicket = table.Column<double>(type: "float", nullable: true),
                    value = table.Column<double>(type: "float", nullable: true),
                    paymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    expenseId = table.Column<long>(type: "bigint", nullable: false),
                    paymentMethodsId = table.Column<long>(type: "bigint", nullable: false),
                    paymentMethodsTypesId = table.Column<long>(type: "bigint", nullable: false),
                    expenseInstallmentsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense.PaymentMethods.Types", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expense.PaymentMethods.Types_Administration.PaymentMethods_paymentMethodsId",
                        column: x => x.paymentMethodsId,
                        principalTable: "Administration.PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expense.PaymentMethods.Types_Administration.PaymentMethods.Types_paymentMethodsTypesId",
                        column: x => x.paymentMethodsTypesId,
                        principalTable: "Administration.PaymentMethods.Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expense.PaymentMethods.Types_Expense_expenseId",
                        column: x => x.expenseId,
                        principalTable: "Expense",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expense.PaymentMethods.Types_Expense.Installments_expenseInstallmentsId",
                        column: x => x.expenseInstallmentsId,
                        principalTable: "Expense.Installments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Administration.Account_accountCategoryId",
                table: "Administration.Account",
                column: "accountCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Administration.Account_typePersonId",
                table: "Administration.Account",
                column: "typePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Administration.Account_userId",
                table: "Administration.Account",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Administration.AccountCategory_userId",
                table: "Administration.AccountCategory",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Administration.ExpenseCategory_userId",
                table: "Administration.ExpenseCategory",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Administration.PaymentMethods_accountId",
                table: "Administration.PaymentMethods",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Administration.PaymentMethods_userId",
                table: "Administration.PaymentMethods",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Administration.PaymentMethods.Types_paymentMethodsId",
                table: "Administration.PaymentMethods.Types",
                column: "paymentMethodsId");

            migrationBuilder.CreateIndex(
                name: "IX_Administration.RevenueCategory_userId",
                table: "Administration.RevenueCategory",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Administration.Supplier_userId",
                table: "Administration.Supplier",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_accountId",
                table: "Expense",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_expenseCategoryId",
                table: "Expense",
                column: "expenseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_paymentMethodsId",
                table: "Expense",
                column: "paymentMethodsId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_typePersonId",
                table: "Expense",
                column: "typePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_userId",
                table: "Expense",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense.Installments_expenseId",
                table: "Expense.Installments",
                column: "expenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense.PaymentMethods.Types_expenseId",
                table: "Expense.PaymentMethods.Types",
                column: "expenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense.PaymentMethods.Types_expenseInstallmentsId",
                table: "Expense.PaymentMethods.Types",
                column: "expenseInstallmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense.PaymentMethods.Types_paymentMethodsId",
                table: "Expense.PaymentMethods.Types",
                column: "paymentMethodsId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense.PaymentMethods.Types_paymentMethodsTypesId",
                table: "Expense.PaymentMethods.Types",
                column: "paymentMethodsTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Revenue_accountId",
                table: "Revenue",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Revenue_revenueCategoryId",
                table: "Revenue",
                column: "revenueCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Revenue_typePersonId",
                table: "Revenue",
                column: "typePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Revenue_userId",
                table: "Revenue",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Expense.PaymentMethods.Types");

            migrationBuilder.DropTable(
                name: "Help.Help");

            migrationBuilder.DropTable(
                name: "Revenue");

            migrationBuilder.DropTable(
                name: "SecurityKeys");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Administration.PaymentMethods.Types");

            migrationBuilder.DropTable(
                name: "Expense.Installments");

            migrationBuilder.DropTable(
                name: "Administration.RevenueCategory");

            migrationBuilder.DropTable(
                name: "Expense");

            migrationBuilder.DropTable(
                name: "Administration.ExpenseCategory");

            migrationBuilder.DropTable(
                name: "Administration.PaymentMethods");

            migrationBuilder.DropTable(
                name: "Administration.Account");

            migrationBuilder.DropTable(
                name: "Administration.AccountCategory");

            migrationBuilder.DropTable(
                name: "Administration.Supplier");

            migrationBuilder.DropTable(
                name: "Administration.Users");
        }
    }
}

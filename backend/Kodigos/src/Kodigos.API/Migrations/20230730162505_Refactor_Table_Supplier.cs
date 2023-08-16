using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kodigos.API.Migrations
{
    public partial class Refactor_Table_Supplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Administration.Supplier_typePersonId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_Revenue_Administration.Supplier_typePersonId",
                table: "Revenue");

            migrationBuilder.RenameColumn(
                name: "typePersonId",
                table: "Revenue",
                newName: "supplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Revenue_typePersonId",
                table: "Revenue",
                newName: "IX_Revenue_supplierId");

            migrationBuilder.RenameColumn(
                name: "typePersonId",
                table: "Expense",
                newName: "supplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_typePersonId",
                table: "Expense",
                newName: "IX_Expense_supplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Administration.Supplier_supplierId",
                table: "Expense",
                column: "supplierId",
                principalTable: "Administration.Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Revenue_Administration.Supplier_supplierId",
                table: "Revenue",
                column: "supplierId",
                principalTable: "Administration.Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Administration.Supplier_supplierId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_Revenue_Administration.Supplier_supplierId",
                table: "Revenue");

            migrationBuilder.RenameColumn(
                name: "supplierId",
                table: "Revenue",
                newName: "typePersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Revenue_supplierId",
                table: "Revenue",
                newName: "IX_Revenue_typePersonId");

            migrationBuilder.RenameColumn(
                name: "supplierId",
                table: "Expense",
                newName: "typePersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_supplierId",
                table: "Expense",
                newName: "IX_Expense_typePersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Administration.Supplier_typePersonId",
                table: "Expense",
                column: "typePersonId",
                principalTable: "Administration.Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Revenue_Administration.Supplier_typePersonId",
                table: "Revenue",
                column: "typePersonId",
                principalTable: "Administration.Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

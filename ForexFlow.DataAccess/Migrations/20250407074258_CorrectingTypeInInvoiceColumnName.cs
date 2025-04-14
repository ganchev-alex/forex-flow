using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForexFlow.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CorrectingTypeInInvoiceColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IssuerContry",
                table: "Invoices",
                newName: "IssuerCountry");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IssuerCountry",
                table: "Invoices",
                newName: "IssuerContry");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForexFlow.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingInvoiceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IssuerName = table.Column<string>(type: "TEXT", nullable: false),
                    IssuerAddress = table.Column<string>(type: "TEXT", nullable: false),
                    IssuerContry = table.Column<string>(type: "TEXT", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BillToName = table.Column<string>(type: "TEXT", nullable: false),
                    BillToAddress = table.Column<string>(type: "TEXT", nullable: false),
                    BillToCountry = table.Column<string>(type: "TEXT", nullable: false),
                    RecipientName = table.Column<string>(type: "TEXT", nullable: false),
                    RecipientAddress = table.Column<string>(type: "TEXT", nullable: false),
                    RecipientCountry = table.Column<string>(type: "TEXT", nullable: false),
                    InvoiceCurrencyCode = table.Column<string>(type: "TEXT", nullable: false),
                    InvoiceItems = table.Column<string>(type: "TEXT", nullable: false),
                    Subtotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    AmountBeforeVat = table.Column<decimal>(type: "TEXT", nullable: false),
                    VatRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    VatAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Currencies_InvoiceCurrencyCode",
                        column: x => x.InvoiceCurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceCurrencyCode",
                table: "Invoices",
                column: "InvoiceCurrencyCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}

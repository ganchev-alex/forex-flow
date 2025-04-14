using ForexFlow.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForexFlow.Model
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public string IssuerName { get; set; }
        public string IssuerAddress { get; set; }
        public string IssuerCountry { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public string BillToName { get; set; }
        public string BillToAddress { get; set; }
        public string BillToCountry { get; set; }
        public string RecipientName { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientCountry { get; set; }

        [ForeignKey("Currency")]
        public string InvoiceCurrencyCode { get; set; }
        public Currency InvoiceCurrency { get; set; }
        public List<Item> InvoiceItems { get; set; } = new List<Item>();
        public decimal Subtotal { get; set; } = 0.00m; // Sum of items BEFORE discount/VAT
        public decimal DiscountAmount { get; set; } = 0.00m; // Total discount applied
        public decimal AmountBeforeVat { get; set; } = 0.00m; // Calc: Subtotal - DiscountAmount
        public decimal VatRate { get; set; } = 0.2m; // 20%
        public decimal VatAmount { get; set; } = 0.00m; // Caclulate the VAT Amount based on the VatRate from the AmountBeforeVat
        public decimal TotalAmount { get; set; } = 0.00m; // Final amount due (AmountBeforeVAT + VatAmount)
    }

    public class Item
    {
        public uint Quantity { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForexFlow.Models
{
    public class SingleCurrencyAmount
    {
        public Guid Id { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;

        [ForeignKey("Currency")]
        public string CurrencyCode { get; set; } = string.Empty;
        public Currency Currency { get; set; } = new Currency();
    }
}



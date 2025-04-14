using System.ComponentModel.DataAnnotations;

namespace ForexFlow.Models
{
    public class Currency
    {
        [Key]
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public decimal ExchangeRateToBase { get; set; }
    }
}

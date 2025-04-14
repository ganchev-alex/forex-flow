using ForexFlow.Models.Extensions;

namespace ForexFlow.Models
{
    public class MultiCurrencyAmount
    {
        public SingleCurrencyAmount BaseAmount { get; set; }
        public Dictionary<string, decimal> AmountCurrencyRepresentations { get; private set; } = new();

        public MultiCurrencyAmount(SingleCurrencyAmount baseAmount, Currency currency)
        {
            BaseAmount = baseAmount;
            AmountCurrencyRepresentations.Add(currency.Code, baseAmount.CalculateCurrencyConversion(currency));
        }
    }
}

namespace ForexFlow.Models.Extensions
{
    public static class SingleCurrencyAmountActions
    {
        public static void SetAmountFromTransfer(this SingleCurrencyAmount currentAmount, SingleCurrencyAmount transferableAmount, bool fullTransfer = false)
        {
            if (currentAmount.CurrencyCode != transferableAmount.CurrencyCode)
            {
                throw new Exception($"You can't transfer one amount to another, if they are from a different currency.\nYou are trying to transfer the amount in the following way: {transferableAmount.CurrencyCode} ({transferableAmount.Currency.Symbol} -> {currentAmount.CurrencyCode} ({currentAmount.Currency.Symbol})");
            }

            currentAmount.Amount = transferableAmount.Amount;

            if (fullTransfer)
            {
                currentAmount.Description = transferableAmount.Description;
            }
        }

        public static void IncreaseAmountByTransfer(this SingleCurrencyAmount currentAmount, SingleCurrencyAmount transferableAmount)
        {
            if (currentAmount.CurrencyCode != transferableAmount.CurrencyCode)
            {
                throw new Exception($"You can't increase one amount by transfering from another, if they are from a different currency.\nYou are trying to increase the amount in the following way: +{transferableAmount.Amount} ({transferableAmount.Currency.Symbol} -> {currentAmount.Amount} ({currentAmount.Currency.Symbol})");
            }

            currentAmount.Amount += transferableAmount.Amount;
            transferableAmount.Amount = 0;
        }

        public static void DecreaseAmountByTransfer(this SingleCurrencyAmount currentAmount, SingleCurrencyAmount transferableAmount)
        {
            if (currentAmount.CurrencyCode != transferableAmount.CurrencyCode)
            {
                throw new Exception($"You can't decrease one amount by transfering from another, if they are from a different currency.\nYou are trying to decrease the amount in the following way: -{transferableAmount.Amount} ({transferableAmount.Currency.Symbol} -> {currentAmount.Amount} ({currentAmount.Currency.Symbol})");
            }

            transferableAmount.Amount += currentAmount.Amount;
            currentAmount.Amount = 0;
        }

        public static void IncreaseAmountByFixed(this SingleCurrencyAmount currentAmount, decimal increasement)
        {
            currentAmount.Amount += increasement;
        }

        public static void DescreaseAmountByFixed(this SingleCurrencyAmount currentAmount, decimal increasement)
        {
            if(currentAmount.Amount - increasement <= 0)
            {
                currentAmount.Amount = 0;
            } 
            else
            {
                currentAmount.Amount -= increasement;
            }
        }


        public static decimal CalculateCurrencyConversion(this SingleCurrencyAmount currencyAmount, Currency newCurrency)
        {
            decimal amountInBaseCurrency = currencyAmount.Amount / currencyAmount.Currency.ExchangeRateToBase;
            return Math.Round(amountInBaseCurrency * newCurrency.ExchangeRateToBase, 2);
        }

        public static void ConvertCurrency(this SingleCurrencyAmount currencyAmount, Currency newCurrency)
        {
            if (currencyAmount.CurrencyCode == newCurrency.Code) return;

            currencyAmount.Amount = currencyAmount.CalculateCurrencyConversion(newCurrency);

            currencyAmount.CurrencyCode = newCurrency.Code;
        }
    }
}


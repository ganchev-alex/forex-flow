using ForexFlow.Models;
using System.ComponentModel;

namespace ForexFlow.ViewModel.DataTransferObjects
{
    public class SingleCurrencyAmountActionsPayload : INotifyPropertyChanged
    {
        private decimal fixedAmount = 0.00m;
        private Guid selectedTransactionId;
        private bool transactionDirection = true;

        private MultiCurrencyAmount? multiCurrencyAmount;
        private string newCurrencyRepresentation;

        private string currencyConversionCode;

        public decimal FixedAmount
        {
            get => this.fixedAmount;
            set
            {
                fixedAmount = value;
                OnPropertyChanged(nameof(FixedAmount));
            }
        }

        public Guid SelectedTransactionId
        {
            get => this.selectedTransactionId;
            set
            {
                selectedTransactionId = value;
                OnPropertyChanged(nameof(SelectedTransactionId));
            }
        }

        public bool TransactionDirection
        {
            get => this.transactionDirection;
            set
            {
                transactionDirection = value;
                OnPropertyChanged(nameof(TransactionDirection));
            }
        }

        public MultiCurrencyAmount? MultiCurrencyAmount
        {
            get => this.multiCurrencyAmount;
            set
            {
                multiCurrencyAmount = value;
                OnPropertyChanged(nameof(MultiCurrencyAmount));
            }
        }

        public string NewCurrencyRepresentation
        {
            get => this.newCurrencyRepresentation;
            set
            {
                newCurrencyRepresentation = value;
                OnPropertyChanged(nameof(NewCurrencyRepresentation));
            }
        }

        public string CurrencyConversionCode
        {
            get => this.currencyConversionCode;
            set
            {
                currencyConversionCode = value;
                OnPropertyChanged(nameof(CurrencyConversionCode));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

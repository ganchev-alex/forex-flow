using System.ComponentModel;

namespace ForexFlow.ViewModel.DataTransferObjects
{
    public class SingleCurrencyAmountNotifier : INotifyPropertyChanged
    {
        private string _decription;
        private decimal _amount = 0.00m;
        private string _currencyCode;

        public string Description
        {
            get => this._decription;
            set
            {
                this._decription = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public decimal Amount
        {
            get => this._amount;
            set
            {
                this._amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        public string CurrencyCode
        {
            get => this._currencyCode;
            set
            {
                this._currencyCode = value;
                OnPropertyChanged(nameof(CurrencyCode));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

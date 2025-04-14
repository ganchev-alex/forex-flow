using ForexFlow.Models;
using System.ComponentModel;

namespace ForexFlow.ViewModel.DataTransferObjects
{
    public class CurrencyActionsPayload : INotifyPropertyChanged
    {
        private string editingCurrencyCode = string.Empty;
        private decimal exchangeRateHolder { set; get; }
        private string newCurrencyCode { get; set; } = string.Empty;
        private decimal newCurrencyExchangeRate { get; set; } = 0.00m;
        private string newCurrencySymbol { get; set; } = string.Empty;
        private string newCurrencyName { get; set; } = string.Empty;

        public string EditingCurrencyCode
        {
            get { return editingCurrencyCode; }
            set { 
                editingCurrencyCode = value; 
                OnPropertyChanged(nameof(EditingCurrencyCode));
            }
        }    
        
        public decimal ExchangeRateHolder
        {
            get { return exchangeRateHolder; }
            set { 
                exchangeRateHolder = value;
                OnPropertyChanged(nameof(ExchangeRateHolder));
            }
        }

        public string NewCurrencyCode
        {
            get { return newCurrencyCode.ToUpper(); }
            set
            {
                newCurrencyCode = value;
                OnPropertyChanged(nameof(NewCurrencyCode));
            }
        }

        public decimal NewCurrencyExchangeRate
        {
            get { return newCurrencyExchangeRate; }
            set
            {
                newCurrencyExchangeRate = value;
                OnPropertyChanged(nameof(newCurrencyExchangeRate));
            }
        }

        public string NewCurrencySymbol
        {
            get { return newCurrencySymbol; }
            set
            {
                newCurrencySymbol = value;
                OnPropertyChanged(nameof(NewCurrencySymbol));
            }
        }

        public string NewCurrencyName
        {
            get { return newCurrencyName; }
            set
            {
                newCurrencyName = value;
                OnPropertyChanged(nameof(NewCurrencyName));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

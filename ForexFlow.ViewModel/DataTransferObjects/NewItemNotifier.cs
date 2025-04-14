using ForexFlow.Models;
using System.ComponentModel;

namespace ForexFlow.ViewModel.DataTransferObjects
{
    public class NewItemNotifier : INotifyPropertyChanged
    {
        private uint quantity;
        private string description = string.Empty;
        private decimal unitPrice;

        private MultiCurrencyAmount currencyRepresentations;

        public uint Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(TotalAmount));
                }
            }
        }

        public string Description
        {
            get => description;
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public decimal UnitPrice
        {
            get => unitPrice;
            set
            {
                if (unitPrice != value)
                {
                    unitPrice = value;
                    OnPropertyChanged(nameof(UnitPrice));
                    OnPropertyChanged(nameof(TotalAmount));
                }
            }
        }

        public decimal TotalAmount => Quantity * UnitPrice;

        public MultiCurrencyAmount CurrencyRepresentations
        {
            get => currencyRepresentations;
            set { 
                currencyRepresentations= value; 
                OnPropertyChanged(nameof(CurrencyRepresentations));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

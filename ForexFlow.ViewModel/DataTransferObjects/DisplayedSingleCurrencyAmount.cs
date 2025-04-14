using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexFlow.ViewModel.DataTransferObjects
{
    public class DisplayedSingleCurrencyAmount : INotifyPropertyChanged
    {
        public Guid AmountId { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        private string _formattedAmount;
        public string FormattedAmount
        {
            get => _formattedAmount;
            set
            {
                _formattedAmount = value;
                OnPropertyChanged(nameof(FormattedAmount));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

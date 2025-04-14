using ForexFlow.Model;
using ForexFlow.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ForexFlow.ViewModel.DataTransferObjects
{
    public class InvoiceActionsPayload : INotifyPropertyChanged
    {
        private ObservableCollection<Invoice> _invoicesSource = new ObservableCollection<Invoice>(); 
        private Currency _selectedCurrency;
        private Invoice _composedInvoice = new Invoice();
        private NewItemNotifier newInvoiceItem = new NewItemNotifier();
        public bool EditingMode { get; set; } = false;
        private string searchValue = string.Empty;

        public ObservableCollection<Invoice> InvoicesSource
        {
            get { return _invoicesSource; }
            set
            {
                _invoicesSource = value;
                OnPropertyChanged(nameof(InvoicesSource));
            }
        } 
        public Currency SelectedCurrency
        {
            get { return _selectedCurrency; }
            set
            {
                _selectedCurrency = value;
                OnPropertyChanged(nameof(SelectedCurrency));
            }
        }

        public Invoice ComposedInvoice
        {
            get { return _composedInvoice; }
            set { 
                _composedInvoice = value; 
                OnPropertyChanged(nameof(ComposedInvoice));
            }
        } 

        public NewItemNotifier NewInvoiceItem
        {
            get { return newInvoiceItem; }
            set
            {
                newInvoiceItem = value;
                OnPropertyChanged(nameof(NewInvoiceItem));
            }
        }

        public string SearchValue
        {
            get { return searchValue; }
            set
            {
                searchValue = value;
                OnPropertyChanged(nameof(SearchValue));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.ComponentModel;

namespace ForexFlow.ViewModel.DataTransferObjects
{
    public class InvoiceComposerNotifier : INotifyPropertyChanged
    {
        private Guid id;
        private string issuerName = string.Empty;
        private string issuerAddress = string.Empty;
        private string issuerCountry = string.Empty;

        //private DateTime issueDate = DateTime.Today;
        //private DateTime dueDate = DateTime.Today.AddDays(30);

        private string billToName = string.Empty;
        private string billToAddress = string.Empty;
        private string billToCountry = string.Empty;
        private string recipientName = string.Empty;
        private string recipientAddress = string.Empty;
        private string recipientCountry = string.Empty;
        private string invoiceCurrencyCode = "EUR";

        //private decimal subtotal;
        //private decimal discountAmount;
        //private decimal vatRate = 0.2m;
        //private decimal vatAmount;
        //private decimal totalAmount;

        public Guid Id
        {
            get => id;
            set { if (id != value) { id = value; OnPropertyChanged(nameof(Id)); } }
        }

        public string IssuerName
        {
            get => issuerName;
            set { if (issuerName != value) { issuerName = value; OnPropertyChanged(nameof(IssuerName)); } }
        }

        public string IssuerAddress
        {
            get => issuerAddress;
            set { if (issuerAddress != value) { issuerAddress = value; OnPropertyChanged(nameof(IssuerAddress)); } }
        }

        public string IssuerCountry
        {
            get => issuerCountry;
            set { if (issuerCountry != value) { issuerCountry = value; OnPropertyChanged(nameof(IssuerCountry)); } }
        }

        public string BillToName
        {
            get => billToName;
            set { if (billToName != value) { billToName = value; OnPropertyChanged(nameof(BillToName)); } }
        }

        public string BillToAddress
        {
            get => billToAddress;
            set { if (billToAddress != value) { billToAddress = value; OnPropertyChanged(nameof(BillToAddress)); } }
        }

        public string BillToCountry
        {
            get => billToCountry;
            set { if (billToCountry != value) { billToCountry = value; OnPropertyChanged(nameof(BillToCountry)); } }
        }

        public string RecipientName
        {
            get => recipientName;
            set { if (recipientName != value) { recipientName = value; OnPropertyChanged(nameof(RecipientName)); } }
        }

        public string RecipientAddress
        {
            get => recipientAddress;
            set { if (recipientAddress != value) { recipientAddress = value; OnPropertyChanged(nameof(RecipientAddress)); } }
        }

        public string RecipientCountry
        {
            get => recipientCountry;
            set { if (recipientCountry != value) { recipientCountry = value; OnPropertyChanged(nameof(RecipientCountry)); } }
        }

        public string InvoiceCurrencyCode
        {
            get => invoiceCurrencyCode;
            set { if (invoiceCurrencyCode != value) { invoiceCurrencyCode = value; OnPropertyChanged(nameof(InvoiceCurrencyCode)); } }
        }

        public List<NewItemNotifier> InvoiceItems { get; set; } = new List<NewItemNotifier>();


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using ForexFlow.DataAccess.Repository;
using ForexFlow.Model;
using ForexFlow.Models;
using ForexFlow.Models.Extensions;
using ForexFlow.ViewModel.DataTransferObjects;
using ForexFlow.ViewModel.RelayDelegats;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ForexFlow.ViewModel.ViewModels
{
    public class InvoiceManagementViewModel
    {
        #region Data Initialization & Fields
        private readonly IUnitOfWork _unitOfWork;
        public ObservableCollection<Invoice> FetchedInvoices { get; set; } = new ObservableCollection<Invoice>();
        public ObservableCollection<Invoice> FilteredInvoices { get; set; } = new ObservableCollection<Invoice>();
        public ObservableCollection<NewItemNotifier> InvoiceItemsHolder { get; set; } = new ObservableCollection<NewItemNotifier>();
        public ObservableCollection<Currency> Currencies { get; set; } = new ObservableCollection<Currency>();
        public InvoiceActionsPayload ActionsPayload { get; set; } = new InvoiceActionsPayload();

        public ICommand AddNewItemToInvoiceCommand { get; set; }
        public ICommand RemoveItemFromInvoiceCommand { get; set; }
        public ICommand SaveInvoiceCommand { get; set; }
        public ICommand PreviewInvoiceCommand { get; set; }
        public ICommand DeleteInvoiceCommand { get; set; }

        public InvoiceManagementViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            AddNewItemToInvoiceCommand = new RelayCommand(AddItemToInvoice, () => ActionsPayload.NewInvoiceItem.Quantity >= 1 && ActionsPayload.NewInvoiceItem.Description != string.Empty && ActionsPayload.NewInvoiceItem.UnitPrice > 0);
            RemoveItemFromInvoiceCommand = new RelayCommandObject(RemoveItemFromInvoice, () => true);
            SaveInvoiceCommand = new RelayCommand(SaveInvoice, () => canSave());
            PreviewInvoiceCommand = new RelayCommandObject(PreviewInvoice, () => true);
            DeleteInvoiceCommand = new RelayCommandObject(DeleteInvoice, () => true);

            ActionsPayload.NewInvoiceItem.PropertyChanged += (s, e) =>
            {
                ((RelayCommand)AddNewItemToInvoiceCommand).RaiseCanExecuteChanged();
            };

            ActionsPayload.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ActionsPayload.SelectedCurrency))
                {
                    ChangeRepresentation();
                }

                if (e.PropertyName == nameof(ActionsPayload.SearchValue))
                {
                    FilterResults();
                }
            };

            LoadCurrencies();
            LoadInvoices();

            ActionsPayload.InvoicesSource = FetchedInvoices;
        }

        private async void LoadInvoices()
        {
            FetchedInvoices.Clear();

            var invoices = await _unitOfWork.invoiceRepository.GetAll();

            foreach (var invoice in invoices)
            {
                FetchedInvoices.Add(invoice);
            }
        }

        private async void LoadCurrencies()
        {
            var currencies = await _unitOfWork.currencyRepository.GetAll();

            if (currencies.Any())
            {
                Currencies.Clear();
                foreach (var currency in currencies)
                {
                    Currencies.Add(currency);
                }
            }

            ActionsPayload.SelectedCurrency = Currencies[0];
        }
        #endregion

        #region Utility Functions
        private void AddItemToInvoice()
        {
            var baseAmount = new SingleCurrencyAmount
            {
                Amount = ActionsPayload.NewInvoiceItem.UnitPrice,
                Description = ActionsPayload.NewInvoiceItem.Description,
                CurrencyCode = ActionsPayload.SelectedCurrency.Code,
                Currency = ActionsPayload.SelectedCurrency
            };

            InvoiceItemsHolder.Add(new NewItemNotifier
            {
                Quantity = ActionsPayload.NewInvoiceItem.Quantity,
                Description = ActionsPayload.NewInvoiceItem.Description,
                UnitPrice = ActionsPayload.NewInvoiceItem.UnitPrice,
                CurrencyRepresentations = new MultiCurrencyAmount(baseAmount, ActionsPayload.SelectedCurrency)
            });

            ActionsPayload.NewInvoiceItem.Quantity = 0;
            ActionsPayload.NewInvoiceItem.Description = string.Empty;
            ActionsPayload.NewInvoiceItem.UnitPrice = 0;
        }

        public void PreparePreview()
        {
            ActionsPayload.EditingMode = true;

            ActionsPayload.ComposedInvoice.Id = Guid.NewGuid();
            ActionsPayload.ComposedInvoice.IssueDate = DateTime.Now;
            ActionsPayload.ComposedInvoice.DueDate = DateTime.Now.AddDays(15);

            ActionsPayload.ComposedInvoice.InvoiceCurrencyCode = ActionsPayload.SelectedCurrency.Code;

            ActionsPayload.ComposedInvoice.Subtotal = 0.00m;
            ActionsPayload.ComposedInvoice.DiscountAmount = 0.00m;
            ActionsPayload.ComposedInvoice.AmountBeforeVat = 0.00m;
            ActionsPayload.ComposedInvoice.VatAmount = 0.00m;
            ActionsPayload.ComposedInvoice.TotalAmount = 0.00m;

            if (InvoiceItemsHolder.Any())
            {
                foreach (var item in InvoiceItemsHolder)
                {
                    ActionsPayload.ComposedInvoice.Subtotal += item.TotalAmount;
                    if (item.Quantity >= 75)
                    {
                        ActionsPayload.ComposedInvoice.DiscountAmount += item.TotalAmount * 0.07m;
                    }
                }

                ActionsPayload.ComposedInvoice.AmountBeforeVat = ActionsPayload.ComposedInvoice.Subtotal - ActionsPayload.ComposedInvoice.DiscountAmount;
                ActionsPayload.ComposedInvoice.VatAmount = ActionsPayload.ComposedInvoice.AmountBeforeVat * ActionsPayload.ComposedInvoice.VatRate;
                ActionsPayload.ComposedInvoice.TotalAmount = ActionsPayload.ComposedInvoice.VatAmount + ActionsPayload.ComposedInvoice.AmountBeforeVat;
            }

            ((RelayCommand)SaveInvoiceCommand).RaiseCanExecuteChanged();
        }

        private bool canSave()
        {
            var ci = ActionsPayload.ComposedInvoice;

            return ActionsPayload.EditingMode && ci.IssuerName != string.Empty && ci.IssuerAddress != string.Empty && ci.IssuerCountry != string.Empty && ci.BillToName != string.Empty && ci.BillToAddress != string.Empty && ci.BillToCountry != string.Empty && ci.RecipientName != string.Empty && ci.RecipientAddress != string.Empty && ci.RecipientCountry != string.Empty && InvoiceItemsHolder.Any();
        }

        public void InvoiceComposerCleanUp()
        {
            ActionsPayload.ComposedInvoice = new Invoice();
            InvoiceItemsHolder.Clear();
            ActionsPayload.SelectedCurrency = Currencies[0];
            ActionsPayload.SearchValue = string.Empty;
        }

        private void FilterResults()
        {
            if (string.IsNullOrWhiteSpace(ActionsPayload.SearchValue))
            {

                ActionsPayload.InvoicesSource = FetchedInvoices;

                FilteredInvoices.Clear();
                return;
            }

            FilteredInvoices.Clear();
            string searchValue = ActionsPayload.SearchValue.ToLowerInvariant();

            foreach (var invoice in FetchedInvoices)
            {
                if (invoice.BillToName.ToLowerInvariant().Contains(searchValue)
                    || invoice.Id.ToString().Contains(searchValue) 
                    || invoice.IssuerName.ToLowerInvariant().Contains(searchValue)
                    || invoice.IssueDate.ToString("dd.MM.yyyy").Contains(searchValue)
                    || $"{invoice.IssuerName}:{invoice.BillToName}".ToLowerInvariant().Contains(searchValue))
                {
                    FilteredInvoices.Add(invoice);
                }
            }

            ActionsPayload.InvoicesSource = FilteredInvoices;

        }
        #endregion

        #region Commands Definitions
        private void ChangeRepresentation()
        {
            if (!InvoiceItemsHolder.Any() || !ActionsPayload.EditingMode) return;

            foreach (var item in InvoiceItemsHolder)
            {
                if(!item.CurrencyRepresentations.AmountCurrencyRepresentations.ContainsKey(ActionsPayload.SelectedCurrency.Code))
                {
                    item.CurrencyRepresentations.AmountCurrencyRepresentations.Add(ActionsPayload.SelectedCurrency.Code, item.CurrencyRepresentations.BaseAmount.CalculateCurrencyConversion(ActionsPayload.SelectedCurrency));
                }

                item.UnitPrice = item.CurrencyRepresentations.AmountCurrencyRepresentations[ActionsPayload.SelectedCurrency.Code];
            }
        }

        private void RemoveItemFromInvoice(object parameter)
        {
            var itemToRemove = parameter as NewItemNotifier;
            if (itemToRemove != null)
            {
                InvoiceItemsHolder.Remove(itemToRemove);
            }
        }

        private async void SaveInvoice()
        {
            foreach (var item in InvoiceItemsHolder) {
                ActionsPayload.ComposedInvoice.InvoiceItems.Add(new Item
                {
                    Quantity = item.Quantity,
                    Description = item.Description,
                    UnitPrice = item.UnitPrice,
                    TotalAmount = item.TotalAmount,
                });
            }

            _unitOfWork.invoiceRepository.Add(ActionsPayload.ComposedInvoice);

            await _unitOfWork.SaveTransaction();

            FetchedInvoices.Add(ActionsPayload.ComposedInvoice);
            InvoiceComposerCleanUp();
        }

        private void PreviewInvoice(object invoice)
        {
            var invoiceToPreview = invoice as Invoice;
            if (invoiceToPreview != null)
            {
                ActionsPayload.EditingMode = false;
                ActionsPayload.ComposedInvoice = invoiceToPreview;

                InvoiceItemsHolder.Clear();

                foreach (var item in invoiceToPreview.InvoiceItems)
                {
                    InvoiceItemsHolder.Add(new NewItemNotifier
                    {
                        Quantity = item.Quantity,
                        Description = item.Description,
                        UnitPrice = item.UnitPrice,
                    });
                }

                ActionsPayload.SelectedCurrency = Currencies.First(c => c.Code == invoiceToPreview.InvoiceCurrencyCode);
            }
        }

        private async void DeleteInvoice(object invoice)
        {
            var invoiceToDelete = invoice as Invoice;
            if (invoiceToDelete != null)
            {
                _unitOfWork.invoiceRepository.Delete(invoiceToDelete);
                await _unitOfWork.SaveTransaction();

                FetchedInvoices.Remove(invoiceToDelete);
                FilteredInvoices.Remove(invoiceToDelete);
            }
        }
        #endregion
    }
}

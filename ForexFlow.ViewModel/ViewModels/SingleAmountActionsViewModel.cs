using ForexFlow.DataAccess.Repository;
using ForexFlow.Models;
using ForexFlow.Models.Extensions;
using ForexFlow.ViewModel.DataTransferObjects;
using ForexFlow.ViewModel.RelayDelegats;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable CS8618
namespace ForexFlow.ViewModel.ViewModels
{
    public class SingleAmountActionsViewModel : INotifyPropertyChanged
    {
        #region Fields And Properties
        private readonly IUnitOfWork _unitOfWork;
        private SingleCurrencyAmount _selectedCurrencyAmount;
        private DisplayedSingleCurrencyAmount _displayedCurrencyAmount;

        public ObservableCollection<DisplayedSingleCurrencyAmount> TransferableAmounts { get; set; } = new ObservableCollection<DisplayedSingleCurrencyAmount>();
        public ObservableCollection<string> AvailableCurrencyCodes { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CurrencyRepresentations { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ConversionCurrencyCodes { get; set; } = new ObservableCollection<string>();
        public SingleCurrencyAmountActionsPayload ActionsPayload { get; set; } = new SingleCurrencyAmountActionsPayload();

        public ICommand FixedIncreasementCommand { get; }
        public ICommand FixedDecreasmentCommand { get; }
        public ICommand SelectTransactionSourceCommand { get; }
        public ICommand ChangeTransactionDirectionCommand { get; }
        public ICommand TransferCommand { get; }
        public ICommand CurrencyRepresentationCommand { get; }
        public ICommand ConversionCommand { get; }
        public ICommand DeleteCommand { get; }

        public DisplayedSingleCurrencyAmount DisplayedCurrencyAmount
        {
            get => _displayedCurrencyAmount;
            set
            {
                _displayedCurrencyAmount = value;
                OnPropertyChanged(nameof(DisplayedCurrencyAmount));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public SingleAmountActionsViewModel(IUnitOfWork unitOfWork, Guid selectedId)

        {
            _unitOfWork = unitOfWork;
            FixedIncreasementCommand = new RelayCommand(FixedIncreasement, () => ActionsPayload.FixedAmount > 0);
            FixedDecreasmentCommand = new RelayCommand(FixedDecreasment, () => ActionsPayload.FixedAmount > 0);
            SelectTransactionSourceCommand = new RelayCommandGuid(SelectTransactionSource, () => true);
            ChangeTransactionDirectionCommand = new RelayCommand(ChangeTransactionDirection, () => true);
            TransferCommand = new RelayCommand(Transfer, () => ActionsPayload.SelectedTransactionId != Guid.Empty);
            CurrencyRepresentationCommand = new RelayCommand(AddCurrencyRepresentation, () => !String.IsNullOrEmpty(ActionsPayload.NewCurrencyRepresentation));
            ConversionCommand = new RelayCommand(ConvertCurrentAmount, () => !String.IsNullOrEmpty(ActionsPayload.CurrencyConversionCode));
            DeleteCommand = new RelayCommand(DeleteAmount, () => _selectedCurrencyAmount != null && _selectedCurrencyAmount.Id !=  Guid.Empty);

            ActionsPayload.PropertyChanged += (s, e) =>
            {
                ((RelayCommand)FixedIncreasementCommand).RaiseCanExecuteChanged();
                ((RelayCommand)FixedDecreasmentCommand).RaiseCanExecuteChanged();
                ((RelayCommandGuid)SelectTransactionSourceCommand).RaiseCanExecuteChanged();
                ((RelayCommand)ChangeTransactionDirectionCommand).RaiseCanExecuteChanged();
                ((RelayCommand)TransferCommand).RaiseCanExecuteChanged();
                ((RelayCommand)CurrencyRepresentationCommand).RaiseCanExecuteChanged();
                ((RelayCommand)ConversionCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
            };

            LoadSelectedAmount(selectedId);
            LoadTransferableAmounts();
            LoadAvailableCurrencies();
        }

        #endregion

        #region Data Fetching and Initialization
        private async void LoadSelectedAmount(Guid selectedId)
        {
            _selectedCurrencyAmount = await _unitOfWork.singleCurrencyAmountRepository.Get(amount => amount.Id == selectedId, "Currency", tracked: true);
            if(_selectedCurrencyAmount != null)
            {
                DisplayedCurrencyAmount = new DisplayedSingleCurrencyAmount
                {
                    AmountId = _selectedCurrencyAmount.Id,
                    Description = _selectedCurrencyAmount.Description,
                    CurrencyCode = _selectedCurrencyAmount.CurrencyCode,
                    FormattedAmount = $"{_selectedCurrencyAmount.Amount:F2} {_selectedCurrencyAmount.Currency.Symbol}"
                };
            }
        }

        private async void LoadTransferableAmounts()
        {
            TransferableAmounts.Clear();
            var availableAmounts = await _unitOfWork.singleCurrencyAmountRepository.GetAll(a => a.CurrencyCode == _selectedCurrencyAmount.CurrencyCode && a.Id != _selectedCurrencyAmount.Id);

            if (availableAmounts != null)
            {
                foreach (var amount in availableAmounts)
                {
                    TransferableAmounts.Add(new DisplayedSingleCurrencyAmount
                    {
                        AmountId = amount.Id,
                        Description = amount.Description,
                        CurrencyCode = amount.CurrencyCode,
                        FormattedAmount = $"{amount.Amount:F2} {amount.CurrencyCode}:   {amount.Description}"
                    });
                }
            }
        }

        private async void LoadAvailableCurrencies()
        {
            AvailableCurrencyCodes.Clear();
            ConversionCurrencyCodes.Clear();

            var availableCurrencyCodes = await _unitOfWork.currencyRepository.GetAll(c => c.Code != _selectedCurrencyAmount.CurrencyCode);

            if (availableCurrencyCodes == null) return;

            foreach (var currency in availableCurrencyCodes)
            {
                ConversionCurrencyCodes.Add(currency.Code);
            }

            if (ConversionCurrencyCodes.Count > 0)
            {
                ActionsPayload.CurrencyConversionCode = ConversionCurrencyCodes[0];
            }

            if (ActionsPayload.MultiCurrencyAmount != null)
            {
                availableCurrencyCodes = availableCurrencyCodes.Where(c => !ActionsPayload.MultiCurrencyAmount.AmountCurrencyRepresentations.ContainsKey(c.Code));
            }
           
            foreach (var currency in availableCurrencyCodes)
            {
                AvailableCurrencyCodes.Add(currency.Code);
            }

            if(AvailableCurrencyCodes.Count > 0)
            {
                ActionsPayload.NewCurrencyRepresentation = AvailableCurrencyCodes[0];
            }
        }

        #endregion
        private async void FixedIncreasement()
        {
            _selectedCurrencyAmount.IncreaseAmountByFixed(ActionsPayload.FixedAmount);
            _unitOfWork.singleCurrencyAmountRepository.Update(_selectedCurrencyAmount);

            await _unitOfWork.SaveTransaction();
            _displayedCurrencyAmount.FormattedAmount = $"{_selectedCurrencyAmount.Amount:F2} {_selectedCurrencyAmount.Currency.Symbol}";
            ActionsPayload.FixedAmount = 0.00m;

            UpdateMultiAmountCurrency();
        }

        private async void FixedDecreasment()
        {
            _selectedCurrencyAmount.DescreaseAmountByFixed(ActionsPayload.FixedAmount);

            _unitOfWork.singleCurrencyAmountRepository.Update(_selectedCurrencyAmount);

            await _unitOfWork.SaveTransaction();
            _displayedCurrencyAmount.FormattedAmount = $"{_selectedCurrencyAmount.Amount:F2} {_selectedCurrencyAmount.Currency.Symbol}";
            ActionsPayload.FixedAmount = 0.00m;

            UpdateMultiAmountCurrency();
        }

        private void SelectTransactionSource(Guid selectedId)
        {
            ActionsPayload.SelectedTransactionId = selectedId;
        }

        private void ChangeTransactionDirection()
        {
            ActionsPayload.TransactionDirection = !ActionsPayload.TransactionDirection;
        } 

        private async void Transfer()
        {
            var transferSource = await _unitOfWork.singleCurrencyAmountRepository.Get(a => a.Id == ActionsPayload.SelectedTransactionId, tracked: true);

            if (transferSource == null) return; 

            if(ActionsPayload.TransactionDirection)
            {
                _selectedCurrencyAmount.IncreaseAmountByTransfer(transferSource);
            } 
            else
            {
                _selectedCurrencyAmount.DecreaseAmountByTransfer(transferSource);
            }

            _unitOfWork.singleCurrencyAmountRepository.Update(_selectedCurrencyAmount);
            _unitOfWork.singleCurrencyAmountRepository.Update(transferSource);
            await _unitOfWork.SaveTransaction();

            LoadSelectedAmount(_selectedCurrencyAmount.Id);
            LoadTransferableAmounts();
            UpdateMultiAmountCurrency();
        }

        private async void AddCurrencyRepresentation()
        {
            var newCurrency = await _unitOfWork.currencyRepository.Get(c => c.Code == ActionsPayload.NewCurrencyRepresentation);
            if (newCurrency == null) return;

            if (ActionsPayload.MultiCurrencyAmount == null)
            {
                ActionsPayload.MultiCurrencyAmount = new MultiCurrencyAmount(_selectedCurrencyAmount, newCurrency);
            }
            else
            {
                ActionsPayload.MultiCurrencyAmount.AmountCurrencyRepresentations.Add(newCurrency.Code, _selectedCurrencyAmount.CalculateCurrencyConversion(newCurrency));
            }

            CurrencyRepresentations.Add($"{newCurrency.Code}: {ActionsPayload.MultiCurrencyAmount.AmountCurrencyRepresentations[newCurrency.Code]} {newCurrency.Symbol}");

            LoadAvailableCurrencies();
        }

        private async void UpdateMultiAmountCurrency()
        {
            CurrencyRepresentations.Clear();

            if (ActionsPayload.MultiCurrencyAmount == null) return;

            foreach(var pair in ActionsPayload.MultiCurrencyAmount.AmountCurrencyRepresentations)
            {
                var currency = await _unitOfWork.currencyRepository.Get(c => c.Code == pair.Key);
                if(currency != null)
                {
                    ActionsPayload.MultiCurrencyAmount.AmountCurrencyRepresentations[pair.Key] = _selectedCurrencyAmount.CalculateCurrencyConversion(currency);
                    CurrencyRepresentations.Add($"{currency.Code}: {ActionsPayload.MultiCurrencyAmount.AmountCurrencyRepresentations[pair.Key]} {currency.Symbol}");
                }
                   
            }
        }

        private async void ConvertCurrentAmount()
        {
            var newCurrency = await _unitOfWork.currencyRepository.Get(c => c.Code == ActionsPayload.CurrencyConversionCode);

            if (newCurrency == null) return;

            _selectedCurrencyAmount.ConvertCurrency(newCurrency);

            _unitOfWork.singleCurrencyAmountRepository.Update(_selectedCurrencyAmount);
            await _unitOfWork.SaveTransaction();

            ActionsPayload.MultiCurrencyAmount = null;
            CurrencyRepresentations.Clear();
            LoadTransferableAmounts();
            LoadAvailableCurrencies();
            LoadSelectedAmount(_selectedCurrencyAmount.Id);
        }

        private async void DeleteAmount()
        {
            _unitOfWork.singleCurrencyAmountRepository.Delete(_selectedCurrencyAmount);
            await _unitOfWork.SaveTransaction();

            _displayedCurrencyAmount.CurrencyCode = "";
            _displayedCurrencyAmount.Description = "";
            _displayedCurrencyAmount.AmountId = Guid.Empty;
            _displayedCurrencyAmount.FormattedAmount = "Deleted.";

            TransferableAmounts.Clear();
            AvailableCurrencyCodes.Clear();
            CurrencyRepresentations.Clear();
            ConversionCurrencyCodes.Clear();
            ActionsPayload = new SingleCurrencyAmountActionsPayload();
            _selectedCurrencyAmount.Id = Guid.Empty;
        }
    }
}

#pragma warning restore CS8618
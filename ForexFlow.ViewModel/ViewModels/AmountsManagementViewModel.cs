using ForexFlow.DataAccess.Repository;
using ForexFlow.Models;
using ForexFlow.ViewModel.DataTransferObjects;
using ForexFlow.ViewModel.RelayDelegats;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ForexFlow.ViewModel.ViewModels
{
    public class AmountsManagementViewModel 
    {
        #region Data Initialization & Fetching
        private readonly IUnitOfWork _unitOfWork;
        public SingleCurrencyAmountNotifier NewSingleCurrencyAmountNotifier { get; set; } = new SingleCurrencyAmountNotifier();
        public ObservableCollection<string> CurrencyCodes { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<DisplayedSingleCurrencyAmount> SingleCurrencyAmounts { get; set; } = new ObservableCollection<DisplayedSingleCurrencyAmount>();
        public ICommand SaveNewAmountCommand { get; }

        public AmountsManagementViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SaveNewAmountCommand = new RelayCommand(SaveNewAmount, CanSave);

            NewSingleCurrencyAmountNotifier.PropertyChanged += (s, e) =>
            {
                ((RelayCommand)SaveNewAmountCommand).RaiseCanExecuteChanged();
            };

            LoadCurrencyCodes();
            LoadAllAmounts();
        }

        private async void LoadCurrencyCodes()
        {
            var currencies = await _unitOfWork.currencyRepository.GetAll();
            CurrencyCodes.Clear();

            foreach (var currency in currencies)
            {
                CurrencyCodes.Add(currency.Code);
            }

            if(CurrencyCodes.Count > 0)
            {
                NewSingleCurrencyAmountNotifier.CurrencyCode = CurrencyCodes[0];
            }
        }

        private async void LoadAllAmounts()
        {
            var amounts = await _unitOfWork.singleCurrencyAmountRepository.GetAll(includeProperties:"Currency", tracked:false);
            SingleCurrencyAmounts.Clear();

            foreach(var amount in amounts)
            {
                SingleCurrencyAmounts.Add(new DisplayedSingleCurrencyAmount { 
                    AmountId = amount.Id,
                    CurrencyCode = amount.CurrencyCode, 
                    Description = amount.Description, 
                    FormattedAmount = $"{amount.Amount:F2} {amount.Currency.Symbol}",
                });
            }
        }

        #endregion

        #region Commands Definition
        private void SaveNewAmount()
        {
            var newAmount = new SingleCurrencyAmount
            {
                Id = Guid.NewGuid(),
                Description = NewSingleCurrencyAmountNotifier.Description,
                CurrencyCode = NewSingleCurrencyAmountNotifier.CurrencyCode,
                Currency = null,
                Amount = NewSingleCurrencyAmountNotifier.Amount,
            };

            _unitOfWork.singleCurrencyAmountRepository.Add(newAmount);
            _unitOfWork.SaveTransaction();

            NewSingleCurrencyAmountNotifier.Description = string.Empty;
            NewSingleCurrencyAmountNotifier.Amount = 0.00m;
            NewSingleCurrencyAmountNotifier.CurrencyCode = CurrencyCodes.FirstOrDefault();
            LoadAllAmounts();
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(NewSingleCurrencyAmountNotifier.Description) && NewSingleCurrencyAmountNotifier.Amount >= 0;
        }

        #endregion
    }
}

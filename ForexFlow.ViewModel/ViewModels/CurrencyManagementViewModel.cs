using ForexFlow.DataAccess.Repository;
using ForexFlow.ViewModel.DataTransferObjects;
using ForexFlow.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ForexFlow.ViewModel.RelayDelegats;

namespace ForexFlow.ViewModel.ViewModels
{
    public class CurrencyManagementViewModel
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CurrencyActionsPayload ActionsPayload { get; set; } = new CurrencyActionsPayload();
        public ObservableCollection<DisplayedCurrency> DisplayedCurrencies { get; set; } = new ObservableCollection<DisplayedCurrency>();

        public ICommand EditSelectCommand { get; set; }
        public ICommand UpdateExchangeRateCommand { get; set; }
        public ICommand AddNewCurrencyCommand { get; set; }
        public CurrencyManagementViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            EditSelectCommand = new RelayCommandCode(SelectCurrencyToEdit, () => true);
            UpdateExchangeRateCommand = new RelayCommand(UpdateExhangeRate, () => ActionsPayload.EditingCurrencyCode != string.Empty && ActionsPayload.ExchangeRateHolder > 0);
            AddNewCurrencyCommand = new RelayCommand(AddNewCurrency, () =>  ActionsPayload.NewCurrencyCode.Length ==  3
                                                                            && ActionsPayload.NewCurrencyName != string.Empty 
                                                                            && ActionsPayload.NewCurrencySymbol != string.Empty 
                                                                            && ActionsPayload.NewCurrencyExchangeRate > 0);

            ActionsPayload.PropertyChanged += (s, e) =>
            {
                ((RelayCommandCode)EditSelectCommand).RaiseCanExecuteChanged();
                ((RelayCommand)UpdateExchangeRateCommand).RaiseCanExecuteChanged();
                ((RelayCommand)AddNewCurrencyCommand).RaiseCanExecuteChanged();
            };

            LoadCurrencies();
        }

        public async void LoadCurrencies()
        {
            DisplayedCurrencies.Clear();

            var currencies = await _unitOfWork.currencyRepository.GetAll();

            if (currencies == null) return;

            foreach (var currency in currencies)
            {
                DisplayedCurrencies.Add(new DisplayedCurrency
                {
                    Code = currency.Code,
                    Name = currency.Name,
                    ExchangeRate = $"{currency.ExchangeRateToBase} {currency.Symbol}"
                });
            }
        }

        public async void SelectCurrencyToEdit(string currencyCode)
        {
            var currencyToUpdate = await _unitOfWork.currencyRepository.Get((c) => c.Code == currencyCode);
            if (currencyToUpdate == null) return;

            ActionsPayload.EditingCurrencyCode = currencyCode;
            ActionsPayload.ExchangeRateHolder = currencyToUpdate.ExchangeRateToBase;
        }

        public async void UpdateExhangeRate()
        {
            var currencyToUpdate = await _unitOfWork.currencyRepository.Get(c => c.Code == ActionsPayload.EditingCurrencyCode, tracked: true);
            if (currencyToUpdate == null) return;

            if(currencyToUpdate.ExchangeRateToBase != ActionsPayload.ExchangeRateHolder)
            {
                currencyToUpdate.ExchangeRateToBase = ActionsPayload.ExchangeRateHolder;
                _unitOfWork.currencyRepository.Update(currencyToUpdate);

                await _unitOfWork.SaveTransaction();

                LoadCurrencies();
            }

            ActionsPayload.EditingCurrencyCode = string.Empty;
            ActionsPayload.ExchangeRateHolder = 0.00m;
        }

        public async void AddNewCurrency()
        {

            var alreadyExists = await _unitOfWork.currencyRepository.Get(c => c.Code == ActionsPayload.NewCurrencyCode);

            if (alreadyExists != null) return;

            var newCurrency = new Currency
            {
                Code = ActionsPayload.NewCurrencyCode,
                ExchangeRateToBase = ActionsPayload.NewCurrencyExchangeRate,
                Name = ActionsPayload.NewCurrencyName,
                Symbol = ActionsPayload.NewCurrencySymbol,
            };

            _unitOfWork.currencyRepository.Add(newCurrency);
            
            await _unitOfWork.SaveTransaction();

            DisplayedCurrencies.Add(new DisplayedCurrency
            {
                Code = newCurrency.Code,
                Name = newCurrency.Name,
                ExchangeRate = $"{newCurrency.ExchangeRateToBase} {newCurrency.Symbol}"
            });

            ActionsPayload.NewCurrencyCode = string.Empty;
            ActionsPayload.NewCurrencyName = string.Empty;
            ActionsPayload.NewCurrencySymbol = string.Empty;
            ActionsPayload.NewCurrencyExchangeRate = 0.00m;
        }
    }
}

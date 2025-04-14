using ForexFlow.DataAccess.Repository.Abstraction;

namespace ForexFlow.DataAccess.Repository
{
    public interface IUnitOfWork
    {
        public ISingleCurrencyAmountRepository singleCurrencyAmountRepository { get; }
        public ICurrencyRepository currencyRepository { get; }
        public IInvoiceRepository invoiceRepository { get; }
        public Task SaveTransaction();
    }
}

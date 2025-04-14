using ForexFlow.DataAccess.Database;
using ForexFlow.DataAccess.Repository.Abstraction;
using ForexFlow.DataAccess.Repository.Implementation;

namespace ForexFlow.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _context;
        public ISingleCurrencyAmountRepository singleCurrencyAmountRepository { get; private set; }
        public ICurrencyRepository currencyRepository { get; private set; }
        public IInvoiceRepository invoiceRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            singleCurrencyAmountRepository = new SingleCurrencyAmountRepository(_context);
            currencyRepository = new CurrencyRepository(_context);
            invoiceRepository = new InvoiceRepository(_context);
        }

        public async Task SaveTransaction()
        {
            await _context.SaveChangesAsync();
        }
    }
}

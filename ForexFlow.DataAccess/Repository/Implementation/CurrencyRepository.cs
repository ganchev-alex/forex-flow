using ForexFlow.DataAccess.Database;
using ForexFlow.DataAccess.Repository.Abstraction;
using ForexFlow.Models;

namespace ForexFlow.DataAccess.Repository.Implementation
{
    public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
    {
        private ApplicationDbContext _context;

        public CurrencyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Currency currency)
        {
            _context.Update(currency);
        }
    }
}

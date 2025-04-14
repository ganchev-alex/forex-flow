using ForexFlow.DataAccess.Database;
using ForexFlow.DataAccess.Repository.Abstraction;
using ForexFlow.Models;

namespace ForexFlow.DataAccess.Repository.Implementation
{
    class SingleCurrencyAmountRepository : Repository<SingleCurrencyAmount>, ISingleCurrencyAmountRepository
    {
        private readonly ApplicationDbContext _context;

        public SingleCurrencyAmountRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(SingleCurrencyAmount amountToUpdate)
        {
            _context.Update(amountToUpdate);
        }
    }
}


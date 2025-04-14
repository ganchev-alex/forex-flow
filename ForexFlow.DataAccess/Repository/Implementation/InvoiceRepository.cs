using ForexFlow.DataAccess.Database;
using ForexFlow.DataAccess.Repository.Abstraction;
using ForexFlow.Model;

namespace ForexFlow.DataAccess.Repository.Implementation
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;
        public InvoiceRepository(ApplicationDbContext context) : base(context) { 
            _context = context;
        }
    }
}

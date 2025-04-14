using ForexFlow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexFlow.DataAccess.Repository.Abstraction
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        public void Update(Currency currency);
    }
}

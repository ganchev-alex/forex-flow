using ForexFlow.Models;

namespace ForexFlow.DataAccess.Repository.Abstraction
{
    public interface ISingleCurrencyAmountRepository : IRepository<SingleCurrencyAmount>
    {
        public void Update(SingleCurrencyAmount amountToUpdate);
    }
}

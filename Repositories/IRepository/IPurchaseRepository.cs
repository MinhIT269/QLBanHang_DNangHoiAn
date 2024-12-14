using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface IPurchaseRepository
    {
        IQueryable<Order> GetFilteredOrdersPurchase(string searchQuery, string sortCriteria);
        Task<object> GetPaymentMethodStatisticsAsync();
    }
}

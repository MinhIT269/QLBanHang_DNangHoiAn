using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface IPurchaseRepository
    {
       IQueryable<Order> GetFilteredOrdersPurchase(string searchQuery, string sortCriteria);
       Task<object> GetPaymentMethodStatisticsAsync();
    }
}

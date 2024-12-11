using PBL6_QLBH.Models;

namespace QLBanHang_API.Services.IService
{
    public interface ITransactionService
    {
        public Task AddTransactionAsync(Transaction transaction);

        public Task SaveChangeAsync();
        //public Task<int> getMaxTransactionId();

        Task<Transaction> GetTransactionByOrderIdAsync(Guid orderId);
    }
}

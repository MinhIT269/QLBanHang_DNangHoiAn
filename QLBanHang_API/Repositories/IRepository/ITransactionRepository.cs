using PBL6_QLBH.Models;

namespace QLBanHang_API.Repositories.IRepository
{
    public interface ITransactionRepository
    {
        Task AddTransactionAsync(Transaction transaction);
        public Task SaveChangeAsync();
        Task<Transaction> GetTransactionByIdAsync(Guid transactionId);
        Task<Transaction> GetTransactionByOrderIdAsync(Guid orderId);
    }
}

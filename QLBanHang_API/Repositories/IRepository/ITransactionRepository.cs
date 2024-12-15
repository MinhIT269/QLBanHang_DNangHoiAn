using PBL6_QLBH.Models;

namespace PBL6.Repositories.IRepository
{
    public interface ITransactionRepository
    {
        Task AddTransactionAsync(Transaction transaction);
        public Task SaveChangeAsync();
        Task<Transaction> GetTransactionByIdAsync(Guid transactionId);
        Task<Transaction> GetTransactionByOrderIdAsync(Guid orderId);
    }
}

using PBL6_QLBH.Models;
using QLBanHang_API.Repositories.IRepository;
using QLBanHang_API.Services.IService;
namespace QLBanHang_API.Services.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task AddTransactionAsync(Transaction transaction)
        {
            var existingTransaction = await _transactionRepository.GetTransactionByIdAsync(transaction.TransactionId);
            if (existingTransaction != null)
            {
                transaction.TransactionId = Guid.NewGuid(); // Tạo lại TransactionId nếu đã tồn tại
            }

            await _transactionRepository.AddTransactionAsync(transaction);
            await _transactionRepository.SaveChangeAsync();
        }

        public async Task<Transaction> GetTransactionByOrderIdAsync(Guid orderId)
        {
            return await _transactionRepository.GetTransactionByOrderIdAsync(orderId);
        }

        public async Task SaveChangeAsync()
        {
            await _transactionRepository.SaveChangeAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6.Repositories.Repository;
using PBL6_BackEnd.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6_BackEnd.Services.ServiceImpl
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

        //public async Task<int> getMaxTransactionId()
        //{
        //   return  await _context.Transactions
        //            .OrderByDescending(t => t.TransactionId)
        //            .Select(t => t.TransactionId)
        //            .FirstOrDefaultAsync();
        //}
    }
}

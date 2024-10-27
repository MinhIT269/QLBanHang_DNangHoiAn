using Microsoft.EntityFrameworkCore;
using PBL6_BackEnd.Services.Service;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;

namespace PBL6_BackEnd.Services.ServiceImpl
{
    public class TransactionService : ITransactionService
    {
        private readonly DataContext _context;
        public TransactionService(DataContext context) {
            _context = context;
        }
        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
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

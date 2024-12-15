using Microsoft.EntityFrameworkCore;
using PBL6.Repositories.IRepository;
using PBL6_QLBH.Data;
using PBL6_QLBH.Models;
using System;

namespace PBL6.Repositories.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataContext _context;

        public TransactionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
        {
            return await _context.Set<Transaction>().FindAsync(transactionId);
        }


        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
          
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction> GetTransactionByOrderIdAsync(Guid orderId)
        {
            return await _context.Transactions.FirstOrDefaultAsync(p => p.OrderId == orderId);
        }
    }
}

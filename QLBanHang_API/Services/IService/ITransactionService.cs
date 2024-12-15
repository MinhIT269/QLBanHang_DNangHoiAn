﻿

using PBL6_QLBH.Models;

namespace PBL6_BackEnd.Services.Service
{
    public interface ITransactionService
    {
        public Task AddTransactionAsync(Transaction transaction);

        public Task SaveChangeAsync();
        //public Task<int> getMaxTransactionId();

        Task<Transaction> GetTransactionByOrderIdAsync(Guid orderId);
    }
}

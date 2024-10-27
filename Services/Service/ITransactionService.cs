

using PBL6_QLBH.Models;

namespace PBL6_BackEnd.Services.Service
{
    public interface ITransactionService
    {
        public Task AddTransactionAsync(Transaction transaction);
        //public Task<int> getMaxTransactionId();
    }
}

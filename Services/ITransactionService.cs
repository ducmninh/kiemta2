using PCM_396.Models;

namespace PCM_396.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetAllTransactionsAsync();
        Task<List<Transaction>> GetTransactionsByFilterAsync(string? type, string? category);
        Task<Transaction?> GetTransactionByIdAsync(int id);
        Task<bool> CreateTransactionAsync(Transaction transaction);
        Task<bool> UpdateTransactionAsync(Transaction transaction);
        Task<bool> DeleteTransactionAsync(int id);
        Task<decimal> GetTotalIncomeAsync();
        Task<decimal> GetTotalExpenseAsync();
        Task<decimal> GetBalanceAsync();
    }
}

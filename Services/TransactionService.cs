using Microsoft.EntityFrameworkCore;
using PCM_396.Data;
using PCM_396.Models;

namespace PCM_396.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Creator)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetTransactionsByFilterAsync(string? type, string? category)
        {
            var query = _context.Transactions.Include(t => t.Creator).AsQueryable();

            if (!string.IsNullOrEmpty(type) && type != "Tất cả")
            {
                query = query.Where(t => t.Type == type);
            }

            if (!string.IsNullOrEmpty(category) && category != "Tất cả")
            {
                query = query.Where(t => t.Category == category);
            }

            return await query.OrderByDescending(t => t.TransactionDate).ToListAsync();
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.Creator)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> CreateTransactionAsync(Transaction transaction)
        {
            try
            {
                transaction.CreatedDate = DateTime.Now;
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTransactionAsync(Transaction transaction)
        {
            try
            {
                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            try
            {
                var transaction = await _context.Transactions.FindAsync(id);
                if (transaction == null) return false;

                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<decimal> GetTotalIncomeAsync()
        {
            return await _context.Transactions
                .Where(t => t.Type == "Thu")
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetTotalExpenseAsync()
        {
            return await _context.Transactions
                .Where(t => t.Type == "Chi")
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetBalanceAsync()
        {
            var income = await GetTotalIncomeAsync();
            var expense = await GetTotalExpenseAsync();
            return income - expense;
        }
    }
}

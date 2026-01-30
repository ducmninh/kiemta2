using Microsoft.AspNetCore.Mvc;
using PCM_396.Services;
using PCM_396.Models;

namespace PCM_396.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsApiController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsApiController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Lấy danh sách tất cả giao dịch
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        /// <summary>
        /// Lọc giao dịch theo loại và danh mục
        /// </summary>
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Transaction>>> FilterTransactions(
            [FromQuery] string? type,
            [FromQuery] string? category)
        {
            var transactions = await _transactionService.GetTransactionsByFilterAsync(type, category);
            return Ok(transactions);
        }

        /// <summary>
        /// Lấy thông tin giao dịch theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound(new { message = $"Không tìm thấy giao dịch với ID {id}" });
            }
            return Ok(transaction);
        }

        /// <summary>
        /// Lấy tổng thu nhập
        /// </summary>
        [HttpGet("stats/income")]
        public async Task<ActionResult<decimal>> GetTotalIncome()
        {
            var income = await _transactionService.GetTotalIncomeAsync();
            return Ok(new { totalIncome = income });
        }

        /// <summary>
        /// Lấy tổng chi phí
        /// </summary>
        [HttpGet("stats/expense")]
        public async Task<ActionResult<decimal>> GetTotalExpense()
        {
            var expense = await _transactionService.GetTotalExpenseAsync();
            return Ok(new { totalExpense = expense });
        }

        /// <summary>
        /// Lấy số dư hiện tại
        /// </summary>
        [HttpGet("stats/balance")]
        public async Task<ActionResult<decimal>> GetBalance()
        {
            var balance = await _transactionService.GetBalanceAsync();
            return Ok(new { balance = balance });
        }

        /// <summary>
        /// Tạo giao dịch mới
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Transaction>> CreateTransaction([FromBody] Transaction transaction)
        {
            try
            {
                var success = await _transactionService.CreateTransactionAsync(transaction);
                if (!success)
                {
                    return BadRequest(new { message = "Không thể tạo giao dịch" });
                }
                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật giao dịch
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTransaction(int id, [FromBody] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest(new { message = "ID không khớp" });
            }

            try
            {
                var success = await _transactionService.UpdateTransactionAsync(transaction);
                if (!success)
                {
                    return NotFound(new { message = $"Không tìm thấy giao dịch với ID {id}" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Xóa giao dịch
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            var success = await _transactionService.DeleteTransactionAsync(id);
            if (!success)
            {
                return NotFound(new { message = $"Không tìm thấy giao dịch với ID {id}" });
            }
            return NoContent();
        }
    }
}

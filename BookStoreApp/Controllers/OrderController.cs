using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly SqlConnection _connection;

        public OrdersController(SqlConnection connection)
        {
            _connection = connection;
        }

        [HttpPost]
        public IActionResult PlaceOrder([FromBody] OrderRequest orderRequest)
        {
            _connection.Open();

            // Check Stock
            var checkStockCmd = new SqlCommand("SELECT StockLevel FROM Books WHERE BookId = @BookId", _connection);
            checkStockCmd.Parameters.AddWithValue("@BookId", orderRequest.BookId);
            var stockLevel = (int?)checkStockCmd.ExecuteScalar();

            if (stockLevel == null || stockLevel < orderRequest.Quantity)
            {
                return BadRequest("Insufficient stock!");
            }

            // Place Order
            var orderCmd = new SqlCommand("INSERT INTO Orders (CustomerId, BookId, Quantity) VALUES (@CustomerId, @BookId, @Quantity)", _connection);
            orderCmd.Parameters.AddWithValue("@CustomerId", orderRequest.CustomerId);
            orderCmd.Parameters.AddWithValue("@BookId", orderRequest.BookId);
            orderCmd.Parameters.AddWithValue("@Quantity", orderRequest.Quantity);
            orderCmd.ExecuteNonQuery();

            // Update Stock
            var updateStockCmd = new SqlCommand("UPDATE Books SET StockLevel = StockLevel - @Quantity WHERE BookId = @BookId", _connection);
            updateStockCmd.Parameters.AddWithValue("@Quantity", orderRequest.Quantity);
            updateStockCmd.Parameters.AddWithValue("@BookId", orderRequest.BookId);
            updateStockCmd.ExecuteNonQuery();

            return Ok("Order placed successfully!");
        }
    }

    // Order Request Model
    public class OrderRequest
    {
        public int CustomerId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}

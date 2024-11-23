using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly SqlConnection _connection;

        public OrderController(SqlConnection connection)
        {
            _connection = connection;
        }

        [HttpPost]
        public IActionResult PlaceOrder([FromBody] OrderRequest orderRequest)
        {
            _connection.Open();

            // Check Price and Stock
            var checkPriceAndStockCmd = new SqlCommand("SELECT Price, StockLevel FROM Books WHERE ISBN = @ISBN", _connection);
            checkPriceAndStockCmd.Parameters.AddWithValue("@ISBN", orderRequest.BookISBN);
            var reader = checkPriceAndStockCmd.ExecuteReader();

            if (!reader.Read())
            {
                reader.Close();
                return NotFound("Book not found.");
            }

            var price = (decimal)reader["Price"];
            var stockLevel = (int)reader["StockLevel"];
            reader.Close();

            // Check if there is enough stock
            if (stockLevel < orderRequest.Quantity)
            {
                return BadRequest("Insufficient stock!");
            }

            // Calculate the total amount for the order
            var totalAmount = price * orderRequest.Quantity;

            // Place Order
            var orderCmd = new SqlCommand("INSERT INTO Orders (CustomerId, BookISBN, Quantity, TotalAmount) VALUES (@CustomerId, @BookISBN, @Quantity, @TotalAmount)", _connection);
            orderCmd.Parameters.AddWithValue("@CustomerId", orderRequest.CustomerId);
            orderCmd.Parameters.AddWithValue("@BookISBN", orderRequest.BookISBN);
            orderCmd.Parameters.AddWithValue("@Quantity", orderRequest.Quantity);
            orderCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
            orderCmd.ExecuteNonQuery();

            // Update Stock
            var updateStockCmd = new SqlCommand("UPDATE Books SET StockLevel = StockLevel - @Quantity WHERE ISBN = @ISBN", _connection);
            updateStockCmd.Parameters.AddWithValue("@Quantity", orderRequest.Quantity);
            updateStockCmd.Parameters.AddWithValue("@ISBN", orderRequest.BookISBN);
            updateStockCmd.ExecuteNonQuery();

            return Ok("Order placed successfully!");
        }
    }

    // Order Request Model
    public class OrderRequest
    {
        public int CustomerId { get; set; }
        public string BookISBN { get; set; }
        public int Quantity { get; set; }
    }
}

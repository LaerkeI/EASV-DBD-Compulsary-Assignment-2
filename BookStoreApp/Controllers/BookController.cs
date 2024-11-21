using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMongoDatabase _mongoDb;

        public BooksController(IMongoClient mongoClient)
        {
            _mongoDb = mongoClient.GetDatabase("BookDetailsDb");
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var booksCollection = _mongoDb.GetCollection<Book>("Books");
            var books = booksCollection.Find(_ => true).ToList();

            var result = new List<Book>();

            foreach (var book in books)
            {
                    result.Add(book);
                    var serializedBook = JsonConvert.SerializeObject(book);
            }

            return Ok(result);
        }
    }

    // Book Model
    public class Book
    {
        [BsonId]  // Marks this property as the MongoDB _id field
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int StockLevel { get; set; }
    }
}


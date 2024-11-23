using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StackExchange.Redis;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMongoDatabase _mongoDb;
        private readonly IDatabase _redisDb;

        public BookController(IMongoClient mongoClient, IConnectionMultiplexer redis)
        {
            _mongoDb = mongoClient.GetDatabase("BookDetailsDb");
            _redisDb = redis.GetDatabase();
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var booksCollection = _mongoDb.GetCollection<Book>("Books");
            var books = booksCollection.Find(_ => true).ToList();

            var result = new List<Book>();

            foreach (var book in books)
            {
                var cacheKey = $"Book:{book.ISBN}";
                var cachedBook = _redisDb.StringGet(cacheKey);

                if (cachedBook.HasValue)
                {
                    var cachedBookDetails = JsonConvert.DeserializeObject<Book>(cachedBook);
                    result.Add(cachedBookDetails);
                }
                else
                {
                    result.Add(book);
                    var serializedBook = JsonConvert.SerializeObject(book);
                    _redisDb.StringSet(cacheKey, serializedBook, TimeSpan.FromSeconds(10)); // Cache for 10 seconds
                }
            }
            return Ok(result);
        }
    }

    // Book Model
    public class Book
    {
        [BsonId] // Marks this property as the MongoDB _id field
        [BsonRepresentation(BsonType.ObjectId)] // Handles conversion between ObjectId and string
        public string Id { get; set; }

        [BsonElement("ISBN")]
        public string ISBN { get; set; }
        
        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("Author")]
        public string Author { get; set; }
        
        [BsonElement("Description")]
        public string Description { get; set; }
        
        [BsonElement("Price")]
        public double Price { get; set; }
    }
}



using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;
using ReadingDiaryApi.Clients;
using ReadingDiaryApi.Models;

namespace ReadingDiaryApi.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly IDBClient _dynamoDBClient;
        private readonly Client _client;

        public BooksController(IDBClient dynamoDBClient, Client client)
        {
            _client = client;
            _dynamoDBClient = dynamoDBClient;
        }
        [HttpGet]
        public async Task<IActionResult> GetBook(string name, int bookIndex)
        {
            var book = await _client.GetBookAsync(name, bookIndex);

            if (book != null)
            {
                return Ok(book);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthorBook(string name, string author, int bookIndex)
        {
            var book = await _client.GetAuthorBookAsync(name, author, bookIndex);

            if (book != null)
            {
                return Ok(book);
            }

            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> SearchAudioBookOnYouTube(string bookTitle)
        {
            string youtubeLink = await _client.GetYouTubeVideo(bookTitle);
            if (youtubeLink != null)
            {
                return Ok(new { YouTubeLink = youtubeLink });
            }

            return NotFound();
        }

        [HttpPost(Name = "PostDynamoDb")]
        public async Task<IActionResult> PostDynamoDb([FromBody] BookDB book)
        {
            var bookDb = new BookDB
            {
                UserID = book.UserID,
                Title = book.Title,
                Author = book.Author,
                Page = book.Page,
                Genre = book.Genre,
                Link = book.Link,
                Description = book.Description,
                PersonalDescription = book.PersonalDescription,
                PersonalTag = book.PersonalTag
            };
            var result = await _dynamoDBClient.CreateBookDB(bookDb);
            if (result == false)
            {
                return BadRequest("Книгу не вдалося створити");
            }
            return Ok("Книгу створено");
        }
        [HttpDelete(Name = "DeleteBookDB")]
        public async Task<IActionResult> DeleteBookDB(string UserID, string Title)
        {
            var result = await _dynamoDBClient.DeleteBookDB(UserID, Title);
            if (result == false)
            {
                return BadRequest("Книгу не вдалося видалити");
            }
            return Ok("Книгу видалено");
        }
        [HttpGet(Name = "GetBookDB")]
        public async Task<BookDB> GetBookDB(string UserID, string Title)
        {
            var result = await _dynamoDBClient.GetBookDB(UserID, Title);
            if (result == null)
                return null;
            var user = new BookDB
            {
                Title = result.Title,
                Author = result.Author,
                Page = result.Page,
                Genre = result.Genre,
                Description = result.Description,
                Link = result.Link,
                PersonalDescription = result.PersonalDescription,
                PersonalTag = result.PersonalTag
            };
            return user;
        }
        [HttpPut(Name = "UpdateDescription")]
        public async Task<IActionResult> UpdateDescription(string UserID, string Title, string Description)
        {
            var result = await _dynamoDBClient.UpdateDescription(UserID, Title, Description);
            if (result == false)
            {
                return BadRequest("Книгу не вдалося обновити");
            }
            return Ok("Книгу оновлено");
        }
        [HttpPut(Name = "UpdateTag")]
        public async Task<IActionResult> UpdateTag(string UserID, string Title, List<string> PersonalTag)
        {
            var result = await _dynamoDBClient.UpdateTag(UserID, Title, PersonalTag);
            if (result == false)
            {
                return BadRequest("Книгу не вдалося обновити");
            }
            return Ok("Книгу оновлено");
        }
        [HttpGet(Name = "GetAllDB")]
        public async Task<List<BookDB>> GetAllDynamoDb(string UserId)
        {
            var response = await _dynamoDBClient.GetAllByUserID(UserId);
            if (response == null)
                return null;
            var result = response
                .Select(x => new BookDB()
                {
                    Title = x.Title,
                    Author = x.Author,
                    Page = x.Page,
                    Genre = x.Genre,
                    Description = x.Description,
                    Link = x.Link,
                    PersonalDescription = x.PersonalDescription,
                    PersonalTag = x.PersonalTag,
                }).ToList();
            return result;
        }
    }
}
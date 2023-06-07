using ReadingDiaryApi.Models;

namespace ReadingDiaryApi.Clients
{
    public interface IDBClient
    {
        DBClient DBClient { get; set; }

        public Task<bool> CreateBookDB(BookDB book);
        public Task<bool> DeleteBookDB(string UserID, string Title);
        public Task<BookDB> GetBookDB(string UserID, string Title);
        public Task<bool> UpdateDescription(string UserID, string Title, string Descriprion);
        public Task<bool> UpdateTag(string UserID, string Title, List<string> PersonalTag);
        public Task<List<BookDB>> GetAllByUserID(string userID);
    }
}

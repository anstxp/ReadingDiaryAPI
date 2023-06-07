using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using ReadingDiaryApi.Extension;
using ReadingDiaryApi.Models;

namespace ReadingDiaryApi.Clients
{
    public class DBClient: IDBClient, IDisposable
    {
        private readonly IAmazonDynamoDB _dynamoDB;
        private string _tablename = "ReadingDiary";

        public DBClient(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDB = dynamoDB;
        }

        public IDisposable IDisposable
        {
            get => default;
            set
            {
            }
        }

        public async Task<bool> CreateBookDB(BookDB book)
        {
            var request = new PutItemRequest
            {
                TableName = _tablename,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"UserID", new AttributeValue{ S = book.UserID } },
                    {"Title", new AttributeValue{ S = book.Title} },
                    {"Author", new AttributeValue{ SS = book.Author} },
                    {"Page", new AttributeValue{ N = $"{book.Page}"} },
                    {"Genre", new AttributeValue{ SS = book.Genre} },
                    {"Description", new AttributeValue{ S = book.Description } },
                    {"Link", new AttributeValue{ S = book.Link } },
                    {"PersonalDescription", new AttributeValue{ S = book.PersonalDescription} },
                    {"PersonalTag", new AttributeValue{ SS = book.PersonalTag} },
                }
            };
            try
            {
                var response = await _dynamoDB.PutItemAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteBookDB(string UserID, string Title)
        {
            var item = new DeleteItemRequest
            {
                TableName = _tablename,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"UserID", new AttributeValue{ S = UserID} },
                    {"Title", new AttributeValue{ S = Title} }
                }
            };

            try
            {
                var response = await _dynamoDB.DeleteItemAsync(item);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
        public async Task<BookDB> GetBookDB(string UserID, string Title)
        {
            var item = new GetItemRequest
            {
                TableName = _tablename,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "UserID", new AttributeValue{S = UserID } },
                    { "Title", new AttributeValue{S = Title } }
                }
            };
            var response = await _dynamoDB.GetItemAsync(item);
            if (response.Item == null || !response.IsItemSet)
                return null;
            var result = response.Item.ToClass<BookDB>();
            return result;
        }
        public async Task<bool> UpdateDescription(string UserID, string Title, string NewPersonalDescription)
        {
            var request = new UpdateItemRequest
            {
                TableName = _tablename,
                Key = new Dictionary<string, AttributeValue>()
        {
            { "UserID", new AttributeValue { S = UserID } },
            { "Title", new AttributeValue { S = Title } }
        },
                UpdateExpression = "SET PersonalDescription = :newPersonalDescription",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
        {
            { ":newPersonalDescription", new AttributeValue { S = NewPersonalDescription } }
        }
            };

            try
            {
                var response = await _dynamoDB.UpdateItemAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateTag(string UserID, string Title, List<string> PersonalTag)
        {
            var request = new UpdateItemRequest
            {
                TableName = _tablename,
                Key = new Dictionary<string, AttributeValue>()
        {
            { "UserID", new AttributeValue { S = UserID } },
            { "Title", new AttributeValue { S = Title } }
        },
                UpdateExpression = "SET PersonalTag = :newPersonalTag",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
        {
            { ":newPersonalTag", new AttributeValue { SS = PersonalTag } }
        }
            };

            try
            {
                var response = await _dynamoDB.UpdateItemAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<BookDB>> GetAllByUserID(string userID)
        {
            var result = new List<BookDB>();
            var request = new ScanRequest
            {
                TableName = _tablename,
                FilterExpression = "UserID = :userID",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
        {
            { ":userID", new AttributeValue { S = userID } }
        }
            };
            var response = await _dynamoDB.ScanAsync(request);
            if (response.Items == null || response.Items.Count == 0)
                return null;
            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                result.Add(item.ToClass<BookDB>());
            }
            return result;
        }
        public void Dispose()
        {
            _dynamoDB.Dispose();
        }
    }
}

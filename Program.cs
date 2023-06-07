using Amazon.Runtime;
using Amazon.DynamoDBv2;
using Amazon;
using ReadingDiaryApi.Constants;
using ReadingDiaryApi.Clients;
using Amazon.DynamoDBv2.DataModel;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);
var credentials = new BasicAWSCredentials(Constant.accesskeyDB, Constant.secretaccesskeyDB);
var config = new AmazonDynamoDBConfig()
{
    RegionEndpoint = RegionEndpoint.USEast1
};
var client = new AmazonDynamoDBClient(credentials, config);
builder.Services.AddSingleton<Client>();
builder.Services.AddSingleton<IAmazonDynamoDB>(client);
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddSingleton<IDBClient, DBClient>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

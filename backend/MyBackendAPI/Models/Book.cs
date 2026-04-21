using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyBackendAPI.Models;

public class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Title")] // Így a DB-ben is 'Title' lesz, követve a C# property nevet
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public int Year { get; set; }
}
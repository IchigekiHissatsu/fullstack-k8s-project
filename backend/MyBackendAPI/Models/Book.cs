using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyBackendAPI.Models;

public class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")] // a DB-ben 'Name' lesz a mező neve
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public int Year { get; set; }
}
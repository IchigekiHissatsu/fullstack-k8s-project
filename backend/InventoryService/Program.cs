using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Itt nincs szükség semmilyen OpenAPI vagy Swagger hívásra

var mongoUri = Environment.GetEnvironmentVariable("MONGODB_URI") ?? "mongodb://localhost:27017";
var mongoClient = new MongoClient(mongoUri);
var database = mongoClient.GetDatabase("LibraryDb");
var booksCollection = database.GetCollection<dynamic>("Books");

app.MapGet("/api/inventory/count", async () => {
    try {
        var count = await booksCollection.CountDocumentsAsync(FilterDefinition<dynamic>.Empty);
        return Results.Ok(new { count, service = "InventoryService-v1" });
    }
    catch (Exception ex) {
        return Results.Problem($"Adatbázis hiba: {ex.Message}");
    }
});

app.MapGet("/", () => "Inventory Service fut (v1)");

app.Run();
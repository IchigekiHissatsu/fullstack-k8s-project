using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var mongoUri = Environment.GetEnvironmentVariable("MONGODB_URI") ?? "mongodb://localhost:27017";
var mongoClient = new MongoClient(mongoUri);

// ITT A LÉNYEG: myappdb kell!
var database = mongoClient.GetDatabase("myappdb");
var booksCollection = database.GetCollection<dynamic>("Books");

app.MapGet("/api/inventory/count", async () => {
    try {
        var count = await booksCollection.CountDocumentsAsync(FilterDefinition<dynamic>.Empty);
        return Results.Ok(new { count, service = "InventoryService-v5", status = "Sikeres!" });
    }
    catch (Exception ex) {
        return Results.Problem($"Adatbázis hiba: {ex.Message}");
    }
});

app.MapGet("/", () => "Inventory Service fut (v5)");

app.Run();
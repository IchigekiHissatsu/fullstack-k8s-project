using MongoDB.Driver;
using MyBackendAPI.Services; // Ez kell a BookService-hez

var builder = WebApplication.CreateBuilder(args);

// --- 1. MONGODB BEÁLLÍTÁSA ---
var mongoUri = Environment.GetEnvironmentVariable("MONGO_URI") 
               ?? "mongodb://dbuser:dbpassword@mongodb.my-app-space.svc.cluster.local:27017/myappdb";

builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoUri));
builder.Services.AddScoped(sp => sp.GetRequiredService<IMongoClient>().GetDatabase("myappdb"));

// --- 2. SERVICE-EK REGISZTRÁCIÓJA ---
builder.Services.AddScoped<BookService>(); // A BookService regisztrálása
builder.Services.AddControllers();         // A Controller-ek támogatása

// Swagger és alapok
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 3. MIDDLEWARE ÉS ÚTVONALAK ---

// Swagger a teszthez
app.UseSwagger();
app.UseSwaggerUI();

// Ez mondja meg a .NET-nek, hogy keresse meg a Controllers mappában a BooksController-t
app.MapControllers(); 

// Egyszerű teszt végpontok (Minimal API stílusban)
app.MapGet("/db-test", async (IMongoDatabase db) => {
    try {
        await db.RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{ping:1}");
        return Results.Ok(new { status = "success", message = "MongoDB kapcsolat OK!" });
    }
    catch (Exception ex) {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/", () => "A .NET Backend szalad és a Controller-ek aktívak!");

app.Run();
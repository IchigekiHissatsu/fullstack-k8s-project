using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// MongoDB beállítása
var mongoUri = Environment.GetEnvironmentVariable("MONGO_URI") 
               ?? "mongodb://dbuser:dbpassword@mongodb.my-app-space.svc.cluster.local:27017/myappdb";

builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoUri));
builder.Services.AddScoped(sp => sp.GetRequiredService<IMongoClient>().GetDatabase("myappdb"));

// Swagger és alapok
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger ell a teszthez
app.UseSwagger();
app.UseSwaggerUI();

// VÉGPONTOK
app.MapGet("/db-test", async (IMongoDatabase db) => {
    try {
        await db.RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{ping:1}");
        return Results.Ok(new { status = "success", message = "MongoDB kapcsolat OK!" });
    }
    catch (Exception ex) {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/", () => "A .NET Backend szalad!");

app.Run();